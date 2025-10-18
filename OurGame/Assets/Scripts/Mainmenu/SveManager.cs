using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveManager
{
    public static void SaveGame()
    {
        PlayerPrefs.SetString("SavedScene", SceneManager.GetActiveScene().name);

        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerPrefs.SetFloat("PlayerX", player.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.position.z);

        PlayerPrefs.Save();
        Debug.Log("Game Saved.");
    }
}