using UnityEngine;

//https://www.youtube.com/watch?v=Eeee4TU69x4
// This script scrolls a UI credits panel upward over time.
// Attach it to a GameObject with a RectTransform (usually a child of a ScrollView or Canvas).
public class CreditScroll : MonoBehaviour
{
    public RectTransform creditContent;   // The RectTransform containing the credit text or content
    public float scrollSpeed = 30f;       // Speed at which the credits scroll upward (units per second)

    void Update()
    {
        // Move the credit content upward every frame based on scroll speed and frame time
        creditContent.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
    }
}