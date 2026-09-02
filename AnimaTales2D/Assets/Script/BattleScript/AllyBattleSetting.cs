using System.Collections.Generic;
using BansheeGz.BGDatabase;
using UnityEngine;

public class AllyBattleSetting : MonoBehaviour,IAllyBattleSetting
{
    public GameObject Canvas { get; set; }
    public List<GameObject> AllyObjPrefab { get; set; }
    public List<GameObject> AllyInstance { get; set; }
    public List<GameObject> AllyInfoPrefab { get; set; }
    public List<GameObject> AllyInfoInstance { get; set; }
    public string ObjName { get; set; }
    public PlayerInfo PlayerInfo { get; private set; }
    public GameObject Prefab { get; set; }
    public List<GameObject> AllyHpPrefab { get; set; }
    public List<GameObject> AllyHpInstance { get; set; }

    public List<float> DamageX { get; set; }
    public List<float> DamageY { get; set; }

    public List<GameObject> AllyParserPrefab { get; set; }
    public List<GameObject> AllyParserInstance { get; set; }
    public GameObject BattleParser { get; set; }
    public BattleManager BattleManager { get; private set; }

    public void initialize()
    {
        AllyInstance = new List<GameObject>();
        AllyObjPrefab = new List<GameObject>();
        AllyHpPrefab = new List<GameObject>();
        AllyHpInstance = new List<GameObject>();
        AllyInfoInstance = new List<GameObject>();
        AllyInfoPrefab = new List<GameObject>();
        AllyParserPrefab = new List<GameObject>();
        AllyParserInstance = new List<GameObject>();
        DamageX = new List<float>();
        DamageY = new List<float>();
        BattleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        BattleParser = GameObject.Find("Battle Parser");
        PlayerInfo = BattleManager.playerInfo;
        Canvas = GameObject.Find("Main Battle UI");
    }

    public void SpawnAlly()
    {
        var database = BGRepo.I;
        var animaTable = database.GetMeta("Anima");
        for (int i = 0; i < PlayerInfo.battleAnima.Count; i++)
        {
            AllyObjPrefab.Add(Resources.Load<GameObject>("Anima/" + PlayerInfo.battleAnima[i].Objectfile));
            AllyHpPrefab.Add(Resources.Load<GameObject>("Minwoo/AllyAnimaHP"));
            AllyInfoPrefab.Add(Resources.Load<GameObject>($"Minwoo/Ally{i}"));
            AllyParserPrefab.Add(Resources.Load<GameObject>($"Minwoo/Battle Parser/Ally{i}Name"));
        }

        if (AllyObjPrefab.Count == 3)
        {
            for (int i = 0; i < AllyObjPrefab.Count; i++)
            {
                SpawnAllyAt(i, new Vector3((i * 3.5f) - 3.5f, -2.2f, 0f), (i * 380f) - 380f, false);
            }
        }
        else if (AllyObjPrefab.Count == 2)
        {
            for (int i = 0; i < AllyObjPrefab.Count; i++)
            {
                SpawnAllyAt(i, new Vector3((i * 3.5f) - 1.75f, -2.2f, 0f), (i * 380f) - 200f, false);
            }
        }
        else
        {
            SpawnAllyAt(0, new Vector3(0f, -2.2f, 0f), 0f, true);
        }
    }

    // Instantiates the ally's world sprite, HP bar, info panel and parser-name entry at index i,
    // and strips the "(Clone)" suffix Unity appends to instantiated GameObject names.
    private void SpawnAllyAt(int i, Vector3 worldPosition, float hpAnchorX, bool rotate180)
    {
        AllyInstance.Add(Instantiate(AllyObjPrefab[i], worldPosition, Quaternion.identity));
        AllyInstance[i].GetComponent<SpriteRenderer>().sortingOrder = -1;
        if (rotate180)
        {
            AllyInstance[i].transform.Rotate(0, 180f, 0);
        }
        StripCloneSuffix(AllyInstance[i]);

        AllyHpInstance.Add(Instantiate(AllyHpPrefab[i], Vector3.zero, Quaternion.identity, Canvas.transform));
        AllyHpInstance[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(hpAnchorX, -390f, 0f);
        StripCloneSuffix(AllyHpInstance[i], i.ToString());

        AllyInfoInstance.Add(Instantiate(AllyInfoPrefab[i], Canvas.transform));
        StripCloneSuffix(AllyInfoInstance[i]);

        AllyParserInstance.Add(Instantiate(AllyParserPrefab[i], BattleParser.transform));
        StripCloneSuffix(AllyParserInstance[i]);
    }

    private static void StripCloneSuffix(GameObject obj, string appendSuffix = "")
    {
        int index = obj.name.IndexOf("(Clone)");
        obj.name = obj.name.Substring(0, index) + appendSuffix;
    }
}
