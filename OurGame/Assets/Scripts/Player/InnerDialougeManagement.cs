using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class InnerDialouge : MonoBehaviour
{

    public GameObject innerDialougePanel;
    public IEnumerator InnerDialogueContorl()
    {
        innerDialougePanel.GetComponent<Image>().CrossFadeAlpha(1f, 0.2f, true);
        yield return new WaitForSeconds(0.22f);
        innerDialougePanel.SetActive(true);

        yield return new WaitForSeconds(2f);
        innerDialougePanel.GetComponent<Image>().CrossFadeAlpha(0f, 0.3f, true);

        yield return new WaitForSeconds(0.32f);
        innerDialougePanel.SetActive(false);

    }
}
