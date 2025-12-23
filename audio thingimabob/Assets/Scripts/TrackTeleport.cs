using UnityEngine;

public class TrackTeleport : MonoBehaviour
{
    public GameObject[] soundtrack;
    public GameObject BeltStart;

    void OnCollisionExit2D(Collision2D collision)
    {
        collision.collider.transform.position = new Vector2(BeltStart.transform.position.x, 0);
    }


}
