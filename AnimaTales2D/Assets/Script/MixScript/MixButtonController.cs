using TMPro;
using UnityEngine;

public class MixButtonController : MonoBehaviour
{
    [SerializeField] private GameObject mixPanel;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private MixManager mixManager;
    [SerializeField] private GameObject resultCanvas;
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private TextMeshProUGUI errorText;

    public void ExitPanel()
    {
        mixManager.Revert();
        mixPanel.SetActive(false);
        exitButton.SetActive(true);
    }
    public void EnterPanel()
    {
        mixPanel.SetActive(true);
        mixManager.Init();
        exitButton.SetActive(false);
    }
    public void ExitMix()
    {
        resultCanvas.SetActive(false);
    }
    public void ExitError()
    {
        errorPanel.SetActive(false);
    }
    public void SkillError()
    {
        errorText.text = "계승할 스킬을 선택해주세요.";
        errorPanel.SetActive(true);
    }
    public void MixError()
    {
        errorText.text = "교감을 위해서는 두 아니마를 각 칸에 넣어주세요.";
        errorPanel.SetActive(true);
    }
}
