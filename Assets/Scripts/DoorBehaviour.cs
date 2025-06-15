using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class DoorBehaviour : MonoBehaviour
{
    public  bool isLocked = true;
    public bool isOpen = false;
    public bool doorOpening = false;
    public bool doorClosing = false;
    [SerializeField]
    float doorOpenSpeed = 20f; // Speed at which the door opens/closes
    [SerializeField]
    float doorOpenAngle = 90f; // Angle to open the door
    float doorCloseAngle; // Angle to close the door (default is 0 degrees)
    float doorIterAngle; // Incremental angle for each update
    [SerializeField]
    float doorSlideDistance = 0f; // Distance to slide the door (if applicable)
    public int count = 0; // Counter to track the number of interactions
    [SerializeField]
    DoorBehaviour doorPair; // Reference to the paired door (if applicable)
    [SerializeField]
    float closeDistance = 20f; // Distance at which the door closes automatically
    [SerializeField]
    bool slidingDoor = false; // Flag to indicate if the door is a sliding door
    [SerializeField]
    GameObject doorLock;
    [SerializeField]
    AudioClip unlockSound; // Sound to play when the door is unlocked
    AudioSource doorAudioSource; // Audio source for playing door sounds
    private void Start()
    {
        doorIterAngle = doorOpenAngle / doorOpenSpeed; // Calculate the incremental angle based on speed and time
        doorSlideDistance = doorSlideDistance / doorOpenSpeed; // Calculate the incremental distance for sliding doors
        if (slidingDoor)
        {
            isLocked = false; // Sliding doors are not locked by default
        }
        else
        {
            doorLock.SetActive(true); // Enable the door lock visual if it's not a sliding door
        }
        Vector3 doorRotation = transform.eulerAngles;
        doorCloseAngle = doorRotation.y; // Initialize the close angle to the current rotation
        doorAudioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to the door
    }
    public void Interact()
    {
        Vector3 doorRotation = transform.eulerAngles;
        if (isLocked)
        {
            Debug.Log("Door is locked. Solve the puzzle to unlock it.");
            return; // Exit if the door is locked
        }
        else
        {
            if (doorPair != null)
            {
                doorPair.isLocked = false; // Ensure the paired door is also unlocked
                
            }
            
        }
        if (!isOpen)
        {
            Debug.Log("Opening door");
            count = 0;
            doorOpening = true;
            if (doorPair != null)
            {
                doorPair.count = 0; // Reset the count for the paired door
                doorPair.doorOpening = true; // Trigger the paired door to open
                
            }
            doorAudioSource.Play(); // Play the door opening sound
            
            


        }
        else
        {
            if (doorPair != null)
            {
                doorPair.count = 0; // Reset the count for the paired door
                doorPair.doorClosing = true; // Trigger the paired door to close
                doorAudioSource.Play(); // Play the door closing sound
            }
            
            count = 0;
            doorClosing = true;

        }


    }
    void Update()
    {
        if (slidingDoor)
        {
            if (doorOpening)
            {
                Vector3 doorPosition = transform.position;
                if (count == doorOpenSpeed)
                {
                    doorOpening = false;
                    isOpen = true; // Set the door state to open
                    return; // Stop updating if the door is fully open

                }
                else
                {
                    doorPosition.y += doorSlideDistance; // Increment the door's position
                    count++; // Increment the count to track the number of updates
                }
                transform.position = doorPosition;
            }
            if (doorClosing)
            {
                Vector3 doorPosition = transform.position;
                if (count == doorOpenSpeed)
                {
                    doorClosing = false;
                    isOpen = false; // Set the door state to closed
                    return; // Stop updating if the door is fully open

                }
                else
                {
                    doorPosition.y -= doorSlideDistance; // Increment the door's position
                    count++; // Increment the count to track the number of updates
                }
                transform.position = doorPosition;
            }
            PlayerBehaviour player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
            if (Vector3.Distance(player.transform.position, transform.position) > closeDistance && isOpen && !doorClosing && !doorOpening)
            {
                count = 0;
                doorClosing = true;
            }
        }
        else
        {

            if (doorOpening)
            {
                Vector3 doorRotation = transform.eulerAngles;
                if (count == doorOpenSpeed)
                {
                    doorOpening = false;
                    isOpen = true; // Set the door state to open
                    return; // Stop updating if the door is fully open

                }
                else
                {
                    doorRotation.y += doorIterAngle; // Increment the door's rotation
                    count++; // Increment the count to track the number of updates
                }
                transform.eulerAngles = doorRotation;
            }
            if (doorClosing)
            {
                Vector3 doorRotation = transform.eulerAngles;
                if (count == doorOpenSpeed)
                {
                    doorClosing = false;
                    isOpen = false; // Set the door state to closed
                    return; // Stop updating if the door is fully open

                }
                else
                {
                    doorRotation.y -= doorIterAngle; // Increment the door's rotation
                    count++; // Increment the count to track the number of updates
                }
                transform.eulerAngles = doorRotation;
            }
            PlayerBehaviour player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
            if (Vector3.Distance(player.transform.position, transform.position) > closeDistance && isOpen && !doorClosing && !doorOpening)
            {
                // Automatically close the door if the player is within a certain distance
                doorPair.count = 0; // Reset the count for the paired door
                doorPair.doorClosing = true; // Trigger the paired door to close
                count = 0;
                doorClosing = true;
            }
        }
        
    }
    public void checkPuzzle()
    {
        var podiums = GameObject.FindGameObjectsWithTag("Puzzle");
        for (int i = 0; i < podiums.Length; i++)
        {
            var podium = podiums[i].GetComponent<PodiumBehavior>();
            if (!podium.ColorIsCorrect)
            {
                return;
            }

        }
        isLocked = false; // Unlock the door if all podiums are correct
        if (!slidingDoor)
        {
            AudioSource.PlayClipAtPoint(unlockSound, doorLock.transform.position); // Play the unlock sound
            doorLock.SetActive(false); // Disable the door lock visual for regular doors
        }
        
        Debug.Log("Puzzle solved! The door is now unlocked.");
    }
}
