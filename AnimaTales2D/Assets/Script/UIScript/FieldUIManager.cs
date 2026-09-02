using UnityEngine;

public class FieldUIManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;

    public void ToggleInventory()
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }
}