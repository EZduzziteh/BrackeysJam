
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using static DialogueStage_Answer;

[RequireComponent(typeof(AudioSource))]
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public DialogueStage introDialogue; //renamed debug value to be used for intro kick-off.
    public DialogueStage currentDialogueStage { get; private set; }
    public DialogueStage lastEndDialogueStage { get; private set; }
    AudioSource aud;

    public UnityEvent OnDialogueAdvanced;
    public UnityEvent OnDialogueStarted;
    public UnityEvent OnDialogueEnded;
    public UnityEvent OnCustomDialogueEventTriggered;
    public UnityEvent OnCustomDialogueEndEventTriggered;
    private int lastSelectedOption;

    bool awaitingUser = false;
    bool AwaitingAutoAdvance = false;
    float AwaitingAutoAdvanceTimer = 0.0f;

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

        OnCustomDialogueEventTriggered.AddListener(() =>
        {
            Debug.Log("TESTING DIALOGUE EVENT DEBUG - OnCustomFireEventOnAdvanceDialogue");
        });

        OnDialogueEnded.AddListener(() =>
        {
            Debug.Log("TESTING DIALOGUE EVENT DEBUG - OnDialogueEnded");
        });

        OnDialogueAdvanced.AddListener(() => {

            Debug.Log("TESTING DIALOGUE EVENT DEBUG - OnDialogueAdvanced");
        });

        if (introDialogue != null)
        {
            if(FindFirstObjectByType<car_interior_controller>().skipTut)
            {
                //skip intro if we're skipping the tutorial.
                Destroy(GetComponent<grandma_DialogueGameEventHandler>());
            }
            else
            {
                StartDialogue(introDialogue);
            }
        }
    }

    private void Update()
    {
        if (AwaitingAutoAdvance)
        {
            AwaitingAutoAdvanceTimer += Time.deltaTime;
            //Debug.Log("Awaiting Auto Advance - " + AwaitingAutoAdvanceTimer);
            DialogueStage_Line lineStage;
            lineStage = (DialogueStage_Line)currentDialogueStage;

            if (lineStage)
            {
                if (AwaitingAutoAdvanceTimer >= lineStage.AutoAdvanceDelay)
                {
                    TryAdvanceDialogue();
                    AwaitingAutoAdvance = false;

                    //Early return so we dont also check the player input on the same frame? not sure if this will impact anything.
                    return;
                }
            }
        }
        

        if (Input.anyKeyDown)
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
                foreach (var f in FindObjectsByType<UI_Dialogue_Line_Panel>(FindObjectsSortMode.None))
                {
                    f.SkipText();
                }
            }

            AwaitingAutoAdvance = false;
        }
    }

    public void HandleTypingComplete()
    {
        Debug.Log("Typing Complete!");

        //start wait timer
        AwaitingAutoAdvance = true;
        AwaitingAutoAdvanceTimer = 0.0f;
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
        if (currentDialogueStage.fireCustomEventBeforeDialogueEnd)
        {
            OnCustomDialogueEndEventTriggered?.Invoke();
        }
        lastEndDialogueStage = currentDialogueStage;
        currentDialogueStage = null;
        UI_Dialogue_Container.Instance.Clear();
        OnDialogueEnded?.Invoke();

    }


    public void SetAwaitingUser(bool isAwaiting)
    {
        awaitingUser = isAwaiting;
    }


    public bool TryAdvanceDialogue()
    {
        //default to 0 if no option specified
        return TryAdvanceDialogue(0);
    }
    public bool TryAdvanceDialogue(int option)
    {

        
        DialogueStage_Line dialogueStageLine;

        try
        {
            dialogueStageLine = (DialogueStage_Line)currentDialogueStage;
            AdvanceDialogue(dialogueStageLine);
            lastSelectedOption = option;
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
                        lastSelectedOption = option;
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

    public int GetLastSelectedOption()
    {
        return lastSelectedOption;
    }
    public void AdvanceDialogue(DialogueStage_Line line)
    {
        AdvanceDialogueStage(line.NextStage);
    }

    public void AdvanceDialogue(DialogueAnswer answer)
    {
        AdvanceDialogueStage(answer.nextStage);
    }

    public void AdvanceDialogueStage(DialogueStage targetStage)
    {
        if (targetStage != null)
        {
            StartDialogue(targetStage);
            OnDialogueAdvanced?.Invoke();
            if (currentDialogueStage.fireCustomEventOnAdvance)
            {
                OnCustomDialogueEventTriggered?.Invoke();
            }
        }
        else
        {
            EndDialogue();
        }
    }

   
}
