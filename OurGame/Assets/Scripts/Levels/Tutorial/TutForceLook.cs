using UnityEngine;
using UnityEngine.UI;

public class TutForceLook : MonoBehaviour
{
    public DialougeState startDialougeScript;
    public Transform girlEyes;
    private Transform camera;
    void Awake()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").transform;

    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("HEY LOOOK AT ME");
        camera.LookAt(girlEyes);
        Vector3 euler = camera.rotation.eulerAngles;
        startDialougeScript.StartDialouge();
        camera.rotation = Quaternion.Euler(euler.x, euler.y, 0);
        //Set to default so player cant click on the girl again.
        girlEyes.parent.gameObject.layer = 0;
        this.gameObject.SetActive(false);
    }
}
