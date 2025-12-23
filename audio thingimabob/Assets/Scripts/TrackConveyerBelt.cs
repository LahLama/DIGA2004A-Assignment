using UnityEngine;

public class TrackConveyerBelt : MonoBehaviour
{
    public GameObject[] soundtrack;
    public float moveAmount = 0.1f;

    void Update()
    {
        foreach (var item in soundtrack)
        {
            item.transform.position -= new Vector3(moveAmount, 0, 0);
        }
    }



}
