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
            float elapsed = 0f;
            while (elapsed < pulseDuration / 2)
            {
                float scale = 1f + Mathf.Sin((elapsed / (pulseDuration / 2)) * Mathf.PI / 2) * pulseScale;
                transform.localScale = originalScale * scale;
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            elapsed = 0f;
            while (elapsed < pulseDuration / 2)
            {
                float scale = 1f + pulseScale - Mathf.Sin((elapsed / (pulseDuration / 2)) * Mathf.PI / 2) * pulseScale;
                transform.localScale = originalScale * scale;
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}