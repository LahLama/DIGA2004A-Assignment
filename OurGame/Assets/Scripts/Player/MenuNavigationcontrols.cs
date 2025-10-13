using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MenuNavigationcontrols : MonoBehaviour
{
    [Header("UI References")]
    public GameObject firstSelectedButton; // Assign in Inspector
    public Button[] menuButtons; // Optional: for manual navigation setup

    [Header("Input Actions")]
    public InputActionAsset inputActions; // Assign your .inputactions asset
    private InputAction navigateAction;
    private InputAction submitAction;
    private InputAction cancelAction;



    private void OnEnable()
    {
        // Get actions from the asset
        navigateAction = inputActions.FindAction("Navigate");
        submitAction = inputActions.FindAction("Submit");
        cancelAction = inputActions.FindAction("Cancel");

        // Enable actions
        navigateAction.Enable();
        submitAction.Enable();
        cancelAction.Enable();

        // Subscribe to input events
        submitAction.performed += OnSubmit;
        cancelAction.performed += OnCancel;
    }

    private void OnDisable()
    {
        // Unsubscribe and disable actions
        submitAction.performed -= OnSubmit;
        cancelAction.performed -= OnCancel;

        navigateAction.Disable();
        submitAction.Disable();
        cancelAction.Disable();
    }


    private void Start()
    {
        // Set initial focus
        if (firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        // Invoke the currently selected button
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        if (selected != null && selected.TryGetComponent(out Button button))
        {
            button.onClick.Invoke();
        }
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        // Optional: handle back/cancel logic
        Debug.Log("Cancel pressed - implement back navigation here.");
    }


}
