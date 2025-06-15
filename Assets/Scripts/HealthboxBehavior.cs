using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

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
    [SerializeField]
    float spawnDelay = 0.5f; // Delay between spawning health pickups
    AudioSource audioSource; // Audio source for playing sounds
    List<int> rotations = new List<int>(); // List to store random rotations for health pickups
    
    public void Interact()
    {
        if (canInteract)
        {
            audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to the healthbox
            audioSource.Play(); // Play the interaction sound
            canInteract = false; // Prevent further interactions until the coroutine completes
            float spawnCount = 0; // Counter to track the number of health pickups spawned
            for (int i = 0; i < healthAmount; i++)
            {
                rotations.Add(i*(360/healthAmount)); // Add random rotations to the list
            }
            
            StartCoroutine(SpawnHealth(spawnCount)); // Start the coroutine to spawn health pickups
        }


    }
    IEnumerator SpawnHealth(float spawnCount)
    {
        if (spawnCount < healthAmount)
        {
            int spawnAngle = UnityEngine.Random.Range(0, rotations.Count);
            spawnPoint.rotation = Quaternion.Euler(-65, rotations[spawnAngle], 0); // Set the rotation for the health pickup
            rotations.RemoveAt(spawnAngle); // Remove the used rotation to avoid duplicates
            GameObject healthDrop = Instantiate(health, spawnPoint.position, health.transform.rotation);
            Vector3 fireForce = spawnPoint.forward * distance;
            healthDrop.GetComponent<Rigidbody>().AddForce(fireForce);
            spawnCount++; // Increment the spawn count
            yield return new WaitForSeconds(spawnDelay); // Wait for a delay before spawning health
            StartCoroutine(SpawnHealth(spawnCount)); // Continue spawning health pickups until the limit is reached
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
}
