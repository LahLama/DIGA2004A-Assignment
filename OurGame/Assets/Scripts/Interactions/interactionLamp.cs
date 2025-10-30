using UnityEngine;

public class interactionLamp : MonoBehaviour, IInteractables
{
    public string lampOnSound = "LampOn";   // Name of the "on" sound clip in Resources
    public string lampOffSound = "LampOff"; // Name of the "off" sound clip in Resources

    public void Interact()
    {
        Light light = transform.GetChild(0).GetComponent<Light>();
        bool isTurningOn = !light.isActiveAndEnabled;
        light.enabled = isTurningOn;

        // Load and play appropriate sound
        string clipName = isTurningOn ? lampOnSound : lampOffSound;
        AudioClip clip = Resources.Load<AudioClip>(clipName);
        if (clip != null)
        {
            SoundManager.Instance.Play(clip);
        }
        else
        {
            Debug.LogWarning($"Lamp sound '{clipName}' not found in Resources.");
        }
    }
}