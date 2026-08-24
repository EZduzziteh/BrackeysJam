
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueAnswerSO", menuName = "Scriptable Objects/DialogueAnswerSO")]
public partial class DialogueStage_Answer : DialogueStage
{
    public List<DialogueAnswer> answers = new List<DialogueAnswer>();
}
