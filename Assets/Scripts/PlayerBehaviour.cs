using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;
using StarterAssets;
using System.Security.Cryptography;
using Unity.Collections;
using UnityEngine.Rendering;
/*
* Author: Lim En Xu Jayson
* Date: 9/6/2025 
* Description: Handles the behavior of the player in the game.
*/

public class PlayerBehaviour : MonoBehaviour
{
    /// <summary>
    /// playerSpawnPoint is a Transform that defines the initial spawn position of the player in the game world.
    /// It is used to reset the player's position when they respawn after death.
    /// It is set to a empty GameObject in the scene to dictate where the player will start the game and respawn after death.
    /// </summary>
    [SerializeField]
    Transform playerSpawnPoint; 
    /// <summary>
    /// maxHealth is an integer that defines the maximum health of the player.
    /// It is set to 100 by default and is used to determine the player's health status in the game.
    /// </summary>
    int maxHealth = 100; 
    /// <summary>
    /// currentHealth is an integer that represents the player's current health.
    /// It is 100 by default and is raised and lowered during the game.
    /// </summary>
    int currentHealth = 100; 
    /// <summary>
    /// currentScore is an integer that represents the player's current score.
    /// The score is incremented when the player fixes areas of their ship
    /// </summary>
    int currentScore = 0; 
    /// <summary>
    /// canInteract is a boolean that indicates whether the player can interact with objects in the game.
    /// It is set to false by default and is set to true when the player is within range of an interactable object.
    /// </summary>
    bool canInteract = false; 
    /// <summary>
    /// currentCollectible stores the current coin object the player has detected.
    /// It is set to null by default and is assigned when the player detects a collectible object with the Raycast.
    /// </summary>
    CollectibleBehaviour currentCollectible = null; 
    /// <summary>
    /// currentDoor stores the current door object the player has detected.
    /// It is set to null by default and is assigned when the player detects a door object with the Raycast.
    /// </summary>
    DoorBehaviour currentDoor = null; 
    /// <summary>
    /// currentBox stores the current health box object the player has detected.
    /// It is set to null by default and is assigned when the player detects a health box object with the Raycast.
    /// </summary>
    HealthboxBehavior currentBox = null; 
    /// <summary>
    /// currentObj stores the current health box object the player has detected.
    /// It is set to null by default and is assigned when the player detects a ship component object with the Raycast.
    ShipComponents currentObj = null; 
    /// <summary>
    /// currentPodium stores the current podium object the player has detected.
    /// It is set to null by default and is assigned when the player detects a podium object with the Raycast.
    /// </summary>
    PodiumBehavior currentPodium = null; 
    /// <summary>
    /// currentItem stores the current puzzle item object the player has detected.
    /// It is set to null by default and is assigned when the player detects a puzzle item object with the Raycast.
    /// </summary>
    PuzzleItemBehaviour currentItem = null; 
    /// <summary>
    /// interactDistance is a float that defines the distance within which the player can interact with objects.
    /// It is set to 2.0f by default and is used to determine the range of interaction for the player.
    /// </summary>
    [SerializeField]
    float interactDistance = 2.0f; 
    /// <summary>
    /// projectile identifies the projectile prefab that the player can fire.
    /// It is set to a prefab in the scene and is used to create projectiles when the player fires their weapon.
    /// </summary>
    [SerializeField]
    GameObject projectile = null; 
    /// <summary>
    /// spawnPoint stores the position and rotation of where the projectile will spawn when fired.
    /// It is set to a empty game object in the scene and is used to determine the spawn point of the projectile.
    /// </summary>
    [SerializeField]
    Transform spawnPoint; 
    /// <summary>
    /// carryPoint stores the position where the player carries an item.
    /// It is set to a empty game object in the scene and is used to determine where the carried item will be positioned in front of the player.
    /// It is used in the puzzle item interaction to place the item in front of the player when they pick it up.
    /// </summary>
    [SerializeField]
    Transform carryPoint; 
    /// <summary>
    /// fireStrength is an integer that defines the strength of the projectile fire force.
    /// It is set to 0 by default and is used to determine how fast the projectile will travel when fired.
    /// </summary>
    [SerializeField]
    int fireStrength = 0; 
    /// <summary>
    /// healthText is a TextMeshProUGUI component that displays the player's current health in the UI.
    /// It is set to a TextMeshProUGUI component in the scene and is used to update the player's health status in the UI.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI healthText; 
    /// <summary>
    /// PlayerUI is a Canvas component that displays the player's UI elements.
    /// </summary>
    [SerializeField]
    Canvas PlayerUI; 
    /// <summary>
    /// DeathUI is a Canvas component that displays the death UI elements when the player dies.
    /// It is set to a Canvas component in the scene and is used to show the death screen when the player dies.
    /// </summary>
    [SerializeField]
    Canvas DeathUI;
    /// <summary>
    /// gunImage is a GameObject that displays the gun image that is shown on the player's UI.
    /// It is activated when the player collects a gun.
    /// </summary>
    [SerializeField]
    GameObject gunImage; 
    /// <summary>
    /// gunDisplaySprite is a Sprite that represents the gun icon in the UI.
    /// It is set to a Sprite in the scene and is used to display the gun icon when the player collects a gun.
    /// </summary>
    [SerializeField]
    Sprite gunDisplaySprite;
    /// <summary>
    /// hasGun is a boolean that indicates whether the player has collected a gun.
    /// It is set to false by default and is set to true when the player collects a gun.
    /// </summary>
    bool hasGun = false;
    /// <summary>
    /// objectivesCollected is a List that stores the names of the objectives that the player has collected.
    /// It acts as an inventory for the player's collected objectives.
    /// </summary>
    List<string> objectivesCollected = new List<string>();
    /// <summary>
    /// dammageIndicator is a red UI element that indicates when the player has taken damage.
    /// </summary>
    [SerializeField]
    GameObject damageIndicator; 
    /// <summary>
    /// carriedItem is a GameObject that stores the item the player is currently carrying.
    /// </summary>
    GameObject carriedItem = null;
    /// <summary>
    /// gunSprites is an array of Sprites that represent the gun icon in the UI.
    /// </summary>
    [SerializeField]
    Sprite[] gunSprites; 
    /// <summary>
    /// gunImageUI is an Image component that displays the gun icon in the UI.
    /// It is set to an Image component in the scene and is used to update the gun icon in the UI.
    /// </summary>
    [SerializeField]
    Image gunImageUI; 
    /// <summary>
    /// wingLSprites is an array of Sprites that represent the left wing icon in the UI.
    /// It is used to display the left wing icon when the player collects the left wing objective.
    /// </summary>
    [SerializeField]
    Sprite[] wingLSprites; 
    /// <summary>
    /// wingLImageUI is an Image component that displays the left wing icon in the UI.
    /// It is set to an Image component in the scene and is used to update the left wing icon in the UI.
    /// </summary>
    [SerializeField]
    Image wingLImageUI; 
    /// <summary>
    /// wingRSprites is an array of Sprites that represent the right wing icon in the UI.
    /// It is used to display the right wing icon when the player collects the right wing objective.
    ///     </summary>
    [SerializeField]
    Sprite[] wingRSprites; 
    /// <summary>
    /// wingRImageUI is an Image component that displays the right wing icon in the UI.
    /// It is set to an Image component in the scene and is used to update the right wing icon in the UI.
    /// </summary>
    [SerializeField]
    Image wingRImageUI; 
    /// <summary>
    /// GeneratorSprites is an array of Sprites that represent the generator icon in the UI.
    /// It is used to display the generator icon when the player collects the generator objective.
    /// </summary>
    [SerializeField]
    Sprite[] GeneratorSprites;
    /// <summary>
    /// GeneratorImageUI is an Image component that displays the generator icon in the UI.
    /// It is set to an Image component in the scene and is used to update the generator icon in the UI.
    /// </summary>
    [SerializeField]
    Image GeneratorImageUI;
    /// <summary>
    /// BubbleSprites is an array of Sprites that represent the bubble icon in the UI.
    /// It is used to display the bubble icon when the player collects the bubble objective.
    /// </summary>
    [SerializeField]
    Sprite[] BubbleSprites;
    /// <summary>
    /// BubbleImageUI is an Image component that displays the bubble icon in the UI.
    /// It is set to an Image component in the scene and is used to update the bubble icon in the UI.
    /// </summary>
    [SerializeField]
    Image BubbleImageUI;
    /// <summary>
    /// healthImageUI is an Image component that displays the health icon in the UI.
    /// It is set to an Image component in the scene and is used to update the health icon based on the player's current health.
    /// </summary>
    [SerializeField]
    Image healthImageUI; 
    /// <summary>
    /// healthSprites is an array of Sprites that represent the health icon in the UI.
    /// It is used to display different health icons based on the player's current health status.
    /// It has 3 different sprites for healthy, wounded, and injured states.
    /// </summary>
    [SerializeField]
    Sprite[] healthSprites;
    /// <summary>
    /// wingLtickImageUI is an Image component that displays the tick icon for the left wing in the UI.
    /// It is set to an Image component in the scene and is used to indicate that the left wing objective has been added to the ship.
    /// </summary>
    [SerializeField]
    Image wingLtickImageUI; 
    /// <summary>
    /// wingRtickImageUI is an Image component that displays the tick icon for the right wing in the UI.
    /// It is set to an Image component in the scene and is used to indicate that the right wing objective has been added to the ship.
    /// </summary>
    [SerializeField]
    Image wingRtickImageUI; 
    /// <summary>
    /// GeneratorTickImageUI is an Image component that displays the tick icon for the generator in the UI.
    /// It is set to an Image component in the scene and is used to indicate that the generator objective has been added to the ship.
    /// </summary>
    [SerializeField]
    Image GeneratorTickImageUI;
    /// <summary>
    /// BubbleTickImageUI is an Image component that displays the tick icon for the bubble in the UI.
    /// It is set to an Image component in the scene and is used to indicate that the bubble objective has been added to the ship.
    /// </summary>
    [SerializeField]
    Image BubbleTickImageUI; 
    /// <summary>
    /// TickSprite is a Sprite that represents the tick icon in the UI.
    /// It is used to indicate that an objective has been collected and added to the ship.
    /// </summary>
    [SerializeField]
    Sprite TickSprite; 
    /// <summary>
    /// score is a TextMeshPro component that displays the player's current score in the UI.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI score;
    /// <summary>
    /// progress is a TextMeshPro component that displays the player's progress in the UI.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI progress;
    /// <summary>
    /// tutorialImage is an Image component that displays the tutorials in the UI.
    /// </summary>
    [SerializeField]
    Image tutorialImage; 
    /// <summary>
    /// tutorialSprites is an array of Sprites that represent the tutorial images in the UI.
    /// It is used to display different tutorial images based on the player's progress in the game.
    /// </summary>
    [SerializeField]
    Sprite[] tutorialSprites; 
    /// <summary>
    /// deathCount is an integer that keeps track of the number of times the player has died in the game.
    /// it is used to display the death count on the win screen.
    /// </summary>
    int deathCount = 0;
    /// <summary>
    /// winScreen is a Canvas component that displays the win screen when the player completes the game.
    /// It is set to a Canvas component in the scene and is used to show the win screen when the player collects all objectives and fully repairs their ship.
    /// </summary>
    [SerializeField]
    Canvas winScreen; 
    /// <summary>
    /// winText is a TextMeshPro component that displays the death count on the win screen.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI winText; 
    /// <summary>
    /// audioSource is an AudioSource component that plays sound effects in the game.
    /// It is used to play the shooting sound
    /// </summary>
    AudioSource audioSource; 
    /// <summary>
    /// deathAudioClip is an AudioClip that plays when the player dies.
    /// It is set to an AudioClip in the scene and is used to play the death sound effect when the player dies.
    /// </summary>
    [SerializeField]
    AudioClip deathAudioClip; 
    /// <summary>
    /// Start is called once at the beginning of the game.
    /// It initializes most of the variables states
    /// </summary>
    void Start()
    {
        healthText.text = "Health: " + currentHealth.ToString(); // Initialize health text
        gunImage.SetActive(false); // Hide the gun image at the start

        damageIndicator.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.0f, false); // Set the damage indicator to fully transparent at the start
        DeathUI.enabled = false; // Disable the death UI
        winScreen.enabled = false; // Disable the win screen at the start
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
        DisplayTutorial(0); // Display the first tutorial image at the start
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to the player


    }
    /// <summary>
    /// Update is called once per frame.
    /// It checks for raycasts to detect interactable objects and updates the player's health and UI elements accordingly.
    /// </summary>
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
    /// <summary>
    /// Death is a function that is called when the player's health reaches zero.
    /// It disables the main player UI, enables the death UI, resets the player's health, unlocks the cursor, increments the death count, and disables the character controller to prevent movement.
    /// It also plays a death sound effect.
    /// </summary>
    void Death()
    {
        PlayerUI.enabled = false; // Disable the main player UI
        DeathUI.enabled = true; // Enable the death UI
        currentHealth = 100; // Set the player's health to zero
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        deathCount += 1; // Increment the death count
        gameObject.GetComponent<FirstPersonController>().enabled = false; // Disable the character controller to prevent movement
        AudioSource.PlayClipAtPoint(deathAudioClip, transform.position, 0.5f); // Play the death sound effect

    }
    /// <summary>
    /// Respawn is a function that resets the player to their spawn point after death
    /// It resets the player's health to maximum, updates the health text, enables the main player UI, disables the death UI, locks the cursor, and re-enables the character controller to allow movement.
    /// </summary>
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
    /// <summary>
    /// OnCollisionEnter detects collisions with other objects in the game.
    /// It is used to check if the player collides with a projectile or a health pickup.
    /// </summary>
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
    /// <summary>
    /// OnInteract is a function that is called when the player interacts with an object in the game.
    /// It checks if the player can interact with objects and performs the appropriate action based on the type of object detected.
    /// </summary>
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
                    if (currentScore >= 4)
                    {
                        WinGame(); // Call the WinGame method if all objectives are collected
                    }
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

    /// <summary>
    /// collectedSomething is a function that is called when the player collects an object in the game.
    /// It checks the type of collectible and modifies the player's score and UI accordingly.
    /// </summary>
    public void collectedSomething(CollectibleBehaviour gameObject)
    {
        // Check the type of collectible and modify the score accordingly
        if (gameObject.collectibleType == "gun")
        {
            hasGun = true; // Set the hasGun flag to true   
            DisplayTutorial(3); // Display the tutorial for the gun
            gunImage.GetComponent<Image>().sprite = gunDisplaySprite; // Set the gun image sprite in the UI
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
            if (objectivesCollected.Count >= 4)
            {
                score.text = objectivesCollected.Count + "/4 <br> Remember to place them in your ship!";
            }
            else
            {
                score.text = objectivesCollected.Count + "/4";
            }
            

        }

    }

    /// <summary>
    /// ModifyHealth is a function that modifies the player's health by a specified amount.
    /// </summary>
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

    /// <summary>
    /// OnFire is a function that is called when the player fires their gun.
    /// It checks if the player has a gun and plays the shooting sound effect.
    /// </summary>
    void OnFire()
    {
        if (hasGun)
        {
            audioSource.Play(); // Play the shooting sound
            GameObject newProjectile = Instantiate(projectile, spawnPoint.position, projectile.transform.rotation);
            Vector3 fireForce = spawnPoint.forward * fireStrength;
            newProjectile.GetComponent<Rigidbody>().AddForce(fireForce);

        }

    }
    /// <summary>
    /// OnTriggerEnter is a function that is called when the player enters a trigger collider.
    /// It is used for the lava hazard in the game.
    /// It plays a damage sound effect when the player enters a hazard trigger 
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        // Check if the player enters a hazard trigger
        if (other.CompareTag("Hazard"))
        {
            AudioSource damageAudioSource = other.GetComponent<AudioSource>();
            damageAudioSource.time = 0.1f; // Reset the audio source time to the beginning
            damageAudioSource.Play(); // Play the damage sound effect
        }
    }
    /// <summary>
    /// OnTriggerStay is a function that is called every frame while the player is inside a trigger collider.
    /// It is used for the lava hazard in the game.
    /// It reduces the player's health by 1 every frame while the player is inside the hazard trigger.
    /// </summary>
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
    /// <summary>
    /// DisplayTutorial is a function that displays a tutorial image based on the tutorial number.
    /// </summary>
    public void DisplayTutorial(int tutorialNumber)
    {
        StopCoroutine(HideTutorialAfterDelay(3.0f)); // Stop any existing coroutine to prevent overlapping fades
        tutorialImage.CrossFadeAlpha(1.0f, 0.1f, false);
        tutorialImage.sprite = tutorialSprites[tutorialNumber]; // Set the initial sprite for the tutorial image
        StartCoroutine(HideTutorialAfterDelay(3.0f)); // Start a coroutine to hide the tutorial image after a delay

    }

    /// <summary>
    /// HideTutorialAfterDelay is a coroutine that waits for a specified delay before fading out the tutorial image.
    /// </summary>
    IEnumerator HideTutorialAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        tutorialImage.CrossFadeAlpha(0.0f, 0.7f, false); // Fade out the tutorial image after the delay
    }
    /// <summary>
    /// WinGame is a function that is called when the player wins the game.
    /// </summary>
    public void WinGame()
    {
        winScreen.GetComponent<AudioSource>().Play(); // Play the win screen audio
        PlayerUI.enabled = false; // Disable the main player UI
        winScreen.enabled = true; // Enable the win screen
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        gameObject.GetComponent<FirstPersonController>().enabled = false; // Disable the character controller to prevent movement
        winText.text = "Deaths= " + deathCount;

    }
}