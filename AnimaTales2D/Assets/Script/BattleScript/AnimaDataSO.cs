using System.Collections.Generic;
using BansheeGz.BGDatabase;
using UnityEngine;

public class AnimaDataSO : ScriptableObject
{
    public bool Animadie = false;
    public bool isAlly = false;
    public bool turnCheck = false;
    public bool isTomb = false;
    public bool isBoss = false;
    public bool isClone = false;
    public int level = 1;
    public string Name;
    public float Maxstamina = 1;
    public float Stamina = 1;
    public float Shield = 0f;
    public float Damage = 1;
    public int DropGold = 1;
    public float Speed = 1;
    public float DropRate = 1;
    public int MaxSkill_pp = 10;
    public int Skill_pp = 10;
    public float defHP;
    public float defAP;
    public float defDP;
    public float defSP;
    public string Objectfile;
    public int location = -1;
    public float Defense = 0;
    public float weight;
    public int enemyIndex= -1;
    public int mood = -1;
    public string type = "";
    public List <string> skillName = new ();
    public List<Sprite> skillSprite = new();
    public string attackName = "";
    public int[] maxLevel = new int[10]{ 14, 20, 27, 35, 43, 52, 60, 70, 80, 100 };
    public Dictionary<string, float> tmpAbility = new Dictionary<string, float>();
    public void Initialize(string name, int level) => LoadFromTable("Anima", name, level, 1f);

    public void TutorInitialize(string name, int level) => LoadFromTable("TutorialAnima", name, level, 1f);

    public void GetAnima(string name, int level) => LoadFromTable("Anima", name, level, 0.4f);

    // Shared loader behind Initialize/TutorInitialize/GetAnima: they only differ in which
    // BGDatabase table to read from and what fraction of max stamina to start with.
    private void LoadFromTable(string tableName, string name, int level, float staminaFraction)
    {
        Name = name;
        var database = BGRepo.I;
        var animaTable = database.GetMeta(tableName);
        animaTable.ForEachEntity(entity => {
            if (entity.Get<string>("name") == name)
            {
                mood = entity.Get<int>("Mood");
                this.level = level;
                weight = entity.Get<float>("Weight");
                defHP = entity.Get<float>("HP");
                defAP = entity.Get<float>("AP");
                defDP = entity.Get<float>("DP");
                defSP = entity.Get<float>("SP");
                Maxstamina = Mathf.Ceil(CalcStat(level, weight, defHP));
                Stamina = Maxstamina * staminaFraction;
                Damage = Mathf.Ceil(CalcStat(level, weight, defAP));
                Defense = Mathf.Ceil(CalcStat(level, weight, defDP));
                DropGold = entity.Get<int>("DropGold");
                Speed = Mathf.Ceil(CalcStat(level, weight, defSP));
                DropRate = entity.Get<float>("DropRate");
                Objectfile = entity.Get<string>("Objectfile");
                attackName = entity.Get<string>("Attack");
                if (entity.Get<List<string>>("Skill") != null)
                {
                    skillName = new List<string>(entity.Get<List<string>>("Skill"));
                    skillSprite.Clear();
                    foreach (var skill in skillName)
                    {
                        skillSprite.Add(Resources.Load<Sprite>($"AnimaSkillImage/{skill}"));
                    }
                }
                type = entity.Get<string>("Type");
            }
        });
    }
    public float CalcStat(int level, float weight, float stat)
    {
        //math.ceil(((2*j)*(j+0.9))*(k * math.sqrt(math.sqrt(pow(i,3))) + k*math.sqrt(math.sqrt(pow(j, i)))))
        
        return Mathf.Ceil(((2f * weight) * (weight + 0.9f)) * (stat * Mathf.Sqrt(Mathf.Sqrt(Mathf.Pow(level, 3f)))) + stat * Mathf.Sqrt(Mathf.Sqrt(Mathf.Pow(weight, level))));
    }
    public void LevelUp()
    {
        if (maxLevel[mood] > level)
        {
            level++;
            Maxstamina = Mathf.Ceil(CalcStat(level, weight, defHP));
            Damage = CalcStat(level, weight, defAP);
            Defense = CalcStat(level, weight, defDP);
            Speed = CalcStat(level, weight, defSP);
        }
        
    }
}
