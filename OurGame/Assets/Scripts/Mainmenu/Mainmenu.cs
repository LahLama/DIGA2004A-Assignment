using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;

    public void NewGame()
    {
        Debug.Log("Starting New Game...");
        SceneManager.LoadScene("GameScene"); // Replace with your actual game scene name
    }

    public void ContinueGame()
    {
        Debug.Log("Continuing Game...");
        // Add your load logic here (e.g., PlayerPrefs or save system)
        SceneManager.LoadScene("GameScene"); // Or load saved state
    }

    public void OpenSettings()
    {
        Debug.Log("Opening Settings...");
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        Debug.Log("Closing Settings...");
        settingsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }
}