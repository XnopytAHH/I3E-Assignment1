using System.Collections;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    Transform spawnPoint; // Transform where the projectile spawns
    [SerializeField]
    int fireStrength = 10; // Strength of the projectile fire force
    [SerializeField]
    float shootInterval = 2f; // Time interval between shots
    bool canShoot = false; // Flag to control shooting
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ShootTimer(shootInterval));
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        // Check if the player is within detection range
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            
            if (Physics.Raycast(transform.position, directionToPlayer, out hitInfo))
            {
                Debug.DrawRay(transform.position, directionToPlayer * 10, Color.red); // Draw a ray for debugging
                Debug.Log("Hit: " + hitInfo.collider.name); // Log the name of the hit object
                if (hitInfo.collider.CompareTag("Player"))
                {
                    // If the player is detected, start shooting
                    canShoot = true;
                    // Rotate towards the player
                    directionToPlayer.y = 0; // Keep the rotation on the horizontal plane
                    transform.rotation = Quaternion.LookRotation(-directionToPlayer);
                }
                else
                {
                    // If the player is not detected, stop shooting
                    canShoot = false;
                }
            }
        }
    }
    IEnumerator ShootTimer(float shootInterval)
        {
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
}