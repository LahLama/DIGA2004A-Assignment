using UnityEngine;

public class PaperPopupController : MonoBehaviour
{
    public GameObject popupPanel;       // This panel itself
    public GameObject collectedIcon;    // The icon to show when closed

    public void ClosePopup()
    {
        popupPanel.SetActive(false);      // Hide this popup
        collectedIcon.SetActive(true);    // Show collected icon
    }
}

