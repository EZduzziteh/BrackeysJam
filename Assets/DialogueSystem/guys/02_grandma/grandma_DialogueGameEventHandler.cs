using UnityEngine;

public class grandma_DialogueGameEventHandler : DialogueGameEventHandler
{
    [SerializeField] private GameObject[] totem;
    [SerializeField] private DialogueStage dialogueAfterHighlightTut;
    [SerializeField] private DialogueStage dialogueCalmTalismanMoment;
    [SerializeField] private float lookWaitTime = 3.0f;
    [SerializeField] private float startEjectWaitTime = 3.0f;

    protected override void Start()
    {
        base.Start();
        showTotem(false);
    }
    private void showTotem(bool enabled)
    {
        foreach (GameObject go in totem)
            {
            go.SetActive(enabled);
            }
    }
    public override void dialogue_gameEvents()
    {
        print("executing dialogue based game event" + DialogueManager.Instance.currentDialogueStage.customEventStepID);
        //intro & Grandma.
        switch (DialogueManager.Instance.currentDialogueStage.customEventStepID)
        {
            case 1:
                //radio ends - silly spawn (at the end of dialogue.
                DialogueManager.Instance.OnDialogueEnded.AddListener(delaySilhouetteSpawn);
                break;
            case 2:
                //...looking at me line - activate highlight swapper. after clicking this, it triggers next line (05_grandma)
                showTotem(true);
                //activate transition highlight.
                watchForTransitions();
                break;
            case 3:
                //Hehehe line -- tunable timer/wait 15s. tut: force player to look at grandma. start "interesting" dialogue
                startTimer(lookWaitTime); // wait 15 eventually.
                controller.startNewTransition(default, car_interior_controller.transitionGameEffect.moveScene);
                break;
            case 4:
                //"interesting" dialogue -- activate timer, wait 5-10s then start eject flow.
                startTimer(startEjectWaitTime);
                break;
            default:
                break;
        }

    }
    public override void timerTriggered()
    {
        base.timerTriggered();
        timerIndex++;
        switch (timerIndex)
        {
            case 1: //after first wait, "force look at grandma"
                DialogueManager.Instance.StartDialogue(dialogueCalmTalismanMoment);
                break;
            case 2: //eject time
                seat.startBootDialogue();
                this.enabled = false;
                break;
            default:
                break;
        }
    }
    private void delaySilhouetteSpawn()
    {
        DialogueManager.Instance.OnDialogueEnded.RemoveListener(delaySilhouetteSpawn);
        seat.stepPassenger();
        seat.lockTransitions(false);
    }
    public override void onPlayerTransition()
    {//start dialogue for grandma 05, after case 2.
        base.onPlayerTransition();
        seat.lockTransitions(true);
        DialogueManager.Instance.StartDialogue(dialogueAfterHighlightTut);
    }

}
