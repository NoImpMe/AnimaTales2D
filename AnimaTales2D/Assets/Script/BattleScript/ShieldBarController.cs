using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShieldBarController : MonoBehaviour
{
    public Image shieldBarFill;

    void Start()
    {
        shieldBarFill.color = Color.blue;
    }
    public void Updateshield(float newshieldPercentage)
    {
        StartCoroutine(SmoothshieldChange(shieldBarFill.fillAmount, newshieldPercentage, 1.0f));
    }

    public IEnumerator SmoothshieldChange(float startFillAmount, float targetFillAmount, float duration)
    {
        float elapsedTime = 0f;

        

        while (elapsedTime < duration)
        {
            shieldBarFill.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / duration);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        shieldBarFill.fillAmount = targetFillAmount;
    }
}
