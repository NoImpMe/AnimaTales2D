using UnityEngine;

public class CorridorUIManager : MonoBehaviour
{
    [Header("UI List")]
    [SerializeField] private GameObject animaDex;
    [SerializeField] private GameObject mixDex;
    [SerializeField] private GameObject mixDetail;

    [Header("버튼 사운드")]
    [SerializeField] private AudioClip btnClip;

    public void OpenAnimaDex()
    {
        PlayButtonSfx();
        animaDex.SetActive(true);
    }
    public void CloseAnimaDex()
    {
        PlayButtonSfx();
        animaDex.SetActive(false);
    }
    public void OpenMixDex()
    {
        PlayButtonSfx();
        mixDex.SetActive(true);
    }
    public void CloseMixDex()
    {
        PlayButtonSfx();
        mixDetail.SetActive(false);
        mixDex.SetActive(false);
    }
    public void ToggleChanged()
    {
        PlayButtonSfx();
    }

    private void PlayButtonSfx()
    {
        AudioManager.Instance.PlaySFX(btnClip);
    }
}
