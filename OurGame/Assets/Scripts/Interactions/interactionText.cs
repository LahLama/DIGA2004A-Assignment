using UnityEngine;

public class text_interaction : MonoBehaviour, IInteractables
{

    private InnerDialouge innerDialouge;

    public string MyWords;
    private void Awake()
    {
        innerDialouge = GameObject.FindWithTag("MainCamera").GetComponent<InnerDialouge>();
    }
    public void Interact()
    {
        innerDialouge.text.text = MyWords;
    }
}
