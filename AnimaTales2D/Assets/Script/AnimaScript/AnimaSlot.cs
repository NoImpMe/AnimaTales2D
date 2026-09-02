using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class AnimaSlot : MonoBehaviour
{
    [Header("UI ¿ä¼Ò")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Button slotButton;

    private AnimaEntry animaData;

    public UnityEvent<AnimaEntry> onClick = new();

    public void Setup(AnimaEntry entry, bool discovered)
    {
        animaData = entry;

        Sprite sprite = entry.GetImage();
        iconImage.sprite = sprite;
        iconImage.color = discovered ? Color.white : Color.black;

        nameText.text = discovered ? entry.name : "???";

        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(() => onClick.Invoke(animaData));
    }

}
