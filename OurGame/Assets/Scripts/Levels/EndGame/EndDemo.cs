using UnityEngine;

public class EndDemo : MonoBehaviour, IInteractables
{
    public void Interact()
    {
        EndDemoScreen();
    }

    private void EndDemoScreen()
    {
        Debug.Log("Transistion to the END VIDEO here");
    }
}
