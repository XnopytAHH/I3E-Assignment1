using System;

using UnityEngine;

using TMPro;
using Microsoft.Unity.VisualStudio.Editor;
using System.Collections.Generic;
using Unity.VisualScripting;
using StarterAssets;
using System.Security.Cryptography;
public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    Transform playerSpawnPoint; // Transform where the player spawns
    int maxHealth = 100; // Player's maximum health
    int currentHealth = 100; // Player's current health
    int currentScore = 0; // Player's current score
    bool canInteract = false; // Flag to check if the player can interact with objects
    CollectibleBehaviour currentCollectible = null; // Stores the current coin object the player has detected
    DoorBehaviour currentDoor = null; // Stores the current door object the player has detected
    HealthboxBehavior currentBox = null; // Stores the current health box object the player has detected
    ShipComponents currentObj = null; // Stores the current health box object the player has detected
    PuzzleItemBehaviour currentItem = null; // Stores the current puzzle item object the player has detected
    [SerializeField]
    float interactDistance = 2.0f; // Distance within which the player can interact with objects
    [SerializeField]
    GameObject projectile = null; // Projectile prefab to instantiate when firing
    [SerializeField]
    Transform spawnPoint; // Transform where the projectile spawns
    [SerializeField]
    int fireStrength = 0; // Strength of the projectile fire force
    [SerializeField]
    TextMeshProUGUI healthText; // Text component to display player's health

    [SerializeField]
    Canvas PlayerUI; // Canvas to display UI elements

    [SerializeField]
    Canvas DeathUI;
    [SerializeField]
    GameObject gunImage; // Image component to display the gun icon
    bool hasGun = false; // Flag to check if the player has a gun
    List<string> objectivesCollected = new List<string>(); // Array to store collected objectives
     [SerializeField]
    GameObject damageIndicator; // GameObject to indicate damage taken by the player
    bool carryingItem = false; // Flag to check if the player is carrying an item

    void Start()
    {
        healthText.text = "Health: " + currentHealth.ToString(); // Initialize health text
        gunImage.SetActive(false); // Hide the gun image at the start
        damageIndicator.SetActive(false); // Hide the damage indicator at the start
        DeathUI.enabled = false; // Disable the death UI
        Respawn(); // Call the Respawn method to set the player's initial position and state
    }
    void Update()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactDistance))

        {
            Debug.DrawLine(spawnPoint.position, hitInfo.point, Color.red); // Draw a red line in the scene view for debugging
            if (hitInfo.collider.CompareTag("Collectible"))
            {
                // Set the canInteract flag to true
                // Get the CollectibleBehaviour component from the detected object
                canInteract = true;

                currentCollectible = hitInfo.collider.GetComponent<CollectibleBehaviour>();
                currentCollectible.Highlight(); // Highlight the coin when detected
            }
            else if (currentCollectible != null)
            {
                // If the raycast hits something else, unhighlight the current coin
                currentCollectible.Unhighlight();
                currentCollectible = null; // Reset currentCollectible to null
            }
            if (hitInfo.collider.CompareTag("Objective"))
            {
                // Set the canInteract flag to true
                // Get the CollectibleBehaviour component from the detected object
                canInteract = true;
                currentObj = hitInfo.collider.GetComponent<ShipComponents>();
                if (objectivesCollected.Contains(currentObj.name))
                {
                    currentObj.hasCollected = true; // Set hasCollected to true if the objective has already been collected
                }
                currentObj.Highlight(); // Highlight the Obj when detected
            }
            else if (currentObj != null)
            {
                // If the raycast hits something else, unhighlight the current coin
                currentObj.Unhighlight();
                currentObj = null; // Reset currentObj to null
            }
            if (hitInfo.collider.CompareTag("Door"))
            {
                // Set the canInteract flag to true
                // Get the DoorBehaviour component from the detected object
                canInteract = true;
                currentDoor = hitInfo.collider.GetComponent<DoorBehaviour>();
            }
            if (hitInfo.collider.CompareTag("Healthbox"))
            {
                // Set the canInteract flag to true
                // Get the HealthboxBehavior component from the detected object
                canInteract = true;
                currentBox = hitInfo.collider.GetComponent<HealthboxBehavior>();
            }
            if (hitInfo.collider.CompareTag("PuzzleItem"))
            {
                // Set the canInteract flag to true
                // Get the PuzzleItemBehaviour component from the detected object
                canInteract = true;
                currentItem = hitInfo.collider.GetComponent<PuzzleItemBehaviour>();
                currentItem.Highlight(); // Highlight the puzzle item when detecte d
            }
            
        }
        else
        {
            // If the raycast does not hit any object, set canInteract to false
            canInteract = false;
            currentDoor = null; // Reset currentDoor to null
            currentBox = null; // Reset currentBox to null
            if (currentObj != null)
            {
                currentObj.Unhighlight(); // Unhighlight the current object if it exists
            }
            if (currentCollectible != null)
            {
                currentCollectible.Unhighlight(); // Unhighlight the current object if it exists
            }
            currentObj = null; // Reset currentObj to null

        }
        if (currentHealth <= 0)
        {
            Death(); // Call the Death method if health is zero or below
        }
    }
    // The Interact callback for the Interact Input Action
    // This method is called when the player presses the interact button
    void Death()
    {
        PlayerUI.enabled = false; // Disable the main player UI
        DeathUI.enabled = true; // Enable the death UI
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        
        gameObject.GetComponent<FirstPersonController>().enabled = false; // Disable the character controller to prevent movement
        
    }
    public void Respawn()
    {
        // Reset the player's position to the spawn point
        
        // Reset the player's health to maximum
        currentHealth = maxHealth;
        // Update the health text to reflect the new health value
        healthText.text = "Health: " + currentHealth.ToString();

        PlayerUI.enabled = true; // Enable the main player UI
        DeathUI.enabled = false; // Disable the death UI
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Debug.Log("Respawning at: " + playerSpawnPoint.position);
        transform.position = playerSpawnPoint.position;
        Physics.SyncTransforms();
        gameObject.GetComponent<FirstPersonController>().enabled = true; // Re-enable the character controller to allow movement
        Debug.Log(gameObject.name); // Log a message to the console for debugging purposes
        

    }
    public void test()
    {
        Debug.Log("Test function called!"); // Log a message to the console for testing purposes
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            damageIndicator.SetActive(true); // Show the damage indicator when hit by a projectile
            GameObject projectile = collision.gameObject;
            projectile.GetComponent<ProjectileBehavior>().collidedWithPlayer(this);
            damageIndicator.SetActive(false); // Hide the damage indicator when exiting the hazard
        }
        else if (collision.gameObject.CompareTag("Pickup"))
        {
            HealthPickupsBehaviour healthPickup = collision.gameObject.GetComponent<HealthPickupsBehaviour>();
            if (currentHealth >= maxHealth)
            {
                return; // If the player's health is already at maximum, do nothing
            }
            else
            {
                healthPickup.Pickup(); // Call the Pickup method on the health pickup object
            }
        }

    }
    void OnInteract()
    {
        // Check if the player can interact with objects
        if (canInteract)
        {
            // Check if the player has detected a coin or a door
            if (currentCollectible != null)
            {
                // Call the Collect method on the coin object
                // Pass the player object as an argument
                currentCollectible.Collect(this);
            }
            else if (currentDoor != null)
            {
                if (currentDoor.doorOpening || currentDoor.doorClosing)
                {
                    return; // If the door is already opening or closing, do nothing
                }
                else
                {// Call the Interact method on the door object
                    // This allows the player to open or close the door
                    currentDoor.Interact();
                }
            }
            else if (currentBox != null)
            {
                // Call the Interact method on the health box object
                // This allows the player to collect health items
                currentBox.Interact();
            }
            else if (currentObj != null)
            {
                if (objectivesCollected.Contains(currentObj.name))
                {

                    currentObj.Place(); // Place the object if it has already been collected
                    currentObj.tag = "Untagged"; // Remove the tag to prevent further interactions
                    currentObj = null; // Reset currentObj to null
                    currentScore += 1;
                }

            }
            else if (currentItem != null)
            {
                if (!carryingItem)
                {
                    carryingItem = true; // Set the carryingItem flag to true
                    currentItem.transform.position = spawnPoint.transform.position; // Set the item's position to the spawn point
                    currentItem.transform.SetParent(spawnPoint.transform); // Set the parent of the item to the player
                }
            }
        }
    }

    // Method to modify the player's score
    // This method takes an integer amount as a parameter
    // It adds the amount to the player's current score
    // The method is public so it can be accessed from other scripts
    public void collectedSomething(CollectibleBehaviour gameObject)
    {
        // Check the type of collectible and modify the score accordingly
        if (gameObject.collectibleType == "gun")
        {
            hasGun = true; // Set the hasGun flag to true   
            Debug.Log("Player has collected a gun!"); // Log that the player has collected a gun
            gunImage.SetActive(true); // Activate the gun image in the UI

        }
        else if (gameObject.collectibleType == "objective")
        {
            // If the collectible is an objective, add it to the objectivesCollected list
            if (!objectivesCollected.Contains(gameObject.collectibleType))
            {
                objectivesCollected.Add(gameObject.name);
                Debug.Log("Objective collected: " + gameObject.collectibleType); // Log the collected objective
                ;
            }
        }
        
    }

    // Method to modify the player's health
    // This method takes an integer amount as a parameter
    // It adds the amount to the player's current health
    // The method is public so it can be accessed from other scripts
    public void ModifyHealth(int amount)
    {
        // Check if the current health is less than the maximum health
        // If it is, increase the current health by the amount passed as an argument

        currentHealth += amount;
        // Check if the current health exceeds the maximum health
        // If it does, set the current health to the maximum health
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthText.text = "Health: " + currentHealth.ToString(); // Update the health text
    }

    // Trigger Callback for when the player exits a trigger collider
    void OnTriggerExit(Collider other)
    {
        // Check if the player has a detected coin or door
        if (currentCollectible != null)
        {
            // If the object that exited the trigger is the same as the current coin
            if (other.gameObject == currentCollectible.gameObject)
            {
                // Set the canInteract flag to false
                // Set the current coin to null
                // This prevents the player from interacting with the coin
                canInteract = false;
                currentCollectible.Unhighlight();
                currentCollectible = null;
            }
        }
        if (currentDoor != null)
        {
            // If the object that exited the trigger is the same as the current door
            if (other.gameObject == currentDoor.gameObject)
            {
                // Set the canInteract flag to false
                // Set the current door to null
                // This prevents the player from interacting with the door
                canInteract = false;
                currentDoor = null;
            }
        }
         damageIndicator.SetActive(false); // Hide the damage indicator when exiting the hazard
    }
    void OnFire()
    {
        if (hasGun)
        {
            GameObject newProjectile = Instantiate(projectile, spawnPoint.position, projectile.transform.rotation);
            Vector3 fireForce = spawnPoint.forward * fireStrength;
            newProjectile.GetComponent<Rigidbody>().AddForce(fireForce);

        }

    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            // If the player is in a hazard area, reduce health over time
            damageIndicator.SetActive(true); // Show the damage indicator when in the hazard
            ModifyHealth(-1); // Reduce health by 1 every frame while in the hazard
        }
    }
    
     
}