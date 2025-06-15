using UnityEngine;

public class CollectibleBehaviour : MonoBehaviour
{
    // Coin value that will be added to the player's score
    [SerializeField]
    public string collectibleType = "";
    MeshRenderer meshRenderer;
    [SerializeField]
    Color color1;
    Color originalColor;
    [SerializeField]
    AudioClip collectibleAudioClip; // Audio clip for the collectible sound
    

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;

    }
    public void Highlight()
    {
        meshRenderer.material.color = color1;
    }
    public void Unhighlight() 
    { 
        meshRenderer.material.color = originalColor;
    }
    public void Collect(PlayerBehaviour player)
    {
        
        player.collectedSomething(this); // Call the player's method to modify the score
        AudioSource.PlayClipAtPoint(collectibleAudioClip, transform.position, 1f); // Play the collectible sound

        Destroy(gameObject); // Destroy the coin object
    }
}
