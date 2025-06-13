using NUnit.Framework;
using UnityEngine;

public class ShipComponents : MonoBehaviour
{
    MeshRenderer meshRenderer;
    [SerializeField]
    Color hoverColor;
    Color uncollectedColor;
    [SerializeField]
    Color collectedColor;
    [SerializeField]
    Material finalMaterial;
    public bool hasCollected = false;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        uncollectedColor = meshRenderer.material.color;
    }

    public void Highlight()
    {
        if (hasCollected)
        {
            meshRenderer.material.color = collectedColor; // Keep the original color if already collected
        } // Prevent highlighting if already collected
        else
        {
            meshRenderer.material.color = hoverColor;
        }
            
            
    }
    public void Unhighlight()
    {
        meshRenderer.material.color = uncollectedColor;
    }
    public void Place()
    {
        gameObject.layer =0; // Set the layer to Default
        meshRenderer.material = finalMaterial;
        meshRenderer.material.color = finalMaterial.color; // Ensure the color is set to the final material's color
    }
}
