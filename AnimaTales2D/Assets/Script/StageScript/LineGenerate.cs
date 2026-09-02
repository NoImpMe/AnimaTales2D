using UnityEngine;
using DG.Tweening;

public class LineGenerate : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform startPoint;
    public Transform endPoint;

    [Header("¼³Á¤")]
    public float segmentLength = 2f;
    public float moveDuration = 2f;
    public float vanishDuration = 1f;

    Vector3 direction;
    float totalDistance;
    void Start()
    {
        lineRenderer.useWorldSpace = false;

        direction = (endPoint.position - startPoint.position).normalized;
        totalDistance = Vector3.Distance(startPoint.position, endPoint.position);

        ResetLineToStart();

        StartTrace();
    }

    void StartTrace()
    {
        DOTween.To(() => 0f, t =>
        {
            float headDist = Mathf.Lerp(0, totalDistance, t);
            float tailDist = Mathf.Max(0, headDist - segmentLength);

            Vector3 worldHead = startPoint.position + direction * headDist;
            Vector3 worldTail = startPoint.position + direction * tailDist;

            Vector3 localHead = transform.InverseTransformPoint(worldHead);
            Vector3 localTail = transform.InverseTransformPoint(worldTail);

            lineRenderer.SetPosition(0, localTail);
            lineRenderer.SetPosition(1, localHead);

        }, 1f, moveDuration)
          .SetEase(Ease.Linear)
          .OnComplete(StartVanish);
    }

    void StartVanish()
    {
        Vector3 initTail = lineRenderer.GetPosition(0);
        Vector3 initHead = lineRenderer.GetPosition(1);

        DOTween.To(() => 0f, t =>
        {
            Vector3 newTail = Vector3.Lerp(initTail, initHead, t);
            lineRenderer.SetPosition(0, newTail);
        }, 1f, vanishDuration)
          .SetEase(Ease.InOutSine)
          .OnComplete(() =>
          {
              ResetLineToStart();
              StartTrace();
          });
    }

    private void ResetLineToStart()
    {
        Vector3 localStart = transform.InverseTransformPoint(startPoint.position);
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, localStart);
        lineRenderer.SetPosition(1, localStart);
    }

    public void setOpenColor()
    {
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
    }

public void setInvalidColor()
    {
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
    }
}
