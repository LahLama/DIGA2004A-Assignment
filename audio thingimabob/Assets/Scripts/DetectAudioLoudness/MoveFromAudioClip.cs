using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/*
    Title: How to Use Your Voice as Input in Unity - Microphone and Audio Loudness Detection
    Author: Valem Tutorials
    Date:  Feb 16, 2022
    Availability: https://www.youtube.com/watch?v=dzD0qP8viLw
    */
public class MoveFromAudioClip : MonoBehaviour
{
    public AudioSource source;
    public float minHeight;
    public float maxHeight;
    public float sensibility = 100;
    public float threshold = 0.1f;
    public float maxTime;
    public AudioLoudnessDetection detector;
    public GameObject[] Soundtrack;
    private float itemTimer = 0f;
    private int index;

    void Awake()
    {
        maxTime = source.clip.length / detector.sampleWindow;
        Debug.Log("MaxTime: " + maxTime);
    }

    void Update()
    {
        float loudness = detector.GetLoudnessFromAudioClip(source.timeSamples, source.clip) * sensibility;

        if (loudness > threshold)
        {
            // Debug.Log("Loudness: " + loudness);
            // Debug.Log("Index: " + Soundtrack[index].name);

            float YPos = Soundtrack[index].transform.position.y;
            itemTimer += Time.deltaTime;

            if (loudness > threshold && itemTimer > maxTime)
            {


                Soundtrack[index].transform.position =
                    Vector3.Lerp(
                        new Vector3(Soundtrack[index].transform.position.x, YPos, Soundtrack[index].transform.position.z),
                        new Vector3(Soundtrack[index].transform.position.x, YPos + loudness, Soundtrack[index].transform.position.z),
                        loudness);

                index++;


                // Reset pos after a few seconds:
                ResetPos(YPos, loudness);



                if (index >= Soundtrack.Length)
                    index = 0;



                itemTimer = 0;
                return;
            }
        }
        else
            loudness = 0;
    }

    private void ResetPos(float Yposition, float loudness)
    {
        if (index == 0) Soundtrack[Soundtrack.Length - 1].transform.position = Vector3.Lerp(
                        new Vector3(Soundtrack[index].transform.position.x, Yposition, Soundtrack[index].transform.position.z),
                        new Vector3(Soundtrack[index].transform.position.x, Yposition - loudness, Soundtrack[index].transform.position.z),
                        loudness);


        else
        {
            Soundtrack[index - 1].transform.position = Vector3.Lerp(
                        new Vector3(Soundtrack[index].transform.position.x, Yposition, Soundtrack[index].transform.position.z),
                        new Vector3(Soundtrack[index].transform.position.x, Yposition - loudness, Soundtrack[index].transform.position.z),
                        loudness);


        }

    }
}
