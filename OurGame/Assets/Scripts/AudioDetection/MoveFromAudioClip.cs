using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=dzD0qP8viLw
public class MoveFromAudioClip : MonoBehaviour
{
    public AudioSource soruce;
    public float minHeight;
    public float maxHeight;
    public float sensibility = 100;
    public float threshold = 0.1f;
    public float maxTime = 2;
    public AudioLoudnessDetection detector;
    public List<GameObject> Soundtrack;
    private float itemTimer = 0f;
    private int index;


    void Update()
    {
        float loudness = detector.GetLoudnessFromAudioClip(soruce.timeSamples, soruce.clip) * sensibility;
        itemTimer += Time.deltaTime;
        if (loudness > threshold && itemTimer > maxTime)
        {


            Soundtrack[index].transform.position =
                Vector3.Lerp(
                    new Vector3(Soundtrack[index].transform.position.x, minHeight, Soundtrack[index].transform.position.z),
                    new Vector3(Soundtrack[index].transform.position.x, maxHeight, Soundtrack[index].transform.position.z),
                    loudness);

            index++;
            if (index >= Soundtrack.Count)
                index = 0;



            itemTimer = 0;
            return;
        }
    }
}
