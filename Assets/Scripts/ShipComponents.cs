using NUnit.Framework;
using UnityEngine;
/*
* Author: Lim En Xu Jayson
* Date: 10/6/2025
* Description: Handles the behavior of ship components in the game.
*/

public class ShipComponents : MonoBehaviour
{
    /// <summary>
    /// MeshRenderer component for the ship component.
    /// Used to change the color when highlighted or placed.
    /// </summary>
    MeshRenderer meshRenderer;
    /// <summary>
    /// Color to change to when the ship component is highlighted.
    /// </summary>
    [SerializeField]
    Color hoverColor;
    /// <summary>
    /// Original color of the ship component before highlighting.
    /// </summary>
    Color uncollectedColor;
    /// <summary>
    /// Color to change to when the ship component is placed
    /// </summary>
    [SerializeField]
    Color collectedColor;
    /// <summary>
    /// Final material to apply when the ship component is placed
    /// </summary>
    [SerializeField]
    Material finalMaterial;
    /// <summary>
    /// AudioSource component for the ship component.
    /// Used to play a sound when the component is placed.
    /// </summary>
    AudioSource collectedAudio; 
    /// <summary>
    /// Indicates whether the ship component has been collected.
    /// </summary>
    public bool hasCollected = false;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        uncollectedColor = meshRenderer.material.color;
        collectedAudio = GetComponent<AudioSource>(); // Get the AudioSource component attached to the ship component
    }
    /// <summary>
    /// Highlights the ship component by changing its color to the hover color.
    /// If the component has already been collected, it keeps the collected color.
    /// </summary>
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
    /// <summary>
    /// Unhighlights the ship component by resetting its color to the uncollected color.
    /// /// </summary>
    public void Unhighlight()
    {
        meshRenderer.material.color = uncollectedColor;
    }
    /// <summary>
    /// Places the ship component, changes its color to the final material, and plays a sound.
    /// </summary>
    public void Place()
    {
        collectedAudio.Play(); // Play the collected sound
        gameObject.layer = 0; // Set the layer to Default
        meshRenderer.material = finalMaterial;
        meshRenderer.material.color = finalMaterial.color; // Ensure the color is set to the final material's color
    }
}
