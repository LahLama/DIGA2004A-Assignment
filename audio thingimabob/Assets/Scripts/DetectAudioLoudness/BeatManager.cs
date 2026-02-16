using UnityEngine;
using UnityEngine.Events;

public class BeatManager : MonoBehaviour
{
    [SerializeField] private float _bpm;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Intervals[] _intervals;

    private void Update()
    {
        foreach (Intervals interval in _intervals) //gets time currently elapsed divided by intervals
        {
            float sampledTime = (_audioSource.timeSamples / (_audioSource.clip.frequency * interval.GetInvervalLength(_bpm)));
            interval.CheckForNewInterval(sampledTime); //sends the info the func to check if we HAVE crossed a new beat
        }
    }
}

[System.Serializable]
public class Intervals
{
    [SerializeField] private float _steps;
    [SerializeField] private UnityEvent _trigger;
    private int _lastIntveral;

    public float GetInvervalLength(float bpm) //gets length of current beat
    {
        return 60f / (bpm * _steps); //gives sec in a beat * steps (1/2 beat, 1/4 beat etc)
    }

    public void CheckForNewInterval(float interval)
    {
        if (Mathf.FloorToInt(interval) != _lastIntveral)
        {
            _lastIntveral = Mathf.FloorToInt(interval);
            _trigger.Invoke();
        }
        else
        {

        }
    }
}



