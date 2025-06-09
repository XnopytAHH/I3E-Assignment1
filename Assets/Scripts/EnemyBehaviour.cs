using System.Collections;
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ShootTimer(shootInterval));
    }

    // Update is called once per frame
    void Update()
    {

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
