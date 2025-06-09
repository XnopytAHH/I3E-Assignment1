using System.Collections;
using System.Threading;
using UnityEngine;

public class HealthboxBehavior : MonoBehaviour
{
    [SerializeField]
    int healthAmount = 0;
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    GameObject health;
    bool canInteract = true;
    [SerializeField]
    float distance = 2.0f; // Distance that health pickups get fired from the healthbox
                           // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void Interact()
    {
        if (canInteract)
        {
            canInteract = false; // Prevent further interactions until the coroutine completes
            float spawnCount = 0; // Counter to track the number of health pickups spawned
            
            StartCoroutine(SpawnHealth(spawnCount)); // Start the coroutine to spawn health pickups
        }


    }
    IEnumerator SpawnHealth(float spawnCount)
    {
        if (spawnCount < healthAmount)
        {

            spawnPoint.rotation = Quaternion.Euler(0, Random.Range(0, healthAmount), 30); // Randomize the rotation of the health drop
            GameObject healthDrop = Instantiate(health, spawnPoint.position, health.transform.rotation);
            Vector3 fireForce = spawnPoint.forward * distance;
            healthDrop.GetComponent<Rigidbody>().AddForce(fireForce);
            spawnCount++; // Increment the spawn count
            yield return new WaitForSeconds(0.3f); // Wait for 0.5 seconds before spawning health
            StartCoroutine(SpawnHealth(spawnCount)); // Continue spawning health pickups until the limit is reached
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
}
