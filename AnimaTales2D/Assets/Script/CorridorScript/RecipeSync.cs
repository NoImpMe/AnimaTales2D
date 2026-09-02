using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class RecipeSync : MonoBehaviour
{
    public Image obj;
    public TextMeshProUGUI desc;

    private string lastTextureName;

    void Update()
    {
        string textureName = obj.sprite.texture.name;
        if (textureName == lastTextureName)
        {
            return;
        }
        lastTextureName = textureName;
        desc.text = textureName == "Unknown" ? "" : textureName;
    }
}
