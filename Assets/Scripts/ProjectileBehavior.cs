using System.Collections;
using UnityEngine;
/*
* Author: Lim En Xu Jayson 
* Date: 9/6/2025
* Description: Handles the behavior of projectiles in the game.
*/


public class ProjectileBehavior : MonoBehaviour
{
    /// <summary>
    /// projDeathTime is the time after spawning which the projectile will be destroyed.
    /// </summary>
    [SerializeField]
    float projDeathTime = 5f; 
    /// <summary>
    /// projectileDamage is the amount of damage the projectile will deal to the player or enemy it collides with.
    /// </summary>
    [SerializeField]
    public int projectileDamage = 10; 
    /// <summary>
    /// projectileAudioClip is the audio clip that will be played when the projectile collides with an object.
    /// </summary>
    [SerializeField]
    AudioClip projectileAudioClip; 
    /// <summary>
    /// canDamage is a flag to control whether the projectile can damage the player or not.
    /// It prevents the projectile from dealing damage multiple times.
    /// </summary>
    bool canDamage = true;
   
    void Start()
    {
        StartCoroutine(DestroyAfterTime(projDeathTime)); // Destroy the projectile after 5 seconds
    }
    /// <summary>
    /// Coroutine to destroy the projectile after a specified time.
    ///</summary>
    IEnumerator DestroyAfterTime(float time)
    {
        StopCoroutine(DestroyAfterTime(time)); // Stop any previous coroutine to avoid multiple calls
        yield return new WaitForSeconds(time);
        Destroy(gameObject); // Destroy the projectile object
    }
    /// <summary>
    /// Method to handle collision with the player.
    /// This method is called when the projectile collides with the player.
    /// </summary>
    public void collidedWithPlayer(PlayerBehaviour player)
    {
        if (canDamage)
        {
            canDamage = false; // Prevent further damage from this projectile
            player.ModifyHealth(-projectileDamage);
        }
        else
        {
            return; // If already damaged, do nothing
        }
        // Apply damage to the player
        
        Destroy(gameObject); // Destroy the projectile after hitting the player
    }
    /// <summary>
    /// Method to handle collision with other objects.
    /// This method is called when the projectile collides with any object in the scene.
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        AudioSource.PlayClipAtPoint(projectileAudioClip, transform.position, 0.3f); // Play the projectile sound on collision
        if (collision.gameObject.CompareTag("Enemy") && gameObject.name != "EnemyProjectile")
        {
            collision.gameObject.GetComponent<EnemyBehaviour>().ModifyHealth(-projectileDamage);
        }
        
        Destroy(gameObject);
    }
}
