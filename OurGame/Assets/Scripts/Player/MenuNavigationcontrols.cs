using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MenuNavigationcontrols : MonoBehaviour
{
    [Header("UI References")]
    public GameObject firstSelectedButton; // Assign in Inspector
    public Button[] menuButtons; // Optional: for manual navigation setup
    public GameObject OptionsPanel; // Reference to the options panel
    private Transform player;

    [Header("Input Actions")]
    public InputActionAsset inputActions; // Assign your .inputactions asset
    private InputAction navigateAction;
    private InputAction submitAction;
    private InputAction cancelAction;
    private InputAction MenuAction;
    private InputAction selectAction;
    private InputAction startAction;

    public GameObject optionsPanelFirstButton;



    public void OnEnable()
    {
          player = GameObject.FindGameObjectWithTag("Player").transform;
        // Get actions from the asset
        navigateAction = inputActions.FindAction("Navigate");
        submitAction = inputActions.FindAction("Submit");
        cancelAction = inputActions.FindAction("Cancel");
        MenuAction = inputActions.FindAction("Menu");
        selectAction = inputActions.FindAction("Select");
        startAction = inputActions.FindAction("Start");

        // Enable actions
        navigateAction.Enable();
        submitAction.Enable();
        cancelAction.Enable();
        MenuAction.Enable();
        selectAction.Enable();
        startAction.Enable();

        // Subscribe to input events
        submitAction.performed += OnSubmit;
        cancelAction.performed += OnCancel;
        MenuAction.performed += OnMenu;
        selectAction.performed += OnSelect;
        //startAction.performed += OnStart;

    }

    public void OnDisable()
    {
        // Unsubscribe and disable actions
        submitAction.performed -= OnSubmit;
        cancelAction.performed -= OnCancel;
        MenuAction.performed -= OnMenu;
        selectAction.performed -= OnSelect;
        startAction.performed -= OnSelect;

        navigateAction.Disable();
        submitAction.Disable();
        cancelAction.Disable();
        MenuAction.Disable();
        selectAction.Disable();
        startAction.Disable();
    }


    public void Start()
    {
        OnEnable();
        // Set initial focus
        if (firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    

    public void OnSelect(InputAction.CallbackContext context)
    {

        // Optional: handle selection logic if needed
         GameObject selected = EventSystem.current.currentSelectedGameObject;

         if (selected == null && firstSelectedButton != null)
    {
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        return;
    }

    // highlight or animate selection
    Debug.Log($"Selected: {selected?.name}");

        if (selected != null && selected.TryGetComponent(out Button button))
        {
            button.onClick.Invoke();
        }
        Debug.Log("Select pressed - implement selection logic here.");
    }

    public void OnSubmit(InputAction.CallbackContext context)
{
    GameObject selected = EventSystem.current.currentSelectedGameObject;

    if (selected == null) return;

    string selectedName = selected.name.ToLower();

    switch (selectedName)
    {
        case "settings":
            if (OptionsPanel != null)
            {
                OptionsPanel.SetActive(true);
                player.GetComponent<PlayerMovement>().enabled = false;
                player.GetComponent<LookFunction>().enabled = false;
                EventSystem.current.SetSelectedGameObject(firstSelectedButton); // Optional: focus inside panel
            }
            break;

        case "quit":
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu"); // Replace with your actual scene name
            break;

        case "close":
            if (OptionsPanel != null)
            {
                OptionsPanel.SetActive(false);
                player.GetComponent<PlayerMovement>().enabled = true;
                player.GetComponent<LookFunction>().enabled = true;
            }
            break;

        default:
            // Fallback: invoke button normally
            if (selected.TryGetComponent(out Button button))
            {
                button.onClick.Invoke();
            }
            break;
    }

    Debug.Log($"Submitted: {selected.name}");
}

    public void OnCancel(InputAction.CallbackContext context)
    {
        // Optional: handle back/cancel logic
        Debug.Log("Cancel pressed - implement back navigation here.");
        if (OptionsPanel != null)
        {
            OptionsPanel.SetActive(!OptionsPanel.activeSelf);
    
            player.GetComponent<PlayerMovement>().enabled = !player.GetComponent<PlayerMovement>().enabled;
            player.GetComponent<LookFunction>().enabled = !player.GetComponent<LookFunction>().enabled;

            // Optionally set focus back to a main menu button
            // Example: EventSystem.current.SetSelectedGameObject(mainMenuFirstButton);
        }
        else
        {
            Debug.LogWarning("Options panel reference is missing.");
        }
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        Debug.Log("Open Menu pressed - opening options panel.");

        // Assuming you have a reference to your options panel GameObject
        // Add this public field to your class:
        // public GameObject optionsPanel;
        Debug.Log("Opening Options...");
        
        if (OptionsPanel != null)
        {
            OptionsPanel.SetActive(true);
             player.GetComponent<PlayerMovement>().enabled = false;
             player.GetComponent<LookFunction>().enabled = false;

            // Optionally set focus to a button in the options panel
            if (optionsPanelFirstButton != null)
            {
                EventSystem.current.SetSelectedGameObject(optionsPanelFirstButton);
            }

        }
        else
        {
            Debug.LogWarning("Options panel reference is missing.");
        }
    
    
        Debug.Log("Open Menu pressed - implement menu opening logic here.");
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        if (selected != null && selected.TryGetComponent(out Button button))
        {
            button.onClick.Invoke();

        }
        // Optional: handle opening the menu
        Debug.Log("Open Menu pressed - implement menu opening logic here.");
    }

   /*public void OnNavigate(InputAction.CallbackContext context)
    {
        Vector2 navigation = context.ReadValue<Vector2>();

        if (navigation.y > 0)
        {
            // Move up
            MoveSelection(-1);
        }
        else if (navigation.y < 0)
        {
            // Move down
            MoveSelection(1);
        }
    }*/

}
