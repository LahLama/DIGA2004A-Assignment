using UnityEngine;

public class PaperClickHandler : MonoBehaviour
{
    public GameObject popupPanel; // The panel to show when clicked

    private bool hasBeenClicked = false;

    private void OnMouseDown()
    {
        if (hasBeenClicked) return;

        hasBeenClicked = true;

        // Show the popup panel
        popupPanel.SetActive(true);

        // Hide the paper in the world
        gameObject.SetActive(false);
    }
}