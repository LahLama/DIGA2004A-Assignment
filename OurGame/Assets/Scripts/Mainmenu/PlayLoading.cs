using UnityEngine;

public class PlayLoading : MonoBehaviour
{   
    [SerializeField] string sceneName;

    [SerializeField] CanvasGroup LoadingScreen;
    [SerializeField] CanvasGroup LoadingText;

    [SerializeField] float TimeToFade = 1f;
    private bool fadein = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadingText.alpha = 0f;
        LoadingScreen.alpha = 0f;
    }

    public void PlayButtonClicked()
    {
     fadein = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadein == true)
        {
            LoadingScreen.alpha += TimeToFade * Time.deltaTime;
            LoadingText.alpha += TimeToFade * Time.deltaTime;
            Invoke(nameof(ChangeScenes), 4f);
            if (LoadingScreen.alpha >=1f && LoadingText.alpha >= 1f)
            {
                fadein = false;
            }
          
           
        }
    }

    void ChangeScenes()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
