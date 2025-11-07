using UnityEngine;
using UnityEngine.SceneManagement;

public class EndDemo : MonoBehaviour, IInteractables
{
    [Header("Scene Settings")]
    public string videoSceneName = "EndVideoScene"; // Set this to your video scene name

    public void Interact()
    {
        Debug.Log("Door opened. Loading video scene...");
        SceneManager.LoadScene(videoSceneName);
    }
}
