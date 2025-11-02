using UnityEngine;

public class CreditScroll : MonoBehaviour
{
    public RectTransform creditContent;
    public float scrollSpeed = 30f;

    void Update()
    {
        creditContent.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
    }
}