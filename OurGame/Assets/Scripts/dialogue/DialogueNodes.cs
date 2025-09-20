using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "Dialogue/Node")]
public class DialogueNodeSO : ScriptableObject
{
    [TextArea(2, 5)]
    public string dialogueLine;

    public DialogueChoice[] choices;
}

