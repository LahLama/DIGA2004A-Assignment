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

    private GameObject player;
    private Interactor _interactor;
    private GameObject holdingContainer;
    private Scrollbar _hideSlider;
    private bool _isplayerHidden;
    private float _hideDuration;
    #endregion


    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        holdingContainer = GameObject.FindWithTag("HoldingPos");
        _hideSlider = GameObject.FindWithTag("HideSlider").GetComponent<Scrollbar>();
        _interactor = GameObject.FindWithTag("MainCamera").GetComponent<Interactor>();
    }

    void Update()
    {
        _hideDuration = _interactor.hideDuration;
        if (_isplayerHidden)
        {

            _hideDuration -= Time.deltaTime;
            // Debug.Log("++" + (int)_sprintTimer);
            _hideSlider.size = 1 - (_hideDuration / 4);
        }
    }

    public void HidePlayer()
    {

        Debug.Log("THIS IS THE NAME OF THE BOX: " + this.name);


        transform.GetChild(0).gameObject.GetComponent<Camera>().enabled = true;
        player.gameObject.GetComponent<MeshRenderer>().enabled = false;
        player.gameObject.GetComponent<PlayerMovement>().enabled = false;
        player.gameObject.GetComponent<LookFunction>().enabled = false;

        if (holdingContainer.activeSelf) { holdingContainer.SetActive(false); }

        _isplayerHidden = true;


    }

    public void ShowPlayer()
    {
        _hideSlider.size = 0;
        transform.GetChild(0).gameObject.GetComponent<Camera>().enabled = false;
        player.gameObject.GetComponent<MeshRenderer>().enabled = true;
        player.gameObject.GetComponent<PlayerMovement>().enabled = true;
        player.gameObject.GetComponent<LookFunction>().enabled = true;
        if (!holdingContainer.activeSelf) { holdingContainer.SetActive(true); }
        _isplayerHidden = false;

    }
}
