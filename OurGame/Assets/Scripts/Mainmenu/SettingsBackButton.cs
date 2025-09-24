using UnityEngine;

public class SettingsBackButton : MonoBehaviour
{
    [SerializeField] GameObject SettingsTab;

    public void BackButtonClicked()
    {
        SettingsTab.SetActive(false);
    }
}
