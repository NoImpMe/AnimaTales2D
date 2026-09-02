using System.Collections;
using UnityEngine;

public class MilestoneMoveLfRtUI : MonoBehaviour
{
    public float moveDistance = 6f;
    private float duration = 1f;
    Vector3 originPos;
    RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originPos = rectTransform.anchoredPosition;
        StartCoroutine(MoveAnimation());
    }
    private IEnumerator MoveAnimation()
    {
        float elapsed = 0f;

        while (true)
        {
            elapsed += Time.deltaTime;

            float t = (elapsed / duration) * 2f * Mathf.PI;

            float yOffset = Mathf.Sin(t) * moveDistance;

            Vector3 pos = originPos;
            pos.x += yOffset;
            rectTransform.anchoredPosition = pos;

            if (elapsed > duration)
                elapsed -= duration;

            yield return null;
        }
    }
    public void Stop()
    {
        Destroy(gameObject);
    }
}
