using UnityEngine;

public class PaperCollectible : MonoBehaviour
{
    public GameObject PaperPanel; 
    private bool isCollected = false;
 void OnMouseDown()
    {
        if (isCollected) return;

        
        PaperPanel.SetActive(true);

        
        gameObject.SetActive(false);
        isCollected = true;
    }
}
