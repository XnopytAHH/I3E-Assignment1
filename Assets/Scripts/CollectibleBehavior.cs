using UnityEngine;

public class CollectibleBehaviour : MonoBehaviour
{
    // Coin value that will be added to the player's score
    [SerializeField]
    string collectibleType = "";
    MeshRenderer meshRenderer;
    [SerializeField]
    Color color1;
    Color originalColor;

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
        
        player.ModifyScore(collectibleType);

        Destroy(gameObject); // Destroy the coin object
    }
}
