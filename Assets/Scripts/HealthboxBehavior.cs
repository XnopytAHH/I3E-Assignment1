using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
/*
* Author: Lim En Xu Jayson
* Date: 9/6/2025
* Description: Handles the behavior of healthboxes in the game.
*/

public class HealthboxBehavior : MonoBehaviour
{
    /// <summary>
    /// Field to specify the amount of health pickups to spawn when the healthbox is interacted with.
    /// </summary>
    [SerializeField]
    int healthAmount = 0;
    /// <summary>
    /// Field to specify the spawn point for health pickups.
    /// </summary>
    [SerializeField]
    Transform spawnPoint;
    /// <summary>
    /// Field to specify the health pickup prefab that will be spawned.
    /// </summary>
    [SerializeField]
    GameObject health;
    /// <summary>
    /// Field to control whether the healthbox can be interacted with or not.
    /// </summary>
    bool canInteract = true;
    /// <summary>
    /// Field to specify the distance at which health pickups are fired from the healthbox.
    /// </summary>
    [SerializeField]
    float distance = 2.0f; 
    /// <summary>
    /// Field to specify the delay between spawning health pickups.
    /// / </summary>
    [SerializeField]
    float spawnDelay = 0.5f;
    /// <summary>
    /// Field to specify the AudioSource component for playing sounds.
    /// It is to play the sound of the box opening and spawning health pickups.
    /// </summary>
    AudioSource audioSource;
    /// <summary>
    /// List to store random rotations for health pickups.
    /// This is used to ensure that health pickups spawn with different orientations.
    /// </summary>
    List<int> rotations = new List<int>(); // List to store random rotations for health pickups
    
    /// <summary>
    /// Method to handle interaction with the healthbox.
    /// This method is called when the player interacts with the healthbox.
    /// </summary>
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
                rotations.Add(i * (360 / healthAmount)); // Add random rotations to the list
            }

            StartCoroutine(SpawnHealth(spawnCount)); // Start the coroutine to spawn health pickups
        }


    }
    /// <summary>
    /// Coroutine to spawn health pickups at the specified spawn point.
    /// This coroutine will spawn health pickups at random rotations and apply a force to them.
    /// </summary>
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
