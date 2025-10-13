using UnityEngine;
using System.Collections;

public class NunDoors : MonoBehaviour
{

    public LayerMask DoorLayer;
    public void DoorInteractions()
    {
        Vector3 sphereCenter = transform.position + transform.forward * 0.5f;
        float radius = 0.5f;
        Collider[] hits = Physics.OverlapSphere(sphereCenter, radius, DoorLayer);
        foreach (Collider col in hits)
        {
            GameObject hitObj = col.gameObject;
            hitObj.SetActive(false);
            StartCoroutine(ActivateDoorAfterDelay(hitObj, 2f));
        }
    }



    IEnumerator ActivateDoorAfterDelay(GameObject hitObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        hitObj.SetActive(true);
    }
}
