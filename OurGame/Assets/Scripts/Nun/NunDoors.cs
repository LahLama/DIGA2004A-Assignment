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

            if (hitObj.gameObject.transform.parent.parent.TryGetComponent<Animator>(out Animator animator))


                if (animator != null)
                {
                    animator.SetTrigger("DoorOpen");

                    return;
                }
                else
                {
                    print("no animator");
                }

        }
    }



    IEnumerator ActivateDoorAfterDelay(GameObject hitObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        hitObj.SetActive(true);
    }
}
