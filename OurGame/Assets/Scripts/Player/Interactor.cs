using UnityEngine;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{

    // /https://www.youtube.com/watch?v=cUf7FnNqv7U

    public LayerMask interactionsMask;
    public Image CanInteractToolTip;
    void Start()
    {

    }

    void Update()
    {

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 3.5f, interactionsMask))
        {
            Debug.Log("hit something");
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.green);
            CanInteractToolTip.color = new Color(1f, 1, 1f, 1f);

        }

        else
        {
            Debug.Log("NULL");
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5f, Color.red);
            CanInteractToolTip.color = new Color(1f, 1, 1f, 0.5f);
        }




    }

}
