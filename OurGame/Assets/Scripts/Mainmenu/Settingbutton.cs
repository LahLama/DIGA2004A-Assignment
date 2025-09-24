using UnityEngine;

public class Settingbutton : MonoBehaviour
{
    [SerializeField] GameObject SettingsTab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SettingsTab.SetActive(false);
    }

    public void SettingsButtonClicked()
    {
        SettingsTab.SetActive(true);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
