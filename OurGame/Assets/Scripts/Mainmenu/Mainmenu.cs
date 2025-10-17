using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject OptionsPanel;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void NewGame()
    {
        Debug.Log("Starting New Game...");
        Time.timeScale = 1; // Ensure game is unpaused
        SceneManager.LoadScene("GameScene");
    }

    public void ContinueGame()
    {
        Debug.Log("Continuing Game...");
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
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

    public void LoadMainMenu()
    {
        Debug.Log("Loading Main Menu...");
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenOptions()
    {
        Debug.Log("Opening Options...");
        PauseGame();
    }

    public void CloseOptions()
    {
        Debug.Log("Closing Options...");
        ResumeGame();
    }

    public void Credits()
    {
        Debug.Log("Showing Credits...");
        Time.timeScale = 1;
        SceneManager.LoadScene("CreditsScene");
    }

    public void closeCredits()
    {
        Debug.Log("Closing Credits...");
        Time.timeScale = 1;
        SceneManager.LoadScene("Main menu");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        OptionsPanel.SetActive(true);
        isPaused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        OptionsPanel.SetActive(false);
        isPaused = false;
    }
}