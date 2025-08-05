using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEditor.ShaderGraph;

public class Interactor : MonoBehaviour
{

    // /https://www.youtube.com/watch?v=cUf7FnNqv7U

    public LayerMask interactionsMask;
    public Image CanInteractToolTip;
    public GameObject CanInteractText;
    public GameObject innerDialougePanel;
    private RaycastHit _hitInfo;
    private bool _interactionInput;
    bool _ray;
    void Update()
    {

        HandleInteractions();
    }

    public void OnInteractions(InputAction.CallbackContext context)
    {
        _interactionInput = context.ReadValueAsButton();
    }


    void HandleInteractions()
    {
        HandleTooltip();
        _ray = Physics.Raycast(transform.position, transform.forward, out _hitInfo, 3.5f, interactionsMask);


        if (_interactionInput)
        {

            if (_ray)
            {
                Debug.Log("hit something");
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * _hitInfo.distance, Color.green);
                StartCoroutine(InnerDialogueContorl());

            }

            else
            {
                Debug.Log("NULL");
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5f, Color.red);
            }
        }






    }


    private void HandleTooltip()
    {
        if (!_ray)
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
