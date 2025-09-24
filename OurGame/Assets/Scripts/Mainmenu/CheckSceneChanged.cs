using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class NewMonoBehaviourScript : MonoBehaviour
{   
    private string currentSceneName;
    [SerializeField] GameObject LoadingImage;

    public Image fadeImage;
    public float textFadeSpeed = 1.0f;

    public TextMeshProUGUI textToFade;
    public float fadeSpeed = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Color textColor = textToFade.color;
        textColor.a = 1.0f;
        textToFade.color = textColor;
        currentSceneName = SceneManager.GetActiveScene().name;
        fadeImage.canvasRenderer.SetAlpha(1f); 
        FadeOutEffect();
    }

    // Update is called once per frame
     private void Update()
    {
        if (currentSceneName == SceneManager.GetActiveScene().name)
        {
             Invoke(nameof(FadeOutEffect), 0.1f);
        }
    }

    private void FadeOutEffect()
    {
        fadeImage.CrossFadeAlpha(0, fadeSpeed, false); 
        textToFade.CrossFadeAlpha(0, fadeSpeed, false);
    }

}
