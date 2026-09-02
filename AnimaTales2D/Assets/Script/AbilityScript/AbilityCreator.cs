using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityCreator : MonoBehaviour
{
    // Cumulative reroll odds: bronze/silver/gold/diamond tiers.
    private const float BronzeThreshold = 0.75f;
    private const float SilverThreshold = 0.88f;
    private const float GoldThreshold = 0.98f;

    private const string AbilityPrefabPathFormat = "Minwoo/Ability/{0}Ability";
    private const string AbilityImageChildName = "AbilityImage";
    private const string AbilityTextChildName = "AbilityTxt";
    private const int AbilityChoiceCount = 3;

    private static readonly Vector3 SelectedAbilityScale = new Vector3(1.2f, 1.2f, 1.2f);

    [SerializeField]
    GameObject[] abilitys;
    [SerializeField]
    TextMeshProUGUI[] rerollTxt = new TextMeshProUGUI[3];
    [SerializeField]
    AbilityReroll[] abilityReroll;
    List<AbilitySO> exceptAbility;
    [SerializeField]
    List<AbilitySO> bronzeList;
    [SerializeField]
    List<AbilitySO> silverList;
    [SerializeField]
    List<AbilitySO> goldList;
    [SerializeField]
    List<AbilitySO> diamondList;
    [SerializeField]
    private AudioClip rerollClip;
    [SerializeField]
    private AudioClip selectClip;

    float duration = 1f;

    void Start()
    {
        InitAbility();
        for (int i = 0; i < AbilityChoiceCount; i++)
        {
            SpawnAbility(i, exceptAbility[i]);
            rerollTxt[i].text = abilityReroll[i].rerollCnt.ToString();
        }
    }

    public void Reroll()
    {
        AudioManager.Instance.PlaySFX(rerollClip);
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
        int selectNum = int.Parse(selectedButton.name.Substring(6, 1));
        if (abilityReroll[selectNum].rerollCnt <= 0)
        {
            return;
        }

        AbilitySO newAbility = AbilityChoice();
        exceptAbility[selectNum] = newAbility;
        SpawnAbility(selectNum, newAbility);
        abilityReroll[selectNum].UseReroll();
        rerollTxt[selectNum].text = abilityReroll[selectNum].rerollCnt.ToString();
    }

    // Instantiates the ability prefab for a slot, wires up its visuals/click handler, and replaces the previous object.
    // Shared by Start() (initial roll) and Reroll() (re-roll of a single slot).
    private void SpawnAbility(int index, AbilitySO ability)
    {
        GameObject oldObj = abilitys[index];
        GameObject newObj = Instantiate(
            Resources.Load<GameObject>(string.Format(AbilityPrefabPathFormat, ability.data.id)),
            oldObj.transform.position,
            Quaternion.identity,
            this.transform);

        newObj.GetComponent<AbilityHolder>().abilitySO = ability;
        newObj.transform.GetComponentsInChildren<Image>(true)
            .FirstOrDefault(t => t.name == AbilityImageChildName).sprite = ability.data.icon;
        newObj.transform.Find(AbilityTextChildName).GetComponent<TextMeshProUGUI>().text = ability.data.description;
        newObj.GetComponent<Button>().onClick.AddListener(SelectAbility);

        abilitys[index] = newObj;
        Destroy(oldObj);
    }

    private void InitAbility()
    {
        exceptAbility = new List<AbilitySO>();
        for (int i = 0; i < AbilityChoiceCount; i++)
        {
            exceptAbility.Add(AbilityChoice());
        }
    }

    private AbilitySO AbilityChoice()
    {
        float odds = Random.Range(0f, 1f);
        if (odds < BronzeThreshold) return PickUnused(bronzeList);
        if (odds < SilverThreshold) return PickUnused(silverList);
        if (odds < GoldThreshold) return PickUnused(goldList);
        return PickUnused(diamondList);
    }

    // Picks a random entry from the list that hasn't already been offered this round.
    private AbilitySO PickUnused(List<AbilitySO> list)
    {
        AbilitySO picked;
        do
        {
            picked = list[Random.Range(0, list.Count)];
        } while (exceptAbility.Contains(picked));
        return picked;
    }

    public void SelectAbility()
    {
        AudioManager.Instance.PlayBGM(selectClip);
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        Transform focus = GameObject.Find("FocusTarget").transform;
        GameObject.Find("Game Manager").GetComponent<AbilityManager>().GetSymbol(selected.GetComponent<AbilityHolder>().abilitySO);
        selected.GetComponent<AbilityVibrator>().StopVib();

        foreach (GameObject obj in abilitys)
        {
            if (obj != selected)
            {
                obj.SetActive(false);
            }
        }
        foreach (var obj in abilityReroll)
        {
            obj.gameObject.SetActive(false);
        }

        StartCoroutine(AbilityMove(selected.GetComponent<RectTransform>(), selected.transform.localPosition, focus.localPosition, selected.transform.localScale));

        CanvasGroup nextBattleButton = GameObject.Find("Next Battle Button").GetComponent<CanvasGroup>();
        nextBattleButton.interactable = true;
        nextBattleButton.alpha = 1f;
    }

    IEnumerator AbilityMove(RectTransform rt, Vector2 fromPos, Vector2 toPos, Vector3 fromScale)
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float lerp = t / duration;

            rt.anchoredPosition = Vector2.Lerp(fromPos, toPos, lerp);
            rt.localScale = Vector3.Lerp(fromScale, SelectedAbilityScale, lerp);

            yield return null;
        }
        rt.anchoredPosition = toPos;
        rt.localScale = SelectedAbilityScale;
    }
}
