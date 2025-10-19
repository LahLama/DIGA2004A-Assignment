using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject OptionsPanel;
    public GameObject mainMenuButtons;
    public GameObject UIPanel;
    private Transform player;
    private bool isPaused = false;

    public void NewGame()
    {
        Debug.Log("Starting New Game...");
        Time.timeScale = 1;
        PlayerPrefs.DeleteAll(); // Clear previous save
        SceneManager.LoadScene("GameScene");
    }

    public void ContinueGame()
    {
        Debug.Log("Continuing Game...");
        if (PlayerPrefs.HasKey("SavedScene"))
        {
            string savedScene = PlayerPrefs.GetString("SavedScene");
            Time.timeScale = 1;
            SceneManager.LoadScene(savedScene);
        }
        else
        {
            Debug.LogWarning("No saved game found. Starting new game...");
            NewGame();
        }
    }

    public void SaveGame()
    {
        SaveManager.SaveGame();
    }

    public void OpenSettings()
    {
        Debug.Log("Opening Settings...");
        settingsPanel.SetActive(true);
        mainMenuButtons.SetActive(false);
        //UIPanel.SetActive(false);
        //PauseGame();
    }

    public void OpenGameSettings(){
        Debug.Log("Opening Settings...");
        settingsPanel.SetActive(true);
        UIPanel.SetActive(false);
        PauseGame();
    }

    public void CloseGameSettings(){
        Debug.Log("Closing Settings...");
        settingsPanel.SetActive(false);
        ResumeGame();
        UIPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        Debug.Log("Closing Settings...");
        settingsPanel.SetActive(false);
        mainMenuButtons.SetActive(true);
        //ResumeGame();
       // UIPanel.SetActive(true);
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
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }

    private void PauseGame()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<LookFunction>().enabled = false;
        Time.timeScale = 0;
        OptionsPanel.SetActive(true);
        isPaused = true;
    }

    private void ResumeGame()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<LookFunction>().enabled = true;
        Time.timeScale = 1;
        OptionsPanel.SetActive(false);
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
                Cursor.lockState = CursorLockMode.None;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(OptionsPanel.transform.GetChild(0).gameObject);
            }
            else
            {
                ResumeGame();
                Cursor.lockState = CursorLockMode.Locked;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(GameObject.Find("ResumeButton"));
            }
        }
    }
}