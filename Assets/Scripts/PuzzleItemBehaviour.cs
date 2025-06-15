using UnityEngine;
/*
* Author: Lim En Xu Jayson 
* Date: 13/6/2025
* Description: 
*/

public class PuzzleItemBehaviour : MonoBehaviour
{
    /// <summary>
    /// Identifies the type of puzzle item.
    /// Used to check whether the item is the correct color for the puzzle.
    /// </summary>
    [SerializeField]
    public string itemColor = ""; 
    /// <summary>
    /// The MeshRenderer component for the puzzle item.
    /// Used to change the color when highlighted or unhighlighted.
    /// </summary>
    MeshRenderer meshRenderer;
    /// <summary>
    /// Color to change to when the puzzle item is highlighted.
    /// </summary>
    [SerializeField]
    Color color1;
    /// <summary>
    /// Original color of the puzzle item before highlighting.
    /// </summary>
    Color originalColor;
    

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;

    }

    /// <summary>
    /// Highlights the puzzle item by changing its color.
    /// </summary>
    public void Highlight()
    {
        meshRenderer.material.color = color1;
    }
    /// <summary>
    /// Unhighlights the puzzle item by resetting its color to the original.
    /// </summary>
    public void Unhighlight()
    {
        meshRenderer.material.color = originalColor;
    }
}
