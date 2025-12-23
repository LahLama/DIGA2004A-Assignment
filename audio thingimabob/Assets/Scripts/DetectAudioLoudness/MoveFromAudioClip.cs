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
    public float maxTime = 0.1f;
    public AudioLoudnessDetection detector;
    public GameObject[] Soundtrack;
    private float itemTimer = 0f;
    private int index;
    private float intialYpos;

    void Awake()
    {
        //maxTime = source.clip.length / detector.sampleWindow;
        Debug.Log("MaxTime: " + maxTime);
        intialYpos = Soundtrack[0].transform.position.y;
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
                // Reset pos after a few seconds:
                // ResetPos(YPos, loudness);
                float newYPos = YPos + loudness;
                if (newYPos > maxHeight)
                {
                    newYPos = maxHeight;
                }
                Soundtrack[index].transform.position =
                    Vector2.Lerp(
                        new Vector2(Soundtrack[index].transform.position.x, YPos),
                        new Vector2(Soundtrack[index].transform.position.x, newYPos),
                        loudness);

                RightSlide(index, loudness);
                LeftSlide(index, loudness);
                index++;

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
        if (index == 0)
        {
            int new_index = Soundtrack.Length - 1;
            Soundtrack[new_index].transform.position = Vector2.Lerp(
                            new Vector2(Soundtrack[new_index].transform.position.x, Yposition),
                            new Vector2(Soundtrack[new_index].transform.position.x, intialYpos),
                            loudness);
        }

        else
        {
            int new_index = index - 1;
            Soundtrack[new_index].transform.position = Vector2.Lerp(
                        new Vector2(Soundtrack[new_index].transform.position.x, Yposition),
                        new Vector2(Soundtrack[new_index].transform.position.x, intialYpos),
                        loudness);


        }

    }

    private void RightSlide(int StartIndex, float loudness)
    {
        float fraction = 1.0f;
        float HeightOfConcern = Soundtrack[StartIndex].transform.position.y;
        // Debug.Log("StartIndex: " + StartIndex);
        // Debug.Log("Height 0 : " + HeightOfConcern);
        // Debug.Log("Height 1 : " + HeightOfConcern * 1 / 2);
        // Debug.Log("Height 2 : " + HeightOfConcern * 1 / 3);
        // Debug.Log("Height 3 : " + HeightOfConcern * 1 / 4);

        for (int i = StartIndex + 1; i <= Soundtrack.Length - 1; i++)
        {
            var track = Soundtrack[i];
            track.transform.position = Vector2.Lerp(
                                     new Vector2(track.transform.position.x, track.transform.position.y),
                                     new Vector2(track.transform.position.x, HeightOfConcern * (1 / fraction)),
                                     loudness);
            fraction++;
            // Debug.Log("Fraction: " + fraction + " affecting the " + i + " index.");
        }
    }

    private void LeftSlide(int StartIndex, float loudness)
    {
        float fraction = 1.0f;
        float HeightOfConcern = Soundtrack[StartIndex].transform.position.y;
        // Debug.Log("StartIndex: " + StartIndex);
        // Debug.Log("Height 0 : " + HeightOfConcern);
        // Debug.Log("Height 1 : " + HeightOfConcern * 1 / 2);
        // Debug.Log("Height 2 : " + HeightOfConcern * 1 / 3);
        // Debug.Log("Height 3 : " + HeightOfConcern * 1 / 4);

        for (int i = StartIndex - 1; i >= 0; i--)
        {
            var track = Soundtrack[i];
            track.transform.position = Vector2.Lerp(
                                     new Vector2(track.transform.position.x, track.transform.position.y),
                                     new Vector2(track.transform.position.x, HeightOfConcern * (1 / fraction)),
                                     loudness);
            fraction++;
            // Debug.Log("Fraction: " + fraction + " affecting the " + i + " index.");
        }
    }
}
