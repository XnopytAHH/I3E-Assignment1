using UnityEngine;

public class MenuScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        // Load the game scene when the start button is pressed
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}