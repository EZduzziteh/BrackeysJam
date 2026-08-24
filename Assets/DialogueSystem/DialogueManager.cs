
using System;
using UnityEngine;
using static DialogueStage_Answer;

[RequireComponent(typeof(AudioSource))]
public class DialogueManager : MonoBehaviour
{

  public DialogueStage currentDialogueStage;
    AudioSource aud;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (currentDialogueStage != null)
        {
            StartDialogue(currentDialogueStage);
        }  
    }



    public void StartDialogue(DialogueStage stage)
  {
        currentDialogueStage = stage;
        ExecuteDialogue();
  }

  public void ExecuteDialogue()
  {
      //  Debug.Log("Executing Dialogue...");

        if (currentDialogueStage != null)
        {

            DialogueStage_Line dialogueStageLine;
            try
            {
                dialogueStageLine = (DialogueStage_Line)currentDialogueStage;
                if (dialogueStageLine != null)
                {
                    Debug.Log(dialogueStageLine.LineData.text);
                    if (dialogueStageLine.NextStage)
                    {
                        AdvanceDialogue(dialogueStageLine);
                        return;
                    }
                }

            } catch (Exception e) { 
            
            }

            DialogueStage_Answer dialogueStageAnswer;
            try {
                dialogueStageAnswer = (DialogueStage_Answer)currentDialogueStage;
                if (dialogueStageAnswer != null)
                {

                    string optionString = "";
                    foreach (var o in dialogueStageAnswer.answers)
                    {
                        optionString += o.LineData.text + ", ";
                        Debug.Log(o.LineData.text);
                    }

                    optionString.Substring(0, optionString.Length - 2); //trims off the trailing ,

                    Debug.Log("Please Answer, Options: ");

                    int randomAnswer = UnityEngine.Random.Range(0, dialogueStageAnswer.answers.Count);


                    Debug.LogWarning("Answer: " + dialogueStageAnswer.answers[randomAnswer].LineData.text);



                    AdvanceDialogue(dialogueStageAnswer.answers[randomAnswer]);
                    return;
                    //Prompt user to enter their answer
                }
            }
            catch(Exception e)
            {

            }

        }
        else
        {
            Debug.Log("No Dialogue to execute!");
            EndDialogue();
        }

  }

    private void EndDialogue()
    {
        Debug.Log("Ending Dialogue!");
        currentDialogueStage = null;
    }

    public void AdvanceDialogue(DialogueStage_Line line)
    {
       // Debug.Log("Advancing...");
        if (line.NextStage != null)
        {
            StartDialogue(line.NextStage);
        }
        else
        {
            EndDialogue();
        }
    }

    public void AdvanceDialogue(DialogueAnswer answer)
    {
        Debug.Log("Advancing...");
        if (answer.nextStage!= null)
        {
            StartDialogue(answer.nextStage);
        }
        else
        {
            EndDialogue();
        }
    }


}
