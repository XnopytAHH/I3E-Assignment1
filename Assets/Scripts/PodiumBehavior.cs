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
    AudioSource audioSource; // Audio source for playing sounds
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        particles.Stop(); // Stop the particle effect initially
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to the podium
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
            audioSource.Play(); // Play the placement sound
            // Instantiate the floating object at the specified position
            floatingObject = obj;
            if (floatingObject.GetComponent<PuzzleItemBehaviour>() != null)
            {
                // Check if the color of the object matches the correct color
                if (floatingObject.GetComponent<PuzzleItemBehaviour>().itemColor == correctColor)
                {
                    ColorIsCorrect = true;
                    var correctParticles = particles.main;
                    correctParticles.startColor = Color.green; // Set particle color to green for correct placement
                    particles.Play(); // Start the particle effect when the correct object is placed
                }
                else
                {
                    ColorIsCorrect = false;
                    
                    var incorrectParticles = particles.main;
                    incorrectParticles.startColor = Color.red; // Set particle color to red for incorrect placement
                    particles.Play(); // Start the particle effect when the incorrect object is placed
                }
            }
            
            var doorPair = GameObject.FindGameObjectsWithTag("Door");
            for (int i = 0; i < doorPair.Length; i++)
            {
                doorPair[i].GetComponent<DoorBehaviour>().checkPuzzle(); // Check the puzzle state for the paired door
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
