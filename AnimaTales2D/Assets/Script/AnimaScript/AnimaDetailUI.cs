using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimaDetailUI : MonoBehaviour
{
    private const string UnknownText = "???";

    [Header("UI ÂüÁ¶")]
    [SerializeField] private Image animaImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private Image skillImage;
    [SerializeField] private TMP_Text skillName;

    public void Display(AnimaEntry anima, bool discovered)
    {
        animaImage.sprite = anima.GetImage();
        animaImage.color = discovered ? Color.white : Color.black;
        nameText.text = discovered ? anima.name : UnknownText;
        descriptionText.text = discovered ? anima.description : UnknownText;
        typeText.text = discovered ? anima.emotion.ToString() : UnknownText;

        if (anima.skillName != null)
        {
            skillImage.color = discovered ? Color.white : Color.black;
            skillImage.sprite = Resources.Load<Sprite>($"AnimaSkillImage/{anima.skillName[0]}");
            skillName.text = discovered ? anima.skillName[0] : UnknownText;
        }
        else
        {
            var color = skillImage.color;
            skillName.text = "";
            color.a = 0f;
            skillImage.color = color;
        }
    }
}
