using UnityEngine;
using UnityEngine.Video;

public class EndDemo : MonoBehaviour, IInteractables
{
    [SerializeField] private VideoPlayer endDemoVideoPlayer;

    public void Interact()
    {
        EndDemoScreen();
    }

    private void EndDemoScreen()
    {
        if (endDemoVideoPlayer != null)
        {
            endDemoVideoPlayer.Play();
            Debug.Log("Playing end demo video...");
        }
        else
        {
            Debug.LogWarning("End demo video player not assigned.");
        }
    }
}