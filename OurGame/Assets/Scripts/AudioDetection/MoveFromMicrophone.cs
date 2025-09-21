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
    private NunAi enemyAI;
    private float NunlookTime;
    public AudioLoudnessDetection detector;
    private Slider slider;
    [SerializeField] double _loudRange;
    Image _barColorbg;
    public bool isLoud = false;
    void Awake()
    {
        slider = GetComponent<Slider>();
        _barColorbg = this.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();

        _loudRange = (slider.maxValue - threshold) * (60.0f / 100.0f);
        slider.value = 0;
        enemyAI = GameObject.FindGameObjectWithTag("NunEnemy").GetComponent<NunAi>();
    }



    void Update()
    {
        NunlookTime = enemyAI.NunlookTime;
        loudness = detector.GetLoudnessFromMicrophone() * sensibility;

        if (loudness > threshold)
            loudness = loudness - threshold;

        slider.value = Mathf.Lerp(loudness, Prevloudness, 0.5f);


        if (slider.value > _loudRange)
        {
            _barColorbg.color = Color.red;
            isLoud = true;
            StartCoroutine(Cooldown(NunlookTime));
        }
        else
        {
            _barColorbg.color = Color.blue;

        }

        Prevloudness = loudness;

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
