using System.Collections;
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
    [SerializeField]
    float detectionRange = 10f; // Range within which the enemy can detect the player
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ShootTimer(shootInterval));
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is within detection range
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= detectionRange)
            {
                // Rotate towards the player
                Vector3 directionToPlayer = -(player.transform.position - transform.position).normalized;
                directionToPlayer.y = 0; // Keep the rotation on the horizontal plane
                transform.rotation = Quaternion.LookRotation(directionToPlayer);

            }
        }
    }
    IEnumerator ShootTimer(float shootInterval)
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before shooting
        GameObject newProjectile = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation); // Instantiate the projectile at the spawn point
        Vector3 fireForce = spawnPoint.forward * fireStrength;
        newProjectile.GetComponent<Rigidbody>().AddForce(fireForce);
        StartCoroutine(ShootTimer(shootInterval)); // Repeat the shooting every 2 seconds
    }
}
