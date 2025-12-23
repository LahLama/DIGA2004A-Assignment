using UnityEngine;

public class TrackTeleport : MonoBehaviour
{

    public GameObject BeltStart;

    float ExitYpos = 0;


    void OnTriggerEnter2D(Collider2D collider)
    {
        collider.transform.position = new Vector2(collider.transform.position.x, ExitYpos);
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        ExitYpos = collider.transform.position.y;
        //store exit y pos as = Old Pos
        Debug.Log("Name: " + collider.name);
        collider.transform.position = new Vector2(BeltStart.transform.position.x, 0);
    }


}
