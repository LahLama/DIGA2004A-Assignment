using UnityEngine;

public class PlayerPositionLoader : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerX"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            float z = PlayerPrefs.GetFloat("PlayerZ");

            transform.position = new Vector3(x, y, z);
            Debug.Log("Player position loaded.");
        }
    }
}
