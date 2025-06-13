using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.Shapes;

public class PodiumBehavior : MonoBehaviour
{
    [SerializeField]
    Transform floatingObjectPosition; // Position where the floating object will be placed
    public GameObject floatingObject; // The object that will be placed on the podium
    [SerializeField]
    public string correctColor = "";
    public bool ColorIsCorrect = false;
    [SerializeField]
    ParticleSystem particles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        particles.Stop(); // Stop the particle effect initially
    }

    // Update is called once per frame
    void Update()
    {
        if (floatingObject != null)
        {
            
            // Update the position and rotation of the floating object
            floatingObject.transform.position = floatingObjectPosition.position;
            floatingObject.GetComponent<Transform>().Rotate(0, 0.2f, 0);

        }

    }
    public void PlaceObject(GameObject obj)
    {
        if (floatingObject == null)
        {
            particles.Play(); // Play the particle effect when an object is placed on the podium
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
            var doorPair = GameObject.FindGameObjectsWithTag("Door");
            doorPair[0].GetComponent<DoorBehaviour>().checkPuzzle(); // Check the puzzle state for the paired door
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
            particles.Stop(); // Stop the particle effect when the object is removed
            floatingObject = null;
            ColorIsCorrect = false; // Reset the color check
        }
        else
        {
            Debug.LogWarning("No floating object to remove from the podium.");
        }
    }
}
