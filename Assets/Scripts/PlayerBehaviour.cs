using System;

using UnityEngine;

using TMPro;
using Microsoft.Unity.VisualStudio.Editor;
public class PlayerBehaviour : MonoBehaviour
{
    int maxHealth = 100; // Player's maximum health
    int currentHealth = 100; // Player's current health
    int currentScore = 0; // Player's current score
    bool canInteract = false; // Flag to check if the player can interact with objects
    CollectibleBehaviour currentCoin = null; // Stores the current coin object the player has detected
    DoorBehaviour currentDoor = null; // Stores the current door object the player has detected
    HealthboxBehavior currentBox = null; // Stores the current health box object the player has detected
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
    GameObject gunImage; // Image component to display the gun icon
    bool hasGun = false; // Flag to check if the player has a gun
     [SerializeField]
    GameObject damageIndicator; // GameObject to indicate damage taken by the player

    void Start()
    {
        healthText.text = "Health: " + currentHealth.ToString(); // Initialize health text
        gunImage.SetActive(false); // Hide the gun image at the start
        damageIndicator.SetActive(false); // Hide the damage indicator at the start
    }
    void Update()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactDistance))
        {
            if (hitInfo.collider.CompareTag("Collectible"))
            {
                // Set the canInteract flag to true
                // Get the CollectibleBehaviour component from the detected object
                canInteract = true;
                currentCoin = hitInfo.collider.GetComponent<CollectibleBehaviour>();
                currentCoin.Highlight(); // Highlight the coin when detected
            }
            else if (currentCoin != null)
            {
                // If the raycast hits something else, unhighlight the current coin
                currentCoin.Unhighlight();
                currentCoin = null; // Reset currentCoin to null
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
        }
        else
        {
            // If the raycast does not hit any object, set canInteract to false
            canInteract = false;
            currentDoor = null; // Reset currentDoor to null
            currentBox = null; // Reset currentBox to null
        }
        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead");
            // Here you can add logic to handle player death, like respawning or ending the game
        }
    }
    // The Interact callback for the Interact Input Action
    // This method is called when the player presses the interact button
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            damageIndicator.SetActive(true); // Show the damage indicator when hit by a projectile
            GameObject projectile = collision.gameObject;
            projectile.GetComponent<ProjectileBehavior>().collidedWithPlayer(this);
             damageIndicator.SetActive(false); // Hide the damage indicator when exiting the hazard
        }

    }
    void OnInteract()
    {
        // Check if the player can interact with objects
        if (canInteract)
        {
            // Check if the player has detected a coin or a door
            if (currentCoin != null)
            {
                // Call the Collect method on the coin object
                // Pass the player object as an argument
                currentCoin.Collect(this);
            }
            else if (currentDoor != null)
            {
                // Call the Interact method on the door object
                // This allows the player to open or close the door
                currentDoor.Interact();
            }
            else if (currentBox != null)
            {
                // Call the Interact method on the health box object
                // This allows the player to collect health items
                currentBox.Interact();
            }
        }
    }

    // Method to modify the player's score
    // This method takes an integer amount as a parameter
    // It adds the amount to the player's current score
    // The method is public so it can be accessed from other scripts
    public void ModifyScore(string collectibleType)
    {
        // Check the type of collectible and modify the score accordingly
        if (collectibleType == "gun")
        {
            hasGun = true; // Set the hasGun flag to true
            Debug.Log("Player has collected a gun!"); // Log that the player has collected a gun
            gunImage.SetActive(true); // Activate the gun image in the UI

        }
        else if (collectibleType == "coin")
        {
            currentScore += 10; // Example score value for a coin
        }
        else if (collectibleType == "gem")
        {
            currentScore += 20; // Example score value for a gem
        }
        Debug.Log("Current Score: " + currentScore); // Log the current score
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
        if (currentCoin != null)
        {
            // If the object that exited the trigger is the same as the current coin
            if (other.gameObject == currentCoin.gameObject)
            {
                // Set the canInteract flag to false
                // Set the current coin to null
                // This prevents the player from interacting with the coin
                canInteract = false;
                currentCoin.Unhighlight();
                currentCoin = null;
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