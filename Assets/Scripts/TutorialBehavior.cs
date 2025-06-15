using UnityEngine;

public class TutorialBehavior : MonoBehaviour
{
    [SerializeField]
    int tutorialStep = 0; // Current step in the tutorial
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehaviour>().DisplayTutorial(tutorialStep);
            Destroy(gameObject); // Destroy the tutorial object after displaying
        }
    }
}
