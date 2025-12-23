using UnityEngine;

public class TrackTeleport : MonoBehaviour
{

    public GameObject BeltStart;

    void OnTriggerExit2D(Collider2D collider)
    {
        Debug.Log("Name: " + collider);
        collider.transform.position = new Vector2(BeltStart.transform.position.x, 0);
    }


}
