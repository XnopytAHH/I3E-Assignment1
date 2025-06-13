using System.Collections;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    float health = 100f; // Health of the enemy
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    Transform spawnPoint; // Transform where the projectile spawns
    [SerializeField]
    int fireStrength = 10; // Strength of the projectile fire force
    [SerializeField]
    float shootInterval = 2f; // Time interval between shots
    [SerializeField]
    float detectionRange = 10f; // Range within which the enemy can detect the player
    bool canShoot = false; // Flag to control shooting
    Animator animator; // Reference to the Animator component
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
        StartCoroutine(ShootTimer(shootInterval));
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
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

    IEnumerator ShootTimer(float shootInterval)
    {
        if (canShoot)
        {
            animator.SetTrigger("Shooting"); // Trigger the shooting animation
        }
        yield return new WaitForSeconds(shootInterval); // Wait for 2 seconds before shooting
        if (canShoot)
        {
            GameObject player = GameObject.FindWithTag("Player");
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            GameObject newProjectile = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation); // Instantiate the projectile at the spawn point
            Vector3 fireForce = directionToPlayer * fireStrength;
            newProjectile.GetComponent<Rigidbody>().AddForce(fireForce);
        }
        StartCoroutine(ShootTimer(shootInterval)); // Repeat the shooting every 2 seconds
    }
    public void ModifyHealth(int amount)
    {
        Debug.Log($"Enemy health modified by {amount}. Current health: {health + amount}");
        health += amount;
    }
    
}
