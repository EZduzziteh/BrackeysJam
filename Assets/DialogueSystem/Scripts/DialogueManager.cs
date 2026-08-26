
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using static DialogueStage_Answer;

[RequireComponent(typeof(AudioSource))]
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public DialogueStage DEBUGDIALOGUESTAGE;
    DialogueStage currentDialogueStage;
    AudioSource aud;


    public UnityEvent OnDialogueAdvanced;
    public UnityEvent OnDialogueStarted;
    public UnityEvent OnDialogueEnded; 



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
        //bind events for debugging
        OnDialogueStarted.AddListener(() =>
        {
            Debug.Log("TESTING DIALOGUE EVENT DEBUG - OnDialogueStarted");
        });

        OnDialogueEnded.AddListener(() =>
        {

            Debug.Log("TESTING DIALOGUE EVENT DEBUG - OnDialogueEnded");
        });

        OnDialogueAdvanced.AddListener(() => {

            Debug.Log("TESTING DIALOGUE EVENT DEBUG - OnDialogueAdvanced");
        });


        if (DEBUGDIALOGUESTAGE != null)
        {
            StartDialogue(DEBUGDIALOGUESTAGE);
        }  
    }



  public void StartDialogue(DialogueStage stage)
  {
        bool newDialogueChain = false;
        if(currentDialogueStage == null)
        {
            Debug.Log("This must be a new dialogue");
            newDialogueChain = true;
        }
        currentDialogueStage = stage;
        ExecuteDialogue();

        if (newDialogueChain)
        {
            OnDialogueStarted?.Invoke();
        }
    }

  public void ExecuteDialogue()
  {
        if (currentDialogueStage != null)
        {
            DialogueStage_Line dialogueStageLine;
            try
            {
                dialogueStageLine = (DialogueStage_Line)currentDialogueStage;
                if (dialogueStageLine != null)
                {
                    UI_Dialogue_Container.Instance.DisplayLine(dialogueStageLine);
                    if (aud.clip)
                    {
                        aud.clip = dialogueStageLine.LineData.audioClip;
                        aud.Play();
                    }
                    return;
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
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        else
        {
            EndDialogue();
        }
  }

    private void EndDialogue()
    {
        currentDialogueStage = null;
        UI_Dialogue_Container.Instance.Clear();
        OnDialogueEnded?.Invoke();

    }

    bool awaitingUser = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (awaitingUser)
            {
                if (TryAdvanceDialogue())
                {
                    //there is a next dialogue
                }
                else
                {
                    // no next dialogue in chain, clear it up.
                    UI_Dialogue_Container.Instance.Clear();
                }
            }
            else
            {
               foreach(var f in FindObjectsByType<UI_Dialogue_Line_Panel>(FindObjectsSortMode.None))
                {
                    f.SkipText();
                }
            }
        }
        
    }

    public void SetAwaitingUser(bool isAwaiting)
    {
        awaitingUser = isAwaiting;
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
                    if (dialogueStageAnswer.answers.Count > option)
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
        if (answer.nextStage!= null)
        {
            StartDialogue(answer.nextStage);
            OnDialogueAdvanced?.Invoke();
        }
        else
        {
            EndDialogue();
        }
    }

   
}
