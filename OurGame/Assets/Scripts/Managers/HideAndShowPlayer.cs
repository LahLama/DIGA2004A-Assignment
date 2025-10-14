using UnityEngine;
using UnityEngine.UI;

public class HideAndShowPlayer : MonoBehaviour
{
    /*
    Title: Creating a Horror Game in Unity - Part 6: Hiding System (JavaScript)
    Author: SpeedTutor
    Date:  Jun 10, 2014
    Availability: https://youtu.be/GTtW57u_cfg?si=NqfWBUk5yLfmXfn-
    */

    #region Varibles

    private GameObject _player;
    private GameObject _holdingContainer;
    private Interactor _interactor;
    private Scrollbar _hideSlider;
    private bool _isplayerHidden;
    private float _hideDuration;

    private VignetteControl vignetteControl;



    #endregion


    #region UnityFunctions
    void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _holdingContainer = GameObject.FindWithTag("HoldingPos");
        _hideSlider = GameObject.FindWithTag("HideSlider").GetComponent<Scrollbar>();
        _interactor = GameObject.FindAnyObjectByType<Interactor>();
        vignetteControl = GameObject.FindAnyObjectByType<VignetteControl>();

        CanvasGroup canvasGroup = _hideSlider.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    void Update()
    {

        // Updates the hide timer
        _hideDuration = _interactor.hideDuration;
        if (_interactor._PlayerIsHidden)
        {
            _hideDuration -= Time.deltaTime;
            _hideSlider.size = 1 - (_hideDuration / 4);
            vignetteControl.HiddenApplyVignette(0.5f);
        }
        else if (!_interactor._PlayerIsHidden)
        {
            vignetteControl.HiddenRemoveVignette(2);
        }

        HandleHideBarAppearing();
    }

    #endregion

    public void HidePlayer()
    {
        _interactor._PlayerIsHidden = true;
        //Enables hiding spot camera
        //Disables player visually
        transform.GetChild(0).gameObject.GetComponent<Camera>().enabled = true;
        _player.gameObject.GetComponent<MeshRenderer>().enabled = false;
        _player.gameObject.GetComponent<PlayerMovement>().enabled = false;
        _player.gameObject.GetComponent<LookFunction>().enabled = false;

        if (_holdingContainer.activeSelf) { _holdingContainer.SetActive(false); }


        _player.layer = LayerMask.NameToLayer("hidePlacesMask");




    }

    public void ShowPlayer()
    {

        _interactor._PlayerIsHidden = false;
        //Disables hiding spot camera
        //Enables player visually
        _hideSlider.size = 0;
        transform.GetChild(0).gameObject.GetComponent<Camera>().enabled = false;
        _player.gameObject.GetComponent<MeshRenderer>().enabled = true;
        _player.gameObject.GetComponent<PlayerMovement>().enabled = true;
        _player.gameObject.GetComponent<LookFunction>().enabled = true;
        if (!_holdingContainer.activeSelf) { _holdingContainer.SetActive(true); }



        _player.layer = LayerMask.NameToLayer("Player");

    }


    void HandleHideBarAppearing()
    {
        if (_interactor._PlayerIsHidden)
        {
            CanvasGroup canvasGroup = _hideSlider.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = _hideSlider.gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, Time.deltaTime / 0.2f);
        }

        else
        {
            CanvasGroup canvasGroup = _hideSlider.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = _hideSlider.gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, Time.deltaTime / 0.7f);
        }
    }
}
