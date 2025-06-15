using UnityEngine;
/*
* Author: Lim En Xu Jayson
* Date: 10/6/2025
* Description: Handles the behavior of health pickups in the game.
*/

public class HealthPickupsBehaviour : MonoBehaviour
{
    /// <summary>
    /// Health of the health pickup. When picked up, it heals the player by this amount.
    /// </summary>
    [SerializeField]
    int healing = 1;
    /// <summary>
    /// Audio clip for the health pickup sound
    /// </summary>
    [SerializeField]
    AudioClip healthPickupAudioClip; // Audio clip for the health pickup sound
    

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
