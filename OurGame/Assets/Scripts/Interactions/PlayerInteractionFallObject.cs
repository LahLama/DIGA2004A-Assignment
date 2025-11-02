using UnityEngine;

public class PlayerInteractionFallObject : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
     void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.GetComponent<interactionAnimation>().Interact ();
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
