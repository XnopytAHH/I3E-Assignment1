using System.Collections;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
/*
* Author: Lim En Xu Jayson
* Date: 9/6/2025
* Description: Handles the behavior of enemies in the game, including health management, shooting projectiles at the player, and detecting the player within a certain range.
*/
public class EnemyBehaviour : MonoBehaviour
{
    /// <summary>
    /// Health of the enemy. If it reaches 0, the enemy is destroyed.
    /// </summary>
    float health = 100f; 
    /// <summary>
    /// Projectile that the enemy will shoot at the player.
    /// </summary>
    [SerializeField]
    GameObject projectile;
    /// <summary>
    /// Transform where the projectile will spawn when shot.
    /// </summary>
    [SerializeField]
    Transform spawnPoint; 
    /// <summary>
    /// Strength of the projectile's fire force.
    /// </summary>
    [SerializeField]
    int fireStrength = 10;
    /// <summary>
    /// Time interval between shots.
    /// summary>
    [SerializeField]
    float shootInterval = 2f; 
    /// <summary>
    /// Range within which the enemy can detect the player.
    /// </summary>
    [SerializeField]
    float detectionRange = 10f; 
    /// <summary>
    /// Flag to control whether the enemy can shoot or not.
    /// </summary>
    bool canShoot = false; 
    /// <summary>
    /// Animator component for controlling enemy animations.
    /// </summary>
    Animator animator; 
    /// <summary>
    /// Audio clip for the enemy's death sound.
    /// </summary>
    [SerializeField]
    AudioClip enemyDeathAudioClip; 
    /// <summary>
    /// AudioSource component for playing sounds.
    /// / </summary>
    AudioSource audioSource; 
    
    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
        StartCoroutine(ShootTimer(shootInterval));
    }

    
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(enemyDeathAudioClip, transform.position, 2f); // Play death sound
            return; // If health is 0 or less, destroy the enemy and exit the update
        }
        else
        {
            GameObject player = GameObject.FindWithTag("Player");
            // Check if the player is within detection range
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= detectionRange)
            {
                RaycastHit hitInfo;
                // Check if the player is within detection range

                if (player != null)
                {
                    Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

                    if (Physics.Raycast(transform.position, directionToPlayer, out hitInfo))
                    {
                        Debug.DrawRay(transform.position, directionToPlayer * 10, Color.red); // Draw a ray for debugging

                        if (hitInfo.collider.CompareTag("PlayerCollider"))
                        {
                            // If the player is detected, start shooting
                            canShoot = true;
                            // Rotate towards the player
                            directionToPlayer.y = 0; // Keep the rotation on the horizontal plane
                            transform.rotation = Quaternion.LookRotation(directionToPlayer);
                        }
                        else
                        {
                            // If the player is not detected, stop shooting
                            canShoot = false;
                        }
                    }
                }
            }
            else
            {
                // If the player is out of range, stop shooting
                canShoot = false;
            }
        }
    }
    /// <summary>
    /// Coroutine that handles the shooting timer.
    /// </summary>

    IEnumerator ShootTimer(float shootInterval)
    {
        if (canShoot)
        {
            animator.SetTrigger("Shooting"); // Trigger the shooting animation
        }
        yield return new WaitForSeconds(shootInterval); // Wait for 2 seconds before shooting
        if (canShoot)
        {
            audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
            
            audioSource.Play(); // Play the shooting sound
            
            GameObject player = GameObject.FindWithTag("Player");
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            GameObject newProjectile = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation); // Instantiate the projectile at the spawn point
            Vector3 fireForce = directionToPlayer * fireStrength;
            newProjectile.GetComponent<Rigidbody>().AddForce(fireForce);
        }
        StartCoroutine(ShootTimer(shootInterval)); // Repeat the shooting every 2 seconds
    }
    /// <summary>
    /// Method to modify the enemy's health.
    /// </summary>
    public void ModifyHealth(int amount)
    {
        Debug.Log($"Enemy health modified by {amount}. Current health: {health + amount}");
        health += amount;
    }
    
}
