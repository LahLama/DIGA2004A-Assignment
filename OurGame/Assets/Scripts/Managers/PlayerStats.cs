using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public enum PlayerLevel
    {
        Tutorial,
        BaseGame,
    };

    public PlayerLevel playerLevel;

    void Start()
    {
        playerLevel = PlayerLevel.Tutorial;


    }


}
