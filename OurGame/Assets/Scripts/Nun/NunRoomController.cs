using UnityEngine;
using System.Collections;

public class NunRoomController : MonoBehaviour
{
    [Header("Nun Movement Settings")]
    public Transform roomEnterPoint;
    public Transform roomExitPoint;
    public float moveSpeed = 2f;

    private bool hasVaseInteracted = false;
    private bool hasBookshelfInteracted = false;
    private bool isBusy = false;

    private void OnEnable()
    {
        // ✅ Subscribe to the new static event
        InteractableEvents.OnObjectInteracted += HandleObjectInteraction;
    }
     private void OnDisable()
    {
        // ✅ Always unsubscribe to avoid memory leaks
        InteractableEvents.OnObjectInteracted -= HandleObjectInteraction;
    }

    private void HandleObjectInteraction(string objectName)
    {
        if (objectName == "Vase")
            hasVaseInteracted = true;
        if (objectName == "Bookshelf")
            hasBookshelfInteracted = true;

        if (!isBusy && hasVaseInteracted && hasBookshelfInteracted)
        {
            StartCoroutine(NunRoomRoutine());
        }
    }
     private IEnumerator NunRoomRoutine()
    {
        isBusy = true;
        Debug.Log("Nun entering the room...");

        // Move to enter point
        yield return MoveToPoint(roomEnterPoint.position);

        Debug.Log("Nun staying in the room...");
        yield return new WaitForSeconds(5f);

        Debug.Log("Nun leaving the room...");
        yield return MoveToPoint(roomExitPoint.position);

        isBusy = false;
    }
  private IEnumerator MoveToPoint(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}