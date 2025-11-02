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
        // Panel appears
        panel.GetComponent<Image>().CrossFadeAlpha(1f, 0.2f, true);
        yield return new WaitForSeconds(0.22f);
        panel.SetActive(true);

        // Panel stays
        yield return new WaitForSeconds(2f);

        // Don't disappear while hitObj is true
        while (FindAnyObjectByType<Interactor>().hitObj)
        {
            yield return null; // wait until next frame and re-check
        }

        // Panel fades out once hitObj is false
        panel.GetComponent<Image>().CrossFadeAlpha(0f, 0.3f, true);

        // Panel disappears
        yield return new WaitForSeconds(0.32f);
        panel.SetActive(false);
    }
}
