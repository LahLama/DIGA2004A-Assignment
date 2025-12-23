using System.Collections.Generic;
using UnityEngine;

public class TrackConveyerBelt : MonoBehaviour
{
    public Transform SoundtrackHolder;
    public List<Transform> Soundtrack;
    public float moveAmount = 0.1f;


    void Awake()
    {

    }
    void Update()
    {
        foreach (var item in Soundtrack)
        {
            item.transform.position -= new Vector3(moveAmount, 0, 0);

        }
    }



}
