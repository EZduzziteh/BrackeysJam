
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueAnswerSO", menuName = "Scriptable Objects/DialogueAnswerSO")]
public class DialogueStage_Answer : DialogueStage
{
    public List<DialogueAnswer> answers = new List<DialogueAnswer>();

    [System.Serializable]
    public struct DialogueAnswer
    {
        public DialogueLineData LineData;
        public DialogueStage nextStage;
    }
}
