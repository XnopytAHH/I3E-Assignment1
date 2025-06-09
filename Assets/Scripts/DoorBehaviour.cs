using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    bool isOpen = false;
    public void Interact()
    {
        Vector3 doorRotation = transform.eulerAngles;
        Debug.Log(doorRotation.y);
        if (!isOpen)
        {
            doorRotation.y += 90f;
            isOpen = true;
        }
        else
        {
            doorRotation.y -= 90f; // Open the door
            isOpen = false;
        }

        transform.eulerAngles = doorRotation;
    }
}
