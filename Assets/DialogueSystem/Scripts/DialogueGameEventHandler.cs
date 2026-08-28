using UnityEngine;

public class DialogueGameEventHandler : MonoBehaviour
{
    protected virtual void Start()
    {
        DialogueManager.Instance.OnCustomDialogueEventTriggered.AddListener(dialogueHandler);
    }
    private void dialogueHandler()
    {
        dialogue_gameEvents();
    }
     public virtual void dialogue_gameEvents()
    {

    }






}