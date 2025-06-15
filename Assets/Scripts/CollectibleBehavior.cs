using UnityEngine;
/*
* Author: Lim En Xu Jayson
* Date: 9/6/2025
* Description: Handles the behavior of collectible items in the game.
*/

public class CollectibleBehaviour : MonoBehaviour
{
    /// <summary>
    /// Identifies the type of collectible.
    /// </summary>
    [SerializeField]
    public string collectibleType = "";
    /// <summary>
    /// The MeshRenderer component for the collectible object.
    /// Used to change the color when highlighted or collected.
    /// </summary>
    MeshRenderer meshRenderer;
    /// <summary>
    /// Color to change to when the collectible is highlighted.
    /// </summary>
    [SerializeField]
    Color color1;
    /// <summary>
    /// Original color of the collectible before highlighting.
    /// </summary>
    Color originalColor;
    /// <summary>
    /// Audio clip to play when the collectible is collected.
    /// </summary>
    [SerializeField]
    AudioClip collectibleAudioClip; // Audio clip for the collectible sound
    
    /// <summary>
    /// Start is called at the beginning of the game.
    /// Initializes the MeshRenderer and original color of the collectible.
    /// </summary>
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;

    }
    /// <summary>
    /// Highlights the collectible by changing its color.
    /// </summary>
    
    public void Highlight()
    {
        meshRenderer.material.color = color1;
    }
    /// <summary>
    /// Unhighlights the collectible by resetting its color to the original.
    /// </summary>
    public void Unhighlight()
    {
        meshRenderer.material.color = originalColor;
    }
    /// <summary>
    /// Collects the collectible, modifies the player's score, plays a sound, and destroys the collectible object.
    ///</summary>
    public void Collect(PlayerBehaviour player)
    {

        player.collectedSomething(this); // Call the player's method to modify the score
        AudioSource.PlayClipAtPoint(collectibleAudioClip, transform.position, 1f); // Play the collectible sound

        Destroy(gameObject); // Destroy the coin object
    }
}
