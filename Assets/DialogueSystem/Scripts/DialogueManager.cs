
using System;
using UnityEngine;
using static DialogueStage_Answer;

[RequireComponent(typeof(AudioSource))]
public class DialogueManager : MonoBehaviour
{

    public static DialogueManager Instance;

  public DialogueStage currentDialogueStage;
    AudioSource aud;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
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
                    UI_Dialogue_Container.Instance.DisplayLine(dialogueStageLine);

                }

            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            DialogueStage_Answer dialogueStageAnswer;
            try {
                dialogueStageAnswer = (DialogueStage_Answer)currentDialogueStage;
                if (dialogueStageAnswer != null)
                {

                    UI_Dialogue_Container.Instance.DisplayAnswer(dialogueStageAnswer);
                   
                    return;
                    //Prompt user to enter their answer
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
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



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log(TryAdvanceDialogue());
        }
    }

    public bool TryAdvanceDialogue(int option = 0)
    {
        DialogueStage_Line dialogueStageLine;

        try
        {
            dialogueStageLine = (DialogueStage_Line)currentDialogueStage;
            AdvanceDialogue(dialogueStageLine);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        DialogueStage_Answer dialogueStageAnswer;
        try
        {
            dialogueStageAnswer = (DialogueStage_Answer)currentDialogueStage;
            if (dialogueStageAnswer != null)
            {
                if(dialogueStageAnswer.answers.Count > option)
                {
                    AdvanceDialogue(dialogueStageAnswer.answers[option]);

                    return true;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return false;
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
