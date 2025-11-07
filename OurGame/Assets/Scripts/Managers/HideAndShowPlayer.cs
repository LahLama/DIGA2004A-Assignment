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

    public void HidePlayer()
    {
        // Mark player hidden so enemies cannot detect them
        _interactor._PlayerIsHidden = true;

        // Enable hiding camera from hiding spot object
        transform.GetChild(0).gameObject.GetComponent<Camera>().enabled = true;

        // Hide player models and disable movement/looking
        playerParts.SetActive(false);
        _player.gameObject.GetComponent<PlayerMovement>().enabled = false;
        _player.gameObject.GetComponent<LookFunction>().enabled = false;

        // Hide hand visuals
        handsDisplay.gameObject.SetActive(false);

        // Hide held item visually if any
        if (_holdingContainer.activeSelf) { _holdingContainer.SetActive(false); }

        // Hide sprint UI
        canvasSprintGroup.alpha = 0f;

        // Move player to new layer for AI masking while hidden
        _player.layer = LayerMask.NameToLayer("hidePlacesMask");

        SoundManager.Instance.StopLooping("SprintStep");
        SoundManager.Instance.StopLooping("WalkStep");

    }

    public void ShowPlayer()
    {
        // Mark player visible again
        _interactor._PlayerIsHidden = false;

        // Reset hide bar UI
        _hideSlider.size = 0;

        // Turn off hiding camera
        transform.GetChild(0).gameObject.GetComponent<Camera>().enabled = false;

        // Bring back visual model and player controls
        playerParts.SetActive(true);
        _player.gameObject.GetComponent<PlayerMovement>().enabled = true;
        _player.gameObject.GetComponent<LookFunction>().enabled = true;

        // Show hands again
        handsDisplay.gameObject.SetActive(true);

        // Restore item visibility if player is holding something
        if (!_holdingContainer.activeSelf) { _holdingContainer.SetActive(true); }

        // Set player back to regular layer
        _player.layer = LayerMask.NameToLayer("Player");
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
