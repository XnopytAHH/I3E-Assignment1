using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
/*
* Author: Lim En Xu Jayson
* Date: 9/6/2025
* Description: Handles the behavior of doors in the game.
*/

public class DoorBehaviour : MonoBehaviour
{
    /// <summary>
    ///  Indicates whether the door is locked or not.
    /// </summary>
    public bool isLocked = true;
    /// <summary>
    /// Indicates whether the door is currently open or closed.
    /// </summary>
    public bool isOpen = false;
    /// <summary>
    /// Flags to indicate if the door is currently opening.
    /// </summary>
    public bool doorOpening = false;
    /// <summary>
    /// Flags to indicate if the door is currently closing.
    /// </summary>
    public bool doorClosing = false;
    [SerializeField]
    /// <summary>
    /// Speed at which the door opens or closes.
    /// </summary>
    float doorOpenSpeed = 20f; 
    /// <summary>
    /// Angle to which the door opens when interacted with.
    /// </summary>
    [SerializeField]
    float doorOpenAngle = 90f; 
    /// <summary>
    /// Angle at which the door closes.
    /// </summary>
    float doorCloseAngle; 
    /// <summary>
    /// Incremental angle for the door's rotation based on speed and time.
    /// </summary>
    float doorIterAngle; 
    /// <summary>
    /// Distance the door slides when opened or closed (for sliding doors).
    /// </summary>
    [SerializeField]
    float doorSlideDistance = 0f; 
    /// <summary>
    /// Count to track the number of updates for opening or closing the door.
    /// </summary>
    public int count = 0; 
    /// <summary>
    /// Reference to the paired door that should open or close simultaneously.
    ///     </summary>
    [SerializeField]
    DoorBehaviour doorPair; 
    /// <summary>
    /// Distance at which the door automatically closes if the player is too far away.
    /// </summary>
    [SerializeField]
    float closeDistance = 20f; 
    /// <summary>
    /// Flag to indicate if the door is a sliding door.
    /// </summary>
    [SerializeField]
    bool slidingDoor = false; 
    /// <summary>
    /// Reference to the door lock GameObject, which is used for visual representation of the door lock.
    /// </summary>
    [SerializeField]
    GameObject doorLock;
    /// <summary>
    /// Audio clip to play when the door is unlocked.
    /// </summary>
    [SerializeField]
    AudioClip unlockSound; 
    /// <summary>
    /// Audio source for playing door sounds.
    /// </summary>
    AudioSource doorAudioSource; 
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
    /// <summary>
    /// Method to interact with the door, opening or closing it based on its current state.
    /// If the door is locked, it will not open until the puzzle is solved.
    /// </summary>
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
    /// <summary>
    /// Checks the puzzle state by verifying if all podiums have the correct color.
    /// If all podiums are correct, the door is unlocked.
    /// </summary>
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
