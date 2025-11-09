using UnityEngine;

public class PaperClickHandler : MonoBehaviour
{
    public GameObject popupPanel; // The panel to show when clicked
    public GameObject collectedPanel; // The panel to show when clicked
    public GameObject paperHolder;

    public bool hasBeenPicked = false;
    private Transform player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }
    void Update()
    {

        if (this.transform.IsChildOf(player) && !hasBeenPicked)
        {
            hasBeenPicked = true;

            // Show the popup panel
            popupPanel.SetActive(true);

            // Hide the paper in the world
            this.GetComponent<MeshRenderer>().enabled = false;

            Invoke("ShowCollectedPaper", 2);
        }
    }


    void ShowCollectedPaper()
    {
        popupPanel.SetActive(false);
        collectedPanel.SetActive(true);
        gameObject.transform.SetParent(paperHolder.transform);


    }

}