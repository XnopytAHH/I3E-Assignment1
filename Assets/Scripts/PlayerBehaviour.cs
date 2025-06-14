using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
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
    PodiumBehavior currentPodium = null; // Stores the current podium object the player has detected
    PuzzleItemBehaviour currentItem = null; // Stores the current puzzle item object the player has detected
    [SerializeField]
    float interactDistance = 2.0f; // Distance within which the player can interact with objects
    [SerializeField]
    GameObject projectile = null; // Projectile prefab to instantiate when firing
    [SerializeField]
    Transform spawnPoint; // Transform where the projectile spawns
    [SerializeField]
    Transform carryPoint; // Transform where the item is carried
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
    GameObject carriedItem= null; // Flag to check if the player is carrying an item
    [SerializeField]
    Sprite[] gunSprites; // Array of sprites for the gun UI
    [SerializeField]
    Image gunImageUI; // Image component to display the gun icon in the UI
    [SerializeField]
    Sprite[] wingLSprites; // Array of sprites for the gun UI
    [SerializeField]
    Image wingLImageUI; // Image component to display the gun icon in the UI
    [SerializeField]
    Sprite[] wingRSprites; // Array of sprites for the gun UI
    [SerializeField]
    Image wingRImageUI; // Image component to display the gun icon in the UI
    [SerializeField]
    Sprite[] GeneratorSprites; // Array of sprites for the gun UI
    [SerializeField]
    Image GeneratorImageUI; // Image component to display the gun icon in the UI
    [SerializeField]
    Sprite[] BubbleSprites; // Array of sprites for the gun UI
    [SerializeField]
    Image BubbleImageUI; // Image component to display the gun icon in the UI
    [SerializeField]
    Image healthImageUI; // Image component to display the health icon in the UI
    [SerializeField]
    Sprite[] healthSprites; // Array of sprites for the health icon in the UI
    [SerializeField]
    Image wingLtickImageUI; // Image component to display the wingL tick icon in the UI
    [SerializeField]
    Image wingRtickImageUI; // Image component to display the wingR tick icon in the UI
    [SerializeField]
    Image GeneratorTickImageUI; // Image component to display the Generator tick icon in the UI
    [SerializeField]
    Image BubbleTickImageUI; // Image component to display the Bubble tick icon in the UI
    [SerializeField]
    Sprite TickSprite; // Sprite for the tick icon in the UI
    [SerializeField]
    TextMeshProUGUI score;
    [SerializeField]
    TextMeshProUGUI progress;
    void Start()
    {
        healthText.text = "Health: " + currentHealth.ToString(); // Initialize health text
        gunImage.SetActive(false); // Hide the gun image at the start
        damageIndicator.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.0f, false); // Set the damage indicator to fully transparent at the start
        DeathUI.enabled = false; // Disable the death UI
        Respawn(); // Call the Respawn method to set the player's initial position and state
        wingLImageUI.sprite = wingLSprites[0]; // Set the wingL icon sprite
        wingRImageUI.sprite = wingRSprites[0]; // Set the wingR icon sprite
        GeneratorImageUI.sprite = GeneratorSprites[0]; // Set the Generator icon sprite
        BubbleImageUI.sprite = BubbleSprites[0]; // Set the Bubble icon sprite

        gunImageUI.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); // Set the scale of the gun icon in the UI

        wingLImageUI.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); // Set the scale of the wingL icon in the UI

        wingRImageUI.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); // Set the scale of the wingR icon in the UI

        GeneratorImageUI.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); // Set the scale of the Generator icon in the UI

        BubbleImageUI.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); // Set the scale of the Bubble icon in the UI

        wingLtickImageUI.sprite = TickSprite; // Set the wingL tick icon sprite
        wingRtickImageUI.sprite = TickSprite; // Set the wingR tick icon sprite
        GeneratorTickImageUI.sprite = TickSprite; // Set the Generator tick icon sprite
        BubbleTickImageUI.sprite = TickSprite; // Set the Bubble tick icon sprite
        wingLtickImageUI.enabled = false; // Disable the wingL tick icon in the UI
        wingRtickImageUI.enabled = false; // Disable the wingR tick icon in the UI 
        GeneratorTickImageUI.enabled = false; // Disable the Generator tick icon in the UI
        BubbleTickImageUI.enabled = false; // Disable the Bubble tick icon in the UI
        
        score.text = currentScore + "/4";
        progress.text = currentScore * 25 + "%";


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
            else if (currentItem != null)
            {
                // If the raycast hits something else, unhighlight the current item
                currentItem.Unhighlight();
                currentItem = null; // Reset currentItem to null
            }
            if (hitInfo.collider.CompareTag("Puzzle"))
            {
                // Set the canInteract flag to true
                // Get the PodiumBehavior component from the detected object
                canInteract = true;
                currentPodium = hitInfo.collider.GetComponent<PodiumBehavior>();
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
            if (currentItem != null)
            {
                currentItem.Unhighlight(); // Unhighlight the current item if it exists
            }
            currentCollectible = null; // Reset currentCollectible to null
            currentItem = null; // Reset currentItem to null
            currentPodium = null; // Reset currentPodium to null
            currentObj = null; // Reset currentObj to null

        }
        if (currentHealth > 50)
        {
            healthImageUI.sprite = healthSprites[0]; // Set the health icon sprite to healthy
        }
        else if (currentHealth > 20)
        {
            healthImageUI.sprite = healthSprites[1]; // Set the health icon sprite to wounded
        }
        else if (currentHealth > 0)
        {
            healthImageUI.sprite = healthSprites[2]; // Set the health icon sprite to injured
        }
        else if (currentHealth <= 0)
        {
            Death(); // Call the Death method if health is zero or below
        }
        if (hasGun)
        {
            gunImageUI.sprite = gunSprites[1]; // Set the gun icon sprite
            gunImageUI.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f); // Set the scale of the gun icon in the UI
            
        }
        else
        {
            gunImageUI.sprite = gunSprites[0]; // Set the no gun icon sprite
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            damageIndicator.GetComponent<Image>().color = Color.red; // Set the damage indicator color to red
            damageIndicator.GetComponent<Image>().CrossFadeAlpha(1.0f, 0.1f, false); // Make the damage indicator fully visible
            damageIndicator.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.7f, false);
            GameObject projectile = collision.gameObject;
            projectile.GetComponent<ProjectileBehavior>().collidedWithPlayer(this);
            
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
                damageIndicator.GetComponent<Image>().color = Color.green; // Set the damage indicator color to green
                damageIndicator.GetComponent<Image>().CrossFadeAlpha(0.7f, 0.1f, false); // Make the damage indicator fully visible
                damageIndicator.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.7f, false); // Fade out the damage indicator after a short duration
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
                    if (currentObj.name == "wingL")
                    {
                        wingLtickImageUI.enabled = true; // Enable the tick icon for wingL

                    }
                    else if (currentObj.name == "wingR")
                    {
                        wingRtickImageUI.enabled = true; // Enable the tick icon for wingR

                    }
                    else if (currentObj.name == "Generator")
                    {
                        GeneratorTickImageUI.enabled = true; // Enable the tick icon for Generator

                    }
                    else if (currentObj.name == "Bubble")
                    {
                        BubbleTickImageUI.enabled = true; // Enable the tick icon for Bubble
                    }

                    currentObj = null; // Reset currentObj to null
                    currentScore += 1;
                    progress.text = currentScore * 25 + "%";
                }

            }
            else if (currentItem != null)
            {
                if (carriedItem == null)
                {
                    carriedItem = currentItem.gameObject; // Set the carried item to the current item
                    currentItem.transform.position = carryPoint.transform.position; // Set the item's position to in front of the spawn point
                    currentItem.transform.SetParent(spawnPoint.transform); // Set the parent of the item to the player
                    currentItem.GetComponent<Rigidbody>().isKinematic = true; // Make the item kinematic to prevent physics interactions
                    currentItem.gameObject.layer = 2; // Set the layer to Ignore Raycast to prevent further interactions
                }
            }
            else if (currentPodium != null)
            {
                if (carriedItem != null && currentPodium.floatingObject == null)
                {
                    currentPodium.PlaceObject(carriedItem); // Place the carried item on the podium
                    carriedItem = null; // Reset the carried item to null
                }
                else if (currentPodium.floatingObject != null)
                {
                    if (carriedItem != null)
                    {
                        Debug.Log("You already have an item in your hands!"); // Log a message if the player is already carrying an item
                        return; // Exit the method if the player is already carrying an item
                    }
                    else
                    {
                        carriedItem = currentPodium.floatingObject; // Set the carried item to the floating object on the podium
                        currentPodium.RemoveObject(); // Remove the floating object from the podium
                        carriedItem.transform.position = carryPoint.transform.position; // Set the item's position to in front of the spawn point
                        carriedItem.transform.SetParent(spawnPoint.transform); // Set the parent of the item to the player
                        carriedItem.GetComponent<Rigidbody>().isKinematic = true; // Make the item kinematic to prevent physics interactions
                        carriedItem.gameObject.layer = 2; // Set the layer to Ignore Raycast to prevent further interactions
                    }
                }
                else
                {
                    Debug.Log("No item to place on the podium!"); // Log a message if no item is carried
                }
            }

        }
        else if (carriedItem != null)
        {
            carriedItem.transform.SetParent(null); // Remove the parent of the item to drop it
            carriedItem.GetComponent<Rigidbody>().isKinematic = false; // Set the item to non-kinematic to allow physics interactions
            carriedItem.gameObject.layer = 0; // Set the layer back to Default
            carriedItem = null; // Reset the carried item to null

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
            if (gameObject.name == "wingL")
            {
                wingLImageUI.sprite = wingLSprites[1]; // Set the wingL icon sprite
                wingLImageUI.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f); // Set the scale of the wingL icon in the UI

            }
            else if (gameObject.name == "wingR")
            {
                wingRImageUI.sprite = wingRSprites[1]; // Set the wingR icon sprite
                wingRImageUI.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f); // Set the scale of the wingR icon in the UI

            }
            else if (gameObject.name == "Generator")
            {
                GeneratorImageUI.sprite = GeneratorSprites[1]; // Set the Generator icon sprite
                GeneratorImageUI.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f); // Set the scale of the Generator icon in the UI

            }
            else if (gameObject.name == "Bubble")
            {
                BubbleImageUI.sprite = BubbleSprites[1]; // Set the Bubble icon sprite
                BubbleImageUI.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f); // Set the scale of the Bubble icon in the UI

            }
            score.text = objectivesCollected.Count + "/4";
        
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
            damageIndicator.GetComponent<Image>().color = Color.red; // Set the damage indicator color to red
            damageIndicator.GetComponent<Image>().CrossFadeAlpha(1.0f, 0.1f, false); // Make the damage indicator fully visible
            damageIndicator.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.7f, false); // Fade out the damage indicator after a short duration
            ModifyHealth(-1); // Reduce health by 1 every frame while in the hazard
        }
    }
    
     
}