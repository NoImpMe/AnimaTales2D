using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AnimaInventoryManager : MonoBehaviour
{
    public static AnimaInventoryManager Instance { get; private set; }

    public PlayerInfo playerInfo;

    public event Action OnAnimaInventoryChanged;
    public event Action<AnimaDataSO> OnPartyAddFailed;

    private MixManager mixManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        playerInfo = ScriptableObject.CreateInstance<PlayerInfo>();
        if (!File.Exists(Path.Combine(Application.persistentDataPath, "save.dat")))
        {
            Debug.Log("¾øÀ½");
            DBUpdater.Save();
        }
        else
        {
            DBUpdater.Load();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public List<AnimaDataSO> GetAllAnima()
    {
        var all = new List<AnimaDataSO>(playerInfo.haveAnima);
        all.AddRange(playerInfo.battleAnima);
        return all;
    }

    public List<AnimaDataSO> GetActiveAnima() => new List<AnimaDataSO>(playerInfo.battleAnima);
    public List<AnimaDataSO> GetHaveAnima() => new List<AnimaDataSO>(playerInfo.haveAnima);

    public bool IsAnimaDefeated(AnimaDataSO anima)
    {
        return anima != null && (anima.Animadie || anima.Stamina <= 0);
    }

    public void AddAnima(AnimaDataSO anima)
    {
        if (anima != null && !playerInfo.haveAnima.Contains(anima))
        {
            playerInfo.haveAnima.Add(anima);
            OnAnimaInventoryChanged?.Invoke();
        }
    }

    public void SetActiveAnima(List<AnimaDataSO> selected)
    {
        if (selected == null) return;

        playerInfo.battleAnima.Clear();
        foreach (var anima in selected)
        {
            if (anima != null)
            {
                playerInfo.battleAnima.Add(anima);
            }
        }

        OnAnimaInventoryChanged?.Invoke();
    }

    public void SwapSlots(AnimaSlotUI fromSlot, AnimaSlotUI toSlot)
    {
        if (fromSlot == null || toSlot == null) return;

        var fromAnima = fromSlot.AnimaData;
        var toAnima = toSlot.AnimaData;

        if (fromAnima == null && toAnima == null) return;

        var fromType = fromSlot.SlotType;
        var toType = toSlot.SlotType;

        var fromList = fromType == InventorySlotType.Inventory ? playerInfo.haveAnima : playerInfo.battleAnima;
        var toList = toType == InventorySlotType.Inventory ? playerInfo.haveAnima : playerInfo.battleAnima;

        if (fromType == InventorySlotType.Inventory && toType == InventorySlotType.Party && IsAnimaDefeated(fromAnima))
        {
            OnPartyAddFailed?.Invoke(fromAnima);
            return;
        }

        int fromIndex = fromAnima != null ? fromList.IndexOf(fromAnima) : -1;
        int toIndex = toAnima != null ? toList.IndexOf(toAnima) : -1;

        if (fromType != toType)
        {
            if (!HandleCrossTypeSwap(fromSlot, toSlot, fromType, toType, fromAnima, toAnima, fromIndex, toIndex))
                return;
        }
        else if (fromAnima != null && toAnima != null && fromIndex >= 0 && toIndex >= 0)
        {
            var temp = fromList[fromIndex];
            fromList[fromIndex] = toList[toIndex];
            toList[toIndex] = temp;
        }

        OnAnimaInventoryChanged?.Invoke();
    }

    public void InvenChanged()
    {
        OnAnimaInventoryChanged?.Invoke();
    }

    // Dispatches a cross-type slot swap to its handler. Returns false when the swap failed
    // and SwapSlots should abort without raising OnAnimaInventoryChanged.
    private bool HandleCrossTypeSwap(AnimaSlotUI fromSlot, AnimaSlotUI toSlot, InventorySlotType fromType, InventorySlotType toType,
        AnimaDataSO fromAnima, AnimaDataSO toAnima, int fromIndex, int toIndex)
    {
        if (fromType == InventorySlotType.Inventory && toType == InventorySlotType.Party)
            return HandleInventoryToParty(fromAnima, toAnima, toIndex);

        if (fromType == InventorySlotType.Party && toType == InventorySlotType.Inventory)
            return HandlePartyToInventory(fromAnima, toAnima, fromIndex, toIndex);

        if (fromType == InventorySlotType.Inventory && toType == InventorySlotType.Main)
        {
            EnsureMixManager();
            if (mixManager.mainAnima != null)
            {
                playerInfo.haveAnima.Add(toAnima);
            }
            playerInfo.haveAnima.Remove(fromAnima);
            mixManager.mainAnima = fromAnima;
            toSlot.AnimaData = fromAnima;
            return true;
        }

        if (fromType == InventorySlotType.Inventory && toType == InventorySlotType.Sub)
        {
            EnsureMixManager();
            if (mixManager.subAnima != null)
            {
                playerInfo.haveAnima.Add(toAnima);
            }
            playerInfo.haveAnima.Remove(fromAnima);
            mixManager.subAnima = fromAnima;
            toSlot.AnimaData = fromAnima;
            return true;
        }

        if (fromType == InventorySlotType.Main && toType == InventorySlotType.Sub)
        {
            var tmp = mixManager.subAnima;
            mixManager.subAnima = mixManager.mainAnima;
            toSlot.AnimaData = mixManager.mainAnima;
            mixManager.mainAnima = tmp;
            fromSlot.AnimaData = tmp;
            return true;
        }

        if (fromType == InventorySlotType.Sub && toType == InventorySlotType.Main)
        {
            var tmp = mixManager.subAnima;
            mixManager.subAnima = mixManager.mainAnima;
            fromSlot.AnimaData = mixManager.mainAnima;
            mixManager.mainAnima = tmp;
            toSlot.AnimaData = tmp;
            return true;
        }

        if (fromType == InventorySlotType.Main && toType == InventorySlotType.Inventory)
        {
            playerInfo.haveAnima.Add(fromAnima);
            fromSlot.AnimaData = null;
            mixManager.mainAnima = null;
            return true;
        }

        if (fromType == InventorySlotType.Sub && toType == InventorySlotType.Inventory)
        {
            playerInfo.haveAnima.Add(fromAnima);
            fromSlot.AnimaData = null;
            mixManager.subAnima = null;
            return true;
        }

        // Unrecognized combination: no state change, matches original fall-through behavior.
        return true;
    }

    private bool HandleInventoryToParty(AnimaDataSO fromAnima, AnimaDataSO toAnima, int toIndex)
    {
        if (fromAnima == null) return true;

        if (IsAnimaDefeated(fromAnima))
        {
            OnPartyAddFailed?.Invoke(fromAnima);
            return false;
        }

        if (toAnima == null && playerInfo.battleAnima.Count < playerInfo.maxAnimaNum)
        {
            playerInfo.haveAnima.Remove(fromAnima);
            playerInfo.battleAnima.Add(fromAnima);
        }
        else if (toIndex >= 0)
        {
            playerInfo.haveAnima.Remove(fromAnima);
            playerInfo.battleAnima[toIndex] = fromAnima;
            playerInfo.haveAnima.Add(toAnima);
        }

        return true;
    }

    private bool HandlePartyToInventory(AnimaDataSO fromAnima, AnimaDataSO toAnima, int fromIndex, int toIndex)
    {
        if (fromAnima == null || fromIndex < 0) return true;

        if (toAnima == null)
        {
            playerInfo.battleAnima.Remove(fromAnima);
            playerInfo.haveAnima.Add(fromAnima);
        }
        else if (toIndex >= 0)
        {
            if (IsAnimaDefeated(toAnima))
            {
                OnPartyAddFailed?.Invoke(toAnima);
                return false;
            }

            playerInfo.battleAnima[fromIndex] = toAnima;
            playerInfo.haveAnima[toIndex] = fromAnima;
        }

        return true;
    }

    private void EnsureMixManager()
    {
        if (mixManager == null)
            mixManager = GameObject.Find("MixManager").GetComponent<MixManager>();
    }
}
