using UnityEngine;
using System.Collections;

public class NunDoors : MonoBehaviour
{
    public LayerMask DoorLayer; // Layer to filter which objects are considered doors

    public void DoorInteractions()
    {
        // Define the centre of a small sphere in front of the nun
        Vector3 sphereCenter = transform.position + transform.forward * 0.5f;
        float radius = 0.5f;

        // Detect all colliders within the sphere that are on the DoorLayer
        Collider[] hits = Physics.OverlapSphere(sphereCenter, radius, DoorLayer);

        foreach (Collider col in hits)
        {
            GameObject hitObj = col.gameObject;

            // Attempt to get an Animator component from the grandparent of the hit object
            if (hitObj.gameObject.transform.parent.parent.TryGetComponent<Animator>(out Animator animator))

                if (animator != null)
                {
                    // Trigger the door opening animation
                    animator.SetTrigger("DoorOpen");
                    return; // Exit after opening the first door
                }
                else
                {
                    // Warn if no animator found
                    print("no animator");
                }
        }
    }

    IEnumerator ActivateDoorAfterDelay(GameObject hitObj, float delay)
    {
        // Wait for a specified delay before activating the door object
        yield return new WaitForSeconds(delay);
        hitObj.SetActive(true);
    }
}
