using UnityEngine;

public class PodiumBehavior : MonoBehaviour
{
    [SerializeField]
    Transform floatingObjectPosition; // Position where the floating object will be placed
    GameObject floatingObject; // The object that will be placed on the podium
    [SerializeField]
    string correctColor = "";
    public bool ColorIsCorrect = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (floatingObject != null)
        {
            // Update the position and rotation of the floating object
            floatingObject.transform.position = floatingObjectPosition.position;
            floatingObject.GetComponent<Transform>().Rotate(0, 1, 0);

        }

    }
    public void PlaceObject(GameObject obj)
    {
        if (floatingObject == null)
        {
            // Instantiate the floating object at the specified position
            floatingObject = obj;
            if (floatingObject.GetComponent<PuzzleItemBehaviour>() != null)
            {
                // Check if the color of the object matches the correct color
                if (floatingObject.GetComponent<PuzzleItemBehaviour>().itemColor == correctColor)
                {
                    ColorIsCorrect = true;
                }
                else
                {
                    ColorIsCorrect = false;
                }
            }
        }
        else
        {
            Debug.LogWarning("A floating object is already placed on the podium.");
        }
    }
    public void RemoveObject()
    {
        if (floatingObject != null)
        {
            // Destroy the floating object and reset the reference
            Destroy(floatingObject);
            floatingObject = null;
            ColorIsCorrect = false; // Reset the color check
        }
        else
        {
            Debug.LogWarning("No floating object to remove from the podium.");
        }
    }
}
