using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Singleton instance
    public static PlayerStats Instance { get; private set; }

    public enum PlayerLevel
    {
        Cutscene,
        Tutorial,
        BaseGame,
    };

    public PlayerLevel playerLevel;

    void Awake()
    {
        // enforce single instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        playerLevel = PlayerLevel.Cutscene;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}