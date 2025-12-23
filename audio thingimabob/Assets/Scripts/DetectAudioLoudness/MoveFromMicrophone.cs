using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
    Title: How to Use Your Voice as Input in Unity - Microphone and Audio Loudness Detection
    Author: Valem Tutorials
    Date:  Feb 16, 2022
    Availability: https://www.youtube.com/watch?v=dzD0qP8viLw
    */
public class MoveFromMicrophone : MonoBehaviour
{
    public float sensibility = 100;
    public float threshold = 0.3f;
    private float Prevloudness;
    private float loudness;
    public AudioLoudnessDetection detector;

    public bool isLoud = false;


    void Update()
    {

        /* loudness = detector.GetLoudnessFromMicrophone() * sensibility;

         if (loudness > threshold)
             loudness = loudness - threshold;

         if (slider.value > _loudRange)
         {
             _barColorbg.color = new Color(1f, 0.5f, 0.5f, 1f);
             isLoud = true;
             StartCoroutine(Cooldown(NunlookTime));
         }
         else
         {
             _barColorbg.color = new Color(0.5f, 0.5f, 1f, 1f); ;

         }

         Prevloudness = loudness;*/

    }

    private IEnumerator Cooldown(float delay)
    {
        float timer = delay;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        isLoud = false;
    }
}
