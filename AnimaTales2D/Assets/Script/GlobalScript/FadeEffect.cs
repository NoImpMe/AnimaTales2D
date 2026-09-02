using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        GetComponent<Image>().color = Color.black;
        canvasGroup.blocksRaycasts = false;
    }

    // Shared alpha-lerp loop used by both FadeIn and FadeOut.
    private IEnumerator FadeAlpha(float targetAlpha)
    {
        float elapsed = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }

    public IEnumerator FadeIn()
    {
        canvasGroup.blocksRaycasts = true;
        yield return FadeAlpha(1f);
    }

    public IEnumerator FadeOut()
    {
        yield return FadeAlpha(0f);
        canvasGroup.blocksRaycasts = false;
    }

    public IEnumerator LoadSceneWithFade(string sceneName)
    {
        yield return StartCoroutine(FadeIn());
        SceneManager.LoadScene(sceneName);
    }
}