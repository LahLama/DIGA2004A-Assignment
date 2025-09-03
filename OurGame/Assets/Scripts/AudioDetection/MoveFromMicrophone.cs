using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
//https://www.youtube.com/watch?v=dzD0qP8viLw


public class MoveFromMicrophone : MonoBehaviour
{
    public float sensibility = 100;
    public float threshold = 0.3f;
    public AudioLoudnessDetection detector;
    private Slider slider;
    [SerializeField] double _loudRange;
    Image _barColorbg;
    void Awake()
    {
        slider = GetComponent<Slider>();
        _barColorbg = this.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();

        _loudRange = (slider.maxValue - threshold) * (60.0f / 100.0f);
        slider.value = 0;
    }



    void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * sensibility;

        if (loudness > threshold)
            slider.value = loudness - threshold;

        if (slider.value > _loudRange)
            _barColorbg.color = Color.red;
        else
            _barColorbg.color = Color.blue;




    }
}
