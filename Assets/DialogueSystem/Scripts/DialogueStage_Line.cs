using UnityEngine;

[CreateAssetMenu(fileName = "DialogueStage_Line", menuName = "Scriptable Objects/DialogueStage_Line")]
public class DialogueStage_Line : DialogueStage
{
    public DialogueStage NextStage;
    public DialogueLineData LineData;
    public bool AutoAdvance = true;
    public float AutoAdvanceDelay = 3.0f;
}
