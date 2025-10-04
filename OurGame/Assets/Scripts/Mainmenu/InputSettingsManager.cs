using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class InputSettingsManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Dropdown inputDropdown;      // Reference to the Dropdown
    public GameObject controllerPanel;       // Panel for Controller settings
    public GameObject keyboardPanel;         // Panel for Keyboard settings
    public GameObject mousePanel;            // Panel for Mouse settings

    void Start()
    {
        // Add listener to handle dropdown changes
        inputDropdown.onValueChanged.AddListener(OnInputChanged);

        // Initialize correct panel based on default dropdown value
        OnInputChanged(inputDropdown.value);
    }

    // Called when dropdown value changes
    public void OnInputChanged(int index)
    {
        controllerPanel.SetActive(index == 0); // Show ControllerPanel if selected
        keyboardPanel.SetActive(index == 1);   // Show KeyboardPanel if selected
        mousePanel.SetActive(index == 2);      // Show MousePanel if selected
    }
}