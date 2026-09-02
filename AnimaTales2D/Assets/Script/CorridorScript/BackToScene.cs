using UnityEngine;

public class BackToScene : MonoBehaviour
{
    [SerializeField]
    private FadeEffect fadePanel;
    [SerializeField]
    private AudioClip btnClip;

    private SceneManagerCorridor sceneManagerCorridor;

    private SceneManagerCorridor GameSceneManager
    {
        get
        {
            if (sceneManagerCorridor == null)
            {
                sceneManagerCorridor = GameObject.Find("Game Manager").GetComponent<SceneManagerCorridor>();
            }
            return sceneManagerCorridor;
        }
    }

    public void backToScenes()
    {
        AudioManager.Instance.PlaySFX(btnClip);
        StartCoroutine(fadePanel.LoadSceneWithFade(GameSceneManager.sceneName));
    }
    public void BackToTiles()
    {
        AudioManager.Instance.PlaySFX(btnClip);
        StartCoroutine(fadePanel.LoadSceneWithFade(GameSceneManager.tileSceneName));
    }
    public void BackToTitle()
    {
        DontDesManager.Instance.TutorialClear();
        StartCoroutine(fadePanel.LoadSceneWithFade("TitleScene"));
        GoldManager.Instance.Init();
    }
}
