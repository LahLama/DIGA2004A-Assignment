using System.Runtime.CompilerServices;
using Unity.Burst.Intrinsics;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;

public class HideAndShowPlayer : MonoBehaviour
{
    #region Varibles

    private GameObject player;
    public Interactor interactor;
    GameObject holdingContainer;
    private bool _isplayerHidden;
    private Scrollbar _hideSlider;
    private float _hideDuration;
    #endregion


    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        holdingContainer = GameObject.FindWithTag("HoldingPos");
        _hideSlider = GameObject.FindWithTag("HideSlider").GetComponent<Scrollbar>();
    }

    void Update()
    {
        _hideDuration = interactor.hideDuration;
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
