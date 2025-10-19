using System.Collections;
using UnityEngine;

public class LightningController : MonoBehaviour
{
    public Light Thunder1;
    public Light Thunder2;
    public Light Thunder3;
    public Light Thunder4;

    public float minTimeBetweenFlashes = 2f;
    public float maxTimeBetweenFlashes = 5f;
    public float flashDuration = 0.15f;

    private Light[] thunderLights;
    private float flashIntensity = 10f;
    private float flashRange = 3.881924f;

    void Start()
     {
        // Group all lights
        thunderLights = new Light[] { Thunder1, Thunder2, Thunder3, Thunder4 };

        // Set fixed intensity and range + turn all off
        foreach (Light light in thunderLights)
        {
            if (light != null)
            {
                light.intensity = flashIntensity;
                light.range = flashRange;
                light.enabled = false;
            }
        }

        StartCoroutine(FlashRoutine());
    }IEnumerator FlashRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minTimeBetweenFlashes, maxTimeBetweenFlashes);
            yield return new WaitForSeconds(waitTime);

            int numberOfFlashes = Random.Range(1, 3); // do 1 or 2 quick flashes

            for (int i = 0; i < numberOfFlashes; i++)
            {
                // Randomly choose which lights to flash
                foreach (Light light in thunderLights)
                {
                    if (Random.value > 0.5f && light != null)
                    {
                        light.enabled = true;
                    }
                }
                yield return new WaitForSeconds(flashDuration);

                // Turn off all lights
                foreach (Light light in thunderLights)
                {
                    if (light != null)
                        light.enabled = false;
                }

                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}