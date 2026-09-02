using BansheeGz.BGDatabase;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MixManager : MonoBehaviour
{
    public TextMeshProUGUI skillText1;
    public TextMeshProUGUI skillText2;
    public AnimaDataSO mainAnima;
    public AnimaDataSO subAnima;
    public Image mainImage;
    public Image subImage;
    public Toggle skill1;
    public Toggle skill2;
    public int checkSkill = -1;
    bool tutoCleared = false;
    [SerializeField]
    GameObject resultCanvas;
    [SerializeField]
    TextMeshProUGUI resultText;
    [SerializeField]
    Image resultImage;
    [SerializeField]
    private AnimaSlotUI mainSlot;
    [SerializeField]
    private AnimaSlotUI subSlot;
    [SerializeField]
    TextAsset mixDataSet;
    [SerializeField]
    private AudioClip bgmClip;
    [SerializeField]
    private AudioClip sucessClip;
    [SerializeField]
    private AudioClip failClip;
    List<MixData> mixDatas;
    List<MixData> matchedMixData;
    AbilityManager abilityManager;
    BGMetaEntity recipeTable;
    BGMetaEntity animaTable;

    // Cached component references (avoid GetComponent lookups on every call).
    private MixButtonController mixButtonController;
    private CanvasGroup mainCanvasGroup;
    private CanvasGroup subCanvasGroup;

    // Tracks which AnimaDataSO the mix slots currently display, so Update()
    // only re-runs the Resources.Load / text assignment when the selection
    // actually changes instead of every frame.
    private AnimaDataSO cachedMainAnima;
    private AnimaDataSO cachedSubAnima;

    private void Start()
    {
        mixDatas = JsonConvert.DeserializeObject<List<MixData>>(mixDataSet.text);
        matchedMixData = new List<MixData>();
        abilityManager = GameObject.Find("Game Manager").GetComponent<AbilityManager>();
        mixButtonController = GetComponent<MixButtonController>();
        tutoCleared =DontDesManager.Instance.tutoCleared;
        AudioManager.Instance.PlayBGM(bgmClip);
    }
    public void Init()
    {
        mainSlot.SetData(null, InventorySlotType.Main);
        subSlot.SetData(null, InventorySlotType.Sub);
    }
    public void Update()
    {
        if (mainAnima == null && mainImage != null)
        {
            skillText1.text = "";
            skillText2.text = "";
            checkSkill = -1;
            skill1.isOn = false;
            skill2.isOn = false;
            ClearMainImage();
        }
        if (subAnima == null && subImage != null )
        {
            ClearSubImage();
        }
        if (mainAnima != null)
        {
            GetMainCanvasGroup().alpha = 1;
            if (mainAnima != cachedMainAnima)
            {
                mainImage.sprite = Resources.Load<Sprite>($"Anima_Sprites/{mainAnima.Objectfile}");
                skillText1.text = mainAnima.skillName[0];
                if (mainAnima.skillName.Count > 1)
                {
                    skillText2.text = mainAnima.skillName[1];
                }
                cachedMainAnima = mainAnima;
            }
        }
        if(subAnima != null)
        {
            GetSubCanvasGroup().alpha = 1;
            if (subAnima != cachedSubAnima)
            {
                subImage.sprite = Resources.Load<Sprite>($"Anima_Sprites/{subAnima.Objectfile}");
                cachedSubAnima = subAnima;
            }
        }
    }
    public void Mix() 
    {
        if (checkSkill < 0)
        {
            mixButtonController.SkillError();
        }
        else if (mainAnima == null || subAnima == null)
        {
            mixButtonController.MixError();
        }
        else if (!tutoCleared)
        {
            resultCanvas.SetActive(true);
            AudioManager.Instance.PlaySFX(sucessClip);
            resultText.text = "교감 성공!!";
            resultImage.sprite = Resources.Load<Sprite>($"Anima_Sprites/고미니");
            int level = mainAnima.level;
            AnimaDataSO resultAnima = ScriptableObject.CreateInstance<AnimaDataSO>();
            resultAnima.TutorInitialize("고미니", level);
            AnimaInventoryManager.Instance.AddAnima(resultAnima);
            mainAnima = null;
            subAnima = null;
            ClearMainImage();
            ClearSubImage();
        }
        else
        {
            resultCanvas.SetActive(true);
            matchedMixData = mixDatas.Where(x => x.Main == mainAnima.Name && x.Sub == subAnima.Name).ToList();
            float odds = Random.Range(0f, 1f);
            if (matchedMixData.Count != 0 && odds < (matchedMixData[0].Odds * (1+abilityManager.MixSymbol)))
            {
                AudioManager.Instance.PlaySFX(sucessClip);
                var database = BGRepo.I;
                recipeTable = database.GetMeta("Recipe");
                recipeTable.ForEachEntity(entity =>
                {
                    if (entity.Get<string>("Main") == mainAnima.Name && entity.Get<string>("Sub") == subAnima.Name && entity.Get<int>("Sucess") == 0)
                    {
                        entity.Set<int>("Sucess", 1);
                        DBUpdater.Save();
                    }
                });
                
                resultText.text = "교감 성공!!";
                resultImage.sprite = Resources.Load<Sprite>($"Anima_Sprites/{matchedMixData[0].Result}");
                int level = mainAnima.level;
                AnimaDataSO resultAnima = ScriptableObject.CreateInstance<AnimaDataSO>();
                resultAnima.Initialize(matchedMixData[0].Result, level);
                switch (checkSkill)
                {
                    case 0:
                        resultAnima.skillName.Add(skillText1.text);
                        resultAnima.skillSprite.Add(Resources.Load<Sprite>("AnimaSkillImage/"+skillText1.text));
                        break;
                    case 1:
                        resultAnima.skillName.Add(skillText2.text);
                        resultAnima.skillSprite.Add(Resources.Load<Sprite>("AnimaSkillImage/" + skillText2.text));
                        break;
                }
                animaTable = database.GetMeta("Anima");
                animaTable.ForEachEntity(entity =>
                {
                    if (entity.Get<string>("name") == resultAnima.Name && entity.Get<int>("Meeted") != 2)
                    {
                        entity.Set<int>("Meeted", 2);
                        DBUpdater.Save();
                    }
                });
                AnimaInventoryManager.Instance.AddAnima(resultAnima);
                mainAnima = null;
                subAnima = null;
                ClearMainImage();
                ClearSubImage();
            }
            else
            {
                AudioManager.Instance.PlaySFX(failClip);
                resultText.text = "교감 실패..";
                resultImage.sprite = mainImage.sprite;
                AnimaInventoryManager.Instance.AddAnima(mainAnima);
                mainAnima = null;
                subAnima = null;
                ClearMainImage();
                ClearSubImage();
            }
            AnimaInventoryManager.Instance.InvenChanged();
        }
    }

    public void Revert()
    {
        if (mainAnima == null && subAnima == null) return;

        if (mainAnima != null)
        {
            AnimaInventoryManager.Instance.playerInfo.haveAnima.Add(mainAnima);
            mainAnima = null;
            mainSlot.AnimaData = null;
        }
        if (subAnima != null)
        {
            AnimaInventoryManager.Instance.playerInfo.haveAnima.Add(subAnima);
            subAnima = null;
            subSlot.AnimaData = null;
        }
        AnimaInventoryManager.Instance.InvenChanged();
    }

    public void CheckSkill1()
    {
        checkSkill = 0;
    }
    public void CheckSkill2()
    {
        if(skillText2.text == "")
        {
            checkSkill = -1;
        }
        else
        {
            checkSkill = 1;
        }

    }

    private CanvasGroup GetMainCanvasGroup()
    {
        if (mainCanvasGroup == null)
            mainCanvasGroup = mainImage.gameObject.GetComponent<CanvasGroup>();
        return mainCanvasGroup;
    }

    private CanvasGroup GetSubCanvasGroup()
    {
        if (subCanvasGroup == null)
            subCanvasGroup = subImage.gameObject.GetComponent<CanvasGroup>();
        return subCanvasGroup;
    }

    private void ClearMainImage()
    {
        mainImage.sprite = null;
        GetMainCanvasGroup().alpha = 0;
        cachedMainAnima = null;
    }

    private void ClearSubImage()
    {
        subImage.sprite = null;
        GetSubCanvasGroup().alpha = 0;
        cachedSubAnima = null;
    }
}
