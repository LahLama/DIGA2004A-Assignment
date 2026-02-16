using System.Runtime.Serialization;
using UnityEngine;

public class BeatScale : MonoBehaviour
{

    [SerializeField] float _pulseSize = 1.15f;

    [SerializeField] float _returnSpeed = 5f;
    private Vector3 _startSize;
    void Start()
    {
        _startSize = transform.localScale;
    }

    public void Pulse()
    {
        transform.localScale = _startSize * _pulseSize;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, _startSize, Time.deltaTime * _returnSpeed);
    }
}
