using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class DoorBehaviour : MonoBehaviour
{
    public bool isOpen = false;
    public bool doorOpening = false;
    public bool doorClosing = false;
    [SerializeField]
    float doorOpenSpeed = 20f; // Speed at which the door opens/closes
    [SerializeField]
    float doorOpenAngle = 90f; // Angle to open the door
    float doorCloseAngle; // Angle to close the door (default is 0 degrees)
    float doorIterAngle; // Incremental angle for each update
    public int count = 0; // Counter to track the number of interactions
    [SerializeField]
    DoorBehaviour doorPair; // Reference to the paired door (if applicable)
    private void Start()
    {
        doorIterAngle = doorOpenAngle / doorOpenSpeed; // Calculate the incremental angle based on speed and time

        Vector3 doorRotation = transform.eulerAngles;
        doorCloseAngle = doorRotation.y; // Initialize the close angle to the current rotation
    }
    public void Interact()
    {
        Vector3 doorRotation = transform.eulerAngles;

        if (!isOpen)
        {
            count = 0;
            doorPair.count = 0; // Reset the count for the paired door
            doorPair.doorOpening = true; // Trigger the paired door to open
            doorOpening = true;
            
            
        }
        else
        {
            doorPair.count = 0; // Reset the count for the paired door
            doorPair.doorClosing = true; // Trigger the paired door to open
            count = 0;
            doorClosing = true;
            
        }

        
    }
    void Update()
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
    }
}
