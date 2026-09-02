using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class StatusSync : MonoBehaviour
{
    IBattleManager battleManager;
    List<AnimaActions> battleAlly;
    List<EnemyActions> battleEnemy;
    string objname;
    public int idx = -1;
    public int dieanima = 0;
    StringBuilder allyBuff = new StringBuilder();
    StringBuilder enemyBuff = new StringBuilder();
    StringBuilder allyDebuff = new StringBuilder();
    StringBuilder enemyDebuff = new StringBuilder();

    // Cached child references so OnEnable (invoked every FixedUpdate, see below) doesn't
    // repeat a transform.Find + GetComponent hierarchy walk for each field every tick.
    TextMeshProUGUI nameText;
    TextMeshProUGUI levelText;
    TextMeshProUGUI hpText;
    TextMeshProUGUI typeText;
    TextMeshProUGUI buffText;
    TextMeshProUGUI debuffText;

    void Awake()
    {
        objname = transform.parent.name;
        battleManager = GameObject.Find("BattleManager").GetComponent<IBattleManager>();
        idx = int.Parse(objname.Substring(objname.Length - 1, 1));

        nameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        levelText = transform.Find("Level").GetComponent<TextMeshProUGUI>();
        hpText = transform.Find("Hp").GetComponent<TextMeshProUGUI>();
        typeText = transform.Find("Type").GetComponent<TextMeshProUGUI>();
        buffText = transform.Find("Buff").GetComponent<TextMeshProUGUI>();
        debuffText = transform.Find("Debuff").GetComponent<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        if (this != null)
        {
            this.enabled = false;
            this.enabled = true;
        }
    }

    void OnEnable()
    {
        if (idx != 0)
        {
            idx = int.Parse(objname.Substring(objname.Length - 1, 1)) - dieanima;
        }

        var buffList = battleManager.BuffManager.GetBuffList();

        if (objname.StartsWith("A"))
        {
            battleAlly = battleManager.AllyActions;
            RefreshDisplay(battleAlly[idx].animaData, buffList, allyBuff, allyDebuff);
        }
        else
        {
            battleEnemy = battleManager.EnemyActions;
            RefreshDisplay(battleEnemy[idx].animaData, buffList, enemyBuff, enemyDebuff);
        }
    }

    private void RefreshDisplay(AnimaDataSO anima, List<Buff> buffList, StringBuilder buffBuilder, StringBuilder debuffBuilder)
    {
        nameText.text = anima.Name;
        levelText.text = "Lv. " + anima.level.ToString();
        hpText.text = Mathf.FloorToInt(anima.Stamina) + " / " + Mathf.FloorToInt(anima.Maxstamina).ToString();

        for (int i = 0; i < buffList.Count; i++)
        {
            if (!ReferenceEquals(buffList[i].target, anima)) continue;

            if (buffList[i].distinct == 0)
            {
                buffBuilder.AppendLine(string.Join(", ", buffList[i].type));
            }
            else
            {
                debuffBuilder.AppendLine(string.Join(", ", buffList[i].type));
            }
        }

        typeText.text = anima.type;
        buffText.text = buffBuilder.ToString();
        debuffText.text = debuffBuilder.ToString();
        buffBuilder.Clear();
        debuffBuilder.Clear();
    }
}
