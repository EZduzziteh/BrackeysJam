using UnityEngine;

[CreateAssetMenu(fileName = "DialogueCoreBundle", menuName = "Scriptable Objects/DialogueCoreBundle")]
public class DialogueCoreBundle : ScriptableObject
{
    public DialogueStage windowDialogue;
    public DialogueStage boardedDialogue;
    public DialogueStage bootDialogue;
    public DialogueStage finalDialogue;

}
