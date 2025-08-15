using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;


public class InnerDialouge : MonoBehaviour
{

    #region Varibles
    public GameObject panel;
    public TextMeshProUGUI text;

    #endregion




    public IEnumerator InnerDialogueContorl()
    {
        //Panel appears
        panel.GetComponent<Image>().CrossFadeAlpha(1f, 0.2f, true);
        yield return new WaitForSeconds(0.22f);
        panel.SetActive(true);

        //Panel Stays
        yield return new WaitForSeconds(2f);
        panel.GetComponent<Image>().CrossFadeAlpha(0f, 0.3f, true);

        //Panely dissapears
        yield return new WaitForSeconds(0.32f);
        panel.SetActive(false);

    }
}
