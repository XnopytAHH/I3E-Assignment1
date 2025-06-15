using UnityEngine;

public class HealthPickupsBehaviour : MonoBehaviour
{
    [SerializeField]
    int healing = 1; // Speed of rotation for the health pickup
    [SerializeField]
    AudioClip healthPickupAudioClip; // Audio clip for the health pickup sound
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Transform>().Rotate(0, 1, 0);
    }
    public void Pickup()
    {
        AudioSource.PlayClipAtPoint(healthPickupAudioClip, transform.position, 0.3f); // Play the health pickup sound
        Debug.Log("health");
        Destroy(gameObject); // Destroy the health pickup after it has been picked up
        // Find the player object
        PlayerBehaviour player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        if (player != null)
        {
            Debug.Log("Player found, applying health pickup.");
            player.ModifyHealth(healing);
            
        }
    }
}
