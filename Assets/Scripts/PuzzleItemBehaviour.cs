using UnityEngine;

public class PuzzleItemBehaviour : MonoBehaviour
{
    [SerializeField]
    public string itemColor = ""; // Color of the puzzle item
    MeshRenderer meshRenderer;
    [SerializeField]
    Color color1;
    Color originalColor;
    

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Highlight()
    {
        meshRenderer.material.color = color1;
    }
    public void Unhighlight() 
    { 
        meshRenderer.material.color = originalColor;
    }
}
