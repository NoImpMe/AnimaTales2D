using UnityEngine;
using UnityEngine.UI;

public class DraggedPortraitUI : MonoBehaviour
{
    public static DraggedPortraitUI Instance { get; private set; }

    [SerializeField] private Image portraitImage;

    private Canvas canvas;
    private RectTransform rectTransform;
    public AnimaSlotUI OriginSlot { get; private set; }

    private void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void BeginDrag(Sprite sprite, AnimaSlotUI originSlot)
    {
        if (portraitImage == null || gameObject == null) return;
        
        OriginSlot = originSlot;
        portraitImage.sprite = sprite;
        portraitImage.enabled = true;
        portraitImage.color = new Color(1, 1, 1, 0.7f);
        gameObject.SetActive(true);
    }

    public void UpdatePosition(Vector2 screenPosition)
    {
        if (canvas == null || rectTransform == null) return;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, screenPosition, canvas.worldCamera, out var localPos);
        rectTransform.localPosition = localPos;
    }

    public void EndDrag()
    {
        OriginSlot = null;
    
        if (portraitImage != null)
        {
            portraitImage.sprite = null;
            portraitImage.enabled = false;
        }

        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }
}