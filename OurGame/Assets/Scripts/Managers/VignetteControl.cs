using UnityEngine;

public class VignetteControl : MonoBehaviour
{
    // Main vignette current intensity
    private float currentIntensity;
    // Hidden vignette current intensity
    private float HiddencurrentIntensity = 0f;

    // Main vignette target intensity
    private float targetIntensity = 0f;
    // Hidden vignette target intensity
    private float HiddentargetIntensity = 0f;

    // Material used for full screen vignette effect
    public Material fullscreenEffectMaterial;
    // Material used while hidden in a hiding spot
    public Material HideAwayVignetteMaterial;

    /// Removes the normal vignette effect (lerps intensity down)
    public void RemoveVignette(int lerpSpeed)
    {
        targetIntensity = 0f; // Target is off
        // Smoothly reduce vignette over time
        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * lerpSpeed);
        // Send intensity value to shader
        fullscreenEffectMaterial.SetFloat("_FullscreenIntensity", currentIntensity);
    }

    /// Applies the normal vignette effect (lerps intensity up)
    public void ApplyVignette(int lerpSpeed)
    {
        targetIntensity = 1f; // Target is full vignette
        // Smoothly increase vignette over time
        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * lerpSpeed);
        // Send intensity value to shader
        fullscreenEffectMaterial.SetFloat("_FullscreenIntensity", currentIntensity);
    }

    /// Applies the hidden vignette effect (no lerp yet, just sets shader value)
    public void HiddenApplyVignette(float lerpSpeed)
    {
        HiddentargetIntensity = 1f; // Target is full hidden vignette
        // Sets current hidden vignette value but does not lerp it yet
        HideAwayVignetteMaterial.SetFloat("_FullscreenIntensity", HiddencurrentIntensity);
    }

    /// Removes the hidden vignette effect (lerps intensity down)
    public void HiddenRemoveVignette(float lerpSpeed)
    {
        HiddentargetIntensity = 0f; // Target is off
        // Smoothly reduce hidden vignette over time
        HiddencurrentIntensity = Mathf.Lerp(HiddencurrentIntensity, HiddentargetIntensity, Time.deltaTime * lerpSpeed);
        // Send intensity value to shader
        HideAwayVignetteMaterial.SetFloat("_FullscreenIntensity", HiddencurrentIntensity);
    }
}
