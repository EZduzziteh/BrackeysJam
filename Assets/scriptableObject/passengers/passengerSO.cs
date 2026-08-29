using UnityEngine;

[CreateAssetMenu(fileName = "passengerSO", menuName = "Scriptable Objects/passengerSO")]
public class passengerSO : ScriptableObject
{
    public Sprite faceSprite;
    public Sprite insideSeatSprite;
    public DialogueCoreBundle dialogueBundle;
    //public DialogueGameEventHandler dialogueEventScript; doesnt work right, will fix later ;)
}
