using UnityEngine;
using UnityEngine.SceneManagement;

// This script logs the name of the currently active scene when the game starts.
public class SceneTracker : MonoBehaviour
{
    void Start()
    {
        // Get the currently active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Extract the name of the scene
        string sceneName = currentScene.name;

        // Output the scene name to the console for debugging or tracking purposes
        Debug.Log("Current Scene: " + sceneName);
    }
}