using TMPro;
using UnityEngine;

public class LivesTracker : MonoBehaviour
{
    [SerializeField] private int currentLives = 8;
    private TextMeshProUGUI lifeTracker;

    void Awake()
    {
        lifeTracker = GameObject.FindGameObjectWithTag("LifeTracker").GetComponent<TextMeshProUGUI>();
        currentLives = 8;
    }
    public void RecieveMessageCatchPlayer()
    {
        currentLives++;
        lifeTracker.text = currentLives + ":00";


        if (currentLives == 12)
        {
            Debug.Log("Game over. You ran out of time");
        }
    }
}
