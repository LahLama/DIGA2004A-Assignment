using UnityEngine;
using UnityEngine.SceneManagement;

// This script handles the interaction that ends the demo and loads a designated video scene.
// It assumes the object implements the IInteractables interface.
public class EndDemo : MonoBehaviour, IInteractables
{
    [Header("Scene Settings")]
    public string videoSceneName = "EndVideoScene"; // Name of the scene to load when interaction occurs

    // Called when the player interacts with this object
    public void Interact()
    {
        // Log interaction for debugging purposes
        Debug.Log("Door opened. Loading video scene...");

        // Load the specified scene (e.g., a video or end screen)
        SceneManager.LoadScene(videoSceneName);
    }
}
