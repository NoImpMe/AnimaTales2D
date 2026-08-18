using UnityEngine;

public class OutlineHighlight : MonoBehaviour
{
    [SerializeField] private Color outlineColor = Color.yellow;
    [SerializeField] private float outlineWidth = 0.03f;
    
    private Renderer rend;
    private Material originalMaterial;
    private Material outlineMaterial;
    private bool isHighlighted = false;
    
    private void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            originalMaterial = rend.material;
            
            Shader outlineShader = Shader.Find("Custom/Outline");
            
            if (outlineShader != null)
            {
                outlineMaterial = new Material(outlineShader);
                outlineMaterial.SetColor("_Color", originalMaterial.color);
                outlineMaterial.SetTexture("_MainTex", originalMaterial.mainTexture);
                outlineMaterial.SetColor("_OutlineColor", outlineColor);
                outlineMaterial.SetFloat("_OutlineWidth", outlineWidth);
            }
        }
    }
    
    private void OnMouseEnter()
    {
        ActivateOutline();
    }
    
    private void OnMouseExit()
    {
        DeactivateOutline();
    }
    
    public void ActivateOutline()
    {
        if (rend != null && outlineMaterial != null && !isHighlighted)
        {
            rend.material = outlineMaterial;
            isHighlighted = true;
        }
    }
    
    public void DeactivateOutline()
    {
        if (rend != null && originalMaterial != null && isHighlighted)
        {
            rend.material = originalMaterial;
            isHighlighted = false;
        }
    }
    
    private void OnDisable()
    {
        if (rend != null && originalMaterial != null && isHighlighted)
        {
            rend.material = originalMaterial;
            isHighlighted = false;
        }
    }
}