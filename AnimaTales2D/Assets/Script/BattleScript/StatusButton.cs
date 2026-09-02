using UnityEngine;

public class StatusButton : MonoBehaviour
{
    public GameObject image;

    public void onClicked()
    {
        image.SetActive(!image.activeSelf);
    }
}
