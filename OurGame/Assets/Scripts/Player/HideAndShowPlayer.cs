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
    #endregion


    #region UnityFunctions
    void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _holdingContainer = GameObject.FindWithTag("HoldingPos");
        _hideSlider = GameObject.FindWithTag("HideSlider").GetComponent<Scrollbar>();
        _interactor = GameObject.FindWithTag("MainCamera").GetComponent<Interactor>();

    }

    void Update()
    {
        // Updates the hide timer
        _hideDuration = _interactor.hideDuration;
        if (_isplayerHidden)
        {
            _hideDuration -= Time.deltaTime;
            _hideSlider.size = 1 - (_hideDuration / 4);
        }
    }

    #endregion

    public void HidePlayer()
    {
        //Enables hiding spot camera
        //Disables player visually
        transform.GetChild(0).gameObject.GetComponent<Camera>().enabled = true;
        _player.gameObject.GetComponent<MeshRenderer>().enabled = false;
        _player.gameObject.GetComponent<PlayerMovement>().enabled = false;
        _player.gameObject.GetComponent<LookFunction>().enabled = false;

        if (_holdingContainer.activeSelf) { _holdingContainer.SetActive(false); }

        _isplayerHidden = true;

        _player.layer = LayerMask.NameToLayer("hidePlacesMask");


    }

    public void ShowPlayer()
    {
        //Disables hiding spot camera
        //Enables player visually
        _hideSlider.size = 0;
        transform.GetChild(0).gameObject.GetComponent<Camera>().enabled = false;
        _player.gameObject.GetComponent<MeshRenderer>().enabled = true;
        _player.gameObject.GetComponent<PlayerMovement>().enabled = true;
        _player.gameObject.GetComponent<LookFunction>().enabled = true;
        if (!_holdingContainer.activeSelf) { _holdingContainer.SetActive(true); }
        _isplayerHidden = false;

        _player.layer = LayerMask.NameToLayer("Player");

    }
}
