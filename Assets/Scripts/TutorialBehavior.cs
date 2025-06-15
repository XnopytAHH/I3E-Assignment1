using UnityEngine;
/*
* Author: Lim En Xu Jayson 
* Date: 14/6/2025
* Description: Handles the tutorial behavior when the player enters a trigger area.
*/

public class TutorialBehavior : MonoBehaviour
{
    /// <summary>
    /// Step in the tutorial to display when the player enters the trigger.
    /// </summary>
    [SerializeField]
    int tutorialStep = 0; 
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehaviour>().DisplayTutorial(tutorialStep);
            Destroy(gameObject); // Destroy the tutorial object after displaying
        }
    }
}
