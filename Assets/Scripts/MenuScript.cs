using UnityEngine;
/*
* Author: Lim En Xu Jayson
* Date: 13/6/2025
* Description: Handles the main menu interactions.
*/

public class MenuScript : MonoBehaviour
{
    /// <summary>
    /// Method to start the game when the start button is pressed.
    /// </summary>
    public void StartGame()
    {
        // Load the game scene when the start button is pressed
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}