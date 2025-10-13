using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void OnPlayClicked()
    {
        // Replace with your actual scene name
        SceneManager.LoadScene("GameScene");
    }

    public void OnCreditClicked()
    {
        Debug.Log("Credit button clicked.");
        // Optional: Show a credits panel here
    }
      public void OnQuitClicked()
    {
        Debug.Log("Quit button clicked.");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}