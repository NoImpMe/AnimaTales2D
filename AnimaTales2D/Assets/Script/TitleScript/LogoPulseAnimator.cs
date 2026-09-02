using UnityEngine;
using System.Collections;

public class LogoAnimator : MonoBehaviour
{
    [SerializeField] private float pulseDuration = 2f;
    [SerializeField] private float pulseScale = 0.1f;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
        StartCoroutine(PulseAnimation());
    }

    private IEnumerator PulseAnimation()
    {
        while (true)
        {
            yield return AnimateScale(1f, 1f + pulseScale);
            yield return AnimateScale(1f + pulseScale, 1f);
        }
    }

    // Eases from fromScale to toScale over half a pulse duration using the same
    // sine ease-out curve the original two hand-written loops used.
    private IEnumerator AnimateScale(float fromScale, float toScale)
    {
        float half = pulseDuration / 2f;
        float elapsed = 0f;
        while (elapsed < half)
        {
            float ease = Mathf.Sin((elapsed / half) * Mathf.PI / 2f);
            float scale = Mathf.Lerp(fromScale, toScale, ease);
            transform.localScale = originalScale * scale;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
