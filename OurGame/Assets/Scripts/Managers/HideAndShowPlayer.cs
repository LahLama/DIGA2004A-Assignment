using System.Runtime.InteropServices;
using Mono.Cecil.Cil;
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

    private GameObject _player; // reference to the player object
    private GameObject _holdingContainer; // where held items sit when visible
    private Interactor _interactor; // handles player interaction logic
    private Scrollbar _hideSlider; // UI display of hide progress
    private bool _isplayerHidden; // tracks hidden state internally (not currently used)
    private float _hideDuration; // countdown for hide duration bar
    private GameObject sprintBar; // reference to stamina UI bar
    private GameObject handsDisplay; // players hand model/display
    private CanvasGroup canvasSprintGroup; // controls visibility of sprint UI

    private GameObject playerParts; // visual model of player to show/hide
    private VignetteControl vignetteControl; // screen effect for hiding
    Vector3 playerOGpos;

    #endregion


    #region UnityFunctions
    void Awake()
    {
        // Cache important objects and components once at start
        _player = GameObject.FindWithTag("Player");
        _holdingContainer = GameObject.FindWithTag("HoldingPos");
        _hideSlider = GameObject.FindWithTag("HideSlider").GetComponent<Scrollbar>();
        _interactor = GameObject.FindAnyObjectByType<Interactor>();
        vignetteControl = GameObject.FindAnyObjectByType<VignetteControl>();
        handsDisplay = GameObject.FindWithTag("Hands");
        playerParts = GameObject.FindWithTag("PlayerParts");

        // Hide bar starts invisible
        CanvasGroup canvasGroup = _hideSlider.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        // Sprint bar UI reference
        sprintBar = GameObject.FindWithTag("SprintSlider");
        canvasSprintGroup = sprintBar.GetComponent<CanvasGroup>();
    }

    void Update()
    {
        // Update hide duration timer display while hiding
        _hideDuration = _interactor.hideDuration;

        if (_interactor._PlayerIsHidden)
        {
            // Reduce remaining hide time and update slider UI fill
            _hideDuration -= Time.deltaTime;
            _hideSlider.size = 1 - (_hideDuration / 4);

            // Apply hiding vignette effect
            vignetteControl.HiddenApplyVignette(0.5f);
        }
        else if (!_interactor._PlayerIsHidden)
        {
            // Remove vignette smoothly when not hiding
            vignetteControl.HiddenRemoveVignette(2);
        }

        // Handles fading of UI display
        HandleHideBarAppearing();
    }

    #endregion

    public void HidePlayer(Collider HidePlace)
    {
        // Mark player hidden so enemies cannot detect them
        _interactor._PlayerIsHidden = true;
        playerOGpos = _player.transform.position;

        _player.gameObject.GetComponent<PlayerMovement>().enabled = false;

        _player.GetComponent<CharacterController>().enabled = false;
        _player.transform.position = HidePlace.transform.position;


        // Hide held item visually if any

        // Hide sprint UI
        canvasSprintGroup.alpha = 1f;

        // Move player to new layer for AI masking while hidden
        _player.layer = LayerMask.NameToLayer("hidePlacesMask");




    }

    public void ShowPlayer()
    {
        // Mark player visible again
        _interactor._PlayerIsHidden = false;
        // Reset hide bar UI
        _hideSlider.size = 0;
        // Bring back visual model and player controls
        _player.gameObject.GetComponent<PlayerMovement>().enabled = true;
        _player.GetComponent<CharacterController>().enabled = true;
        _player.transform.position = playerOGpos;
        canvasSprintGroup.alpha = 0f;
        // Set player back to regular layer
        _player.layer = LayerMask.NameToLayer("Player");

        // Mark player hidden so enemies cannot detect them
        _interactor._PlayerIsHidden = true;
    }


    void HandleHideBarAppearing()
    {
        // Smooth fade in UI if hiding
        if (_interactor._PlayerIsHidden)
        {
            CanvasGroup canvasGroup = _hideSlider.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = _hideSlider.gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, Time.deltaTime / 0.2f);
        }

        // Fade out UI once bar is near empty when not hidden
        else if (_hideSlider.size < 0.05)
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
