using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html
//https://docs.unity3d.com/Manual/index.html
// MainMenu: handles main menu, pause/options UI, scene loading and basic save/continue behavior.
public class MainMenu : MonoBehaviour
{
    // UI panels and groups set from the Inspector
    public GameObject settingsPanel;    // general settings panel
    public GameObject OptionsPanel;     // in-game options/pause panel
    public GameObject mainMenuButtons;  // main menu buttons container
    public GameObject UIPanel;          // in-game UI container (HUD)

    // Player transform cached when pausing/resuming
    private Transform player;

    // Tracks whether the game is currently paused via the menu
    private bool isPaused = false;

    // Start a new game: reset time scale, clear saved prefs and load the game scene
    public void NewGame()
    {
        Debug.Log("Starting New Game...");
        Time.timeScale = 1;                 // ensure game time is running
        PlayerPrefs.DeleteAll();            // Clear previous save data
        SceneManager.LoadScene("GameScene"); // load new game scene
    }

    // Continue from a saved game if a saved scene exists, otherwise start a new game
    public void ContinueGame()
    {
        Debug.Log("Continuing Game...");
        if (PlayerPrefs.HasKey("SavedScene"))
        {
            string savedScene = PlayerPrefs.GetString("SavedScene");
            Time.timeScale = 1;                 // resume time before loading
            SceneManager.LoadScene(savedScene); // load saved scene
        }
        else
        {
            Debug.LogWarning("No saved game found. Starting new game...");
            NewGame();
        }
    }

    // Wrapper to call the save system (assumes SaveManager exists elsewhere)
    public void SaveGame()
    {
        SaveManager.SaveGame();
    }

    // Open settings from the main menu: show settings panel and hide main menu buttons
    public void OpenSettings()
    {
        Debug.Log("Opening Settings...");
        settingsPanel.SetActive(true);
        mainMenuButtons.SetActive(false);
        //UIPanel.SetActive(false);
        //PauseGame();
    }

    // Open in-game settings: show settings and pause the game, hide HUD
    public void OpenGameSettings()
    {
        Debug.Log("Opening Settings...");
        settingsPanel.SetActive(true);
        UIPanel.SetActive(false);
        PauseGame();
    }

    // Close in-game settings: hide settings, resume the game and re-enable HUD
    public void CloseGameSettings()
    {
        Debug.Log("Closing Settings...");
        settingsPanel.SetActive(false);
        ResumeGame();
        UIPanel.SetActive(true);
    }

    // Close settings opened from main menu: hide settings and show main menu buttons
    public void CloseSettings()
    {
        Debug.Log("Closing Settings...");
        settingsPanel.SetActive(false);
        mainMenuButtons.SetActive(true);
        //ResumeGame();
        // UIPanel.SetActive(true);
    }

    // Load the main menu scene (from other scenes)
    public void LoadMainMenu()
    {
        Debug.Log("Loading Main Menu...");
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    // Open options (currently just pauses the game and brings up options UI)
    public void OpenOptions()
    {
        Debug.Log("Opening Options...");
        PauseGame();
    }

    // Close options: resume gameplay and hide options UI
    public void CloseOptions()
    {
        Debug.Log("Closing Options...");
        ResumeGame();
    }

    // Load credits scene
    public void Credits()
    {
        Debug.Log("Showing Credits...");
        Time.timeScale = 1;
        SceneManager.LoadScene("CreditsScene");
    }

    // Close credits and return to main menu
    public void closeCredits()
    {
        Debug.Log("Closing Credits...");
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    // Quit the application (works in built player; no effect in editor)
    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }

    // Pause the game: disable player controls, stop time and show options panel
    private void PauseGame()
    {
        // Find the player by tag and cache the transform
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Disable movement and look scripts so player can't move/rotate while paused
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<LookFunction>().enabled = false;

        // Freeze game time (UI will still respond because UI uses unscaled time)
        Time.timeScale = 0;

        // Show the options/pause panel
        OptionsPanel.SetActive(true);

        isPaused = true;
    }

    // Resume the game: re-enable player controls, resume time and hide options panel
    private void ResumeGame()
    {
        // Find the player by tag and cache the transform (in case scene changed)
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Re-enable movement and look scripts
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<LookFunction>().enabled = true;

        // Resume game time
        Time.timeScale = 1;

        // Hide the options/pause panel
        OptionsPanel.SetActive(false);

        isPaused = false;
    }

    // Listen for Escape key to toggle pause/options UI
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                // Pause the game and show the options UI; unlock cursor so user can click
                PauseGame();
                Cursor.lockState = CursorLockMode.None;

                // Reset selected UI element then select the first child of OptionsPanel
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(OptionsPanel.transform.GetChild(0).gameObject);
            }
            else
            {
                // Resume the game and lock cursor back for gameplay
                ResumeGame();
                Cursor.lockState = CursorLockMode.Locked;

                // Reset selected UI element and set selection to the ResumeButton in the scene (if present)
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(GameObject.Find("ResumeButton"));
            }
        }
    }
}