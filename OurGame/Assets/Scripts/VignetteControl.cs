using UnityEngine;

public class VignetteControl : MonoBehaviour
{
    private float currentIntensity = 0f;
    private float targetIntensity = 0f;
    public float lerpSpeed = 2f; // Adjust for faster/slower transitions

    public Material fullscreenEffectMaterial;

    ///UN Dramatic Mode
    //https://docs.unity3d.com/2020.3/Documentation/ScriptReference/Material.SetFloat.html
    public void RemoveVignette()
    {
        targetIntensity = 0f;
        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * lerpSpeed);
        fullscreenEffectMaterial.SetFloat("_FullscreenIntensity", currentIntensity);
    }
    //Dramatic Mode
    public void ApplyVignette()
    {
        targetIntensity = 1f;
        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * lerpSpeed);
        fullscreenEffectMaterial.SetFloat("_FullscreenIntensity", currentIntensity);
    }
}
