using UnityEngine;
using UnityEngine.SceneManagement;

// Handles main menu button actions.
public class MainMenuButtons : MonoBehaviour
{
    // Load the game scene (replace "GameScene" with your scene name).
    public void OnPlayClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    // Show credits (currently logs a message).
    public void OnCreditClicked()
    {
        Debug.Log("Credit button clicked.");
    }

    // Quit the app; stops play mode in the Editor.
    public void OnQuitClicked()
    {
        Debug.Log("Quit button clicked.");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
