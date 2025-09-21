using UnityEngine;

public class VignetteControl : MonoBehaviour
{
    private float currentIntensity = 0f;
    private float HiddencurrentIntensity = 0f;
    private float targetIntensity = 0f;

    private float HiddentargetIntensity = 0f;

    public Material fullscreenEffectMaterial;
    public Material HideAwayVignetteMaterial;

    ///UN Dramatic Mode
    //https://docs.unity3d.com/2020.3/Documentation/ScriptReference/Material.SetFloat.html
    public void RemoveVignette(int lerpSpeed)
    {
        targetIntensity = 0f;
        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * lerpSpeed);
        fullscreenEffectMaterial.SetFloat("_FullscreenIntensity", currentIntensity);
    }
    //Dramatic Mode
    public void ApplyVignette(int lerpSpeed)
    {
        //Do beat per 0.25s to sync up with the controller vibrations
        targetIntensity = 1f;
        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * lerpSpeed);
        fullscreenEffectMaterial.SetFloat("_FullscreenIntensity", currentIntensity);
    }


    public void HiddenApplyVignette(float lerpSpeed)
    {
        HiddentargetIntensity = 1f;
        HideAwayVignetteMaterial.SetFloat("_FullscreenIntensity", HiddencurrentIntensity);
    }


    public void HiddenRemoveVignette(float lerpSpeed)
    {
        HiddentargetIntensity = 0f;
        HiddencurrentIntensity = Mathf.Lerp(HiddencurrentIntensity, HiddentargetIntensity, Time.deltaTime * lerpSpeed);
        HideAwayVignetteMaterial.SetFloat("_FullscreenIntensity", HiddencurrentIntensity);
    }
}
