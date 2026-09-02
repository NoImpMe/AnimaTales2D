using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParserData : MonoBehaviour
{
    bool parsercheck = false;
    [SerializeField]
    GameObject battleParser;
    [SerializeField]
    GameObject battleLog;
    [SerializeField]
    GameObject maxButton;
    [SerializeField]
    GameObject minButton;
    [SerializeField]
    Button damageButton;
    [SerializeField]
    Button healButton;
    List<GameObject> allyDamage = new List<GameObject>();
    List<GameObject> allyHeal = new List<GameObject>();
    List<GameObject> enemyDamage= new List<GameObject>();
    List<GameObject> enemyHeal = new List<GameObject>();
    
    public void OnClickParser()
    {
        if (!parsercheck)
        {
            battleParser.GetComponent<CanvasGroup>().alpha = 1;
            battleParser.GetComponent<CanvasGroup>().interactable = true;
            parsercheck = true;
            for (int i = 0; i < 3; i++)
            {
                if (GameObject.Find($"Ally{i}Name") != null)
                {
                    allyDamage.Add(GameObject.Find($"A{i}Damage"));
                    allyHeal.Add(GameObject.Find($"A{i}Heal"));
                }
            }
            for (int i = 0; i < 3; i++)
            {
                if (battleParser.transform.Find($"Enemy{i}Name") != null)
                {
                    enemyDamage.Add(GameObject.Find($"E{i}Damage"));
                    enemyHeal.Add(GameObject.Find($"E{i}Heal"));
                }
            }
        }
        else
        {
            battleParser.GetComponent<CanvasGroup>().alpha = 0;
            battleParser.GetComponent<CanvasGroup>().interactable = false;
            parsercheck = false;
        }
    }
    public void DamageButton()
    {
        damageButton.interactable = false;
        healButton.interactable = true;
        SetGroupAlpha(allyDamage, 1);
        SetGroupAlpha(enemyDamage, 1);
        SetGroupAlpha(allyHeal, 0);
        SetGroupAlpha(enemyHeal, 0);
    }
    public void HealButton()
    {
        healButton.interactable = false;
        damageButton.interactable = true;
        SetGroupAlpha(allyDamage, 0);
        SetGroupAlpha(enemyDamage, 0);
        SetGroupAlpha(allyHeal, 1);
        SetGroupAlpha(enemyHeal, 1);
    }

    private static void SetGroupAlpha(List<GameObject> group, float alpha)
    {
        for (int i = 0; i < group.Count; i++)
        {
            group[i].GetComponent<CanvasGroup>().alpha = alpha;
        }
    }
    public void OpenLog()
    {
        battleLog.GetComponent<CanvasGroup>().alpha = 1;
        battleLog.GetComponent <CanvasGroup>().interactable = true;
        maxButton.SetActive(false);
        minButton.SetActive(true);
    }
    public void CloseLog()
    {
        battleLog.GetComponent<CanvasGroup>().alpha = 0;
        battleLog.GetComponent<CanvasGroup>().interactable = false;
        minButton.SetActive(false);
        maxButton.SetActive(true) ;
    }
}
