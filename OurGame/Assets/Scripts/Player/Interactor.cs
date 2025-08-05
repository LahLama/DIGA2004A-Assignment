using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEditor.ShaderGraph;
using Unity.VisualScripting;

public class Interactor : MonoBehaviour
{

    //Drop": Look in pickUp parent, look a children tha matches current enabled one on the player, if yes: diabble On player, set pos of 'drop" to be infront(only 1 mili, cause Out of bounds things) of player then enable dropped item

    // /https://www.youtube.com/watch?v=cUf7FnNqv7U
    //https://youtu.be/zEfahR66Pa8

    public LayerMask interactionsMask;
    public LayerMask pickUpMask;
    public Image CanInteractToolTip;
    public GameObject CanInteractText;
    public GameObject innerDialougePanel;
    public RaycastHit hitInfo;
    private bool _interactionInput;
    bool _isGenericObject;
    bool _isPickUpObject;

    private void Start() { DisableAllOnPlayerItems(); }
    void Update() { HandleInteractions(); }

    public void OnInteractions(InputAction.CallbackContext context) { _interactionInput = context.ReadValueAsButton(); }


    void HandleInteractions()
    {
        HandleTooltip();
        _isGenericObject = Physics.Raycast(transform.position, transform.forward, out hitInfo, 3.5f, interactionsMask);
        _isPickUpObject = Physics.Raycast(transform.position, transform.forward, out hitInfo, 3.5f, pickUpMask);


        if (_interactionInput)
        {

            if (_isGenericObject)
            {
                Debug.Log("hit _isGenericObject");
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.green);
                StartCoroutine(InnerDialogueContorl());

            }

            else if (_isPickUpObject)
            {
                Debug.Log("hit _isPickUpObject");
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.blue);

                CheckAndSetPickUpObj();

            }

            else
            {
                Debug.Log("NULL");
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5f, Color.red);
            }
        }






    }

    private void CheckAndSetPickUpObj()
    {

        GameObject PickUpObj = hitInfo.collider.gameObject;
        Debug.Log("Name is: " + PickUpObj.name);

        transform.GetChild(0);

        foreach (Transform child in transform.GetChild(0))
        {
            if (PickUpObj.name == child.name)
            {
                PickUpObj.SetActive(false);
                child.gameObject.SetActive(true);
            }
        }
    }


    private void DisableAllOnPlayerItems()
    {
        foreach (Transform child in transform.GetChild(0))
        {
            child.gameObject.SetActive(false);
        }
    }



    private void HandleTooltip()
    {
        if (!_isGenericObject || !_isPickUpObject)
        {
            //Nothing to interact with 
            CanInteractToolTip.color = new Color(1f, 1, 1f, 0.5f);
            CanInteractText.SetActive(false);

        }
        else
        {
            //Can interact with something
            CanInteractToolTip.color = new Color(1f, 1, 1f, 1f);
            CanInteractText.SetActive(true);

        }
    }



    private IEnumerator InnerDialogueContorl()
    {
        innerDialougePanel.GetComponent<Image>().CrossFadeAlpha(1f, 0.2f, true);
        yield return new WaitForSeconds(0.22f);
        innerDialougePanel.SetActive(true);

        yield return new WaitForSeconds(2f);
        innerDialougePanel.GetComponent<Image>().CrossFadeAlpha(0f, 0.3f, true);

        yield return new WaitForSeconds(0.32f);
        innerDialougePanel.SetActive(false);

    }






}
