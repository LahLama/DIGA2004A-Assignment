using UnityEngine;
//https://www.youtube.com/watch?v=dzD0qP8viLw
public class AudioLoudnessDetection : MonoBehaviour
{

    public int sampleWindow = 32;
    public float[] AverageSamples;
    private AudioClip MicrophoneClip;

    void Start()
    {
        MicrophoneToAudioClip();
    }

    public void MicrophoneToAudioClip()
    {
        //get microphone
        string microphoneName = Microphone.devices[0];
        //The microphone , looping,  length of clip, frequency
        MicrophoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }

    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), MicrophoneClip);
    }

    public float GetLoudnessFromAudioClip(int clipPositon, AudioClip clip)
    {
        //We seperate the clip into sample segments. 
        //Data before clip position
        int startPosition = clipPositon - sampleWindow;
        if (startPosition < 0) return 0;
        float[] waveData = new float[sampleWindow];
        AverageSamples = new float[sampleWindow];


        //takes two parameters:
        // array for the data
        // postion to start getting data
        clip.GetData(waveData, startPosition);
        // clip.GetData(visualSegments, startPosition);


        //Get loudness

        //Abs Mean Value
        float totalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++)
        {

            //segement == -1 <--> 1
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        //gives ups the average loadness per segement
        return totalLoudness / sampleWindow;

    }


}
