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
        if(!controller.skipTut)
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
                updateTransitionSpriteVisibility(true);
                watchForTransitions();
                break;
            case 3:
                //Hehehe line -- tunable timer/wait 15s. tut: teach player to look at grandma. start "interesting" dialogue
                watchForTransitions();
                updateTransitionSpriteVisibility(true);
                break;
            case 4:
                //"interesting" dialogue -- activate timer, wait 5-10s then start eject flow.
                controller.lockTransitions(true);
                startTimer(startEjectWaitTime);
                break;
            default:
                break;
        }

    }

    private void updateTransitionSpriteVisibility(bool enabledState=false)
    {
        foreach (interactable interactee in FindObjectsByType<interactable>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (interactee.effectName == interactable.interactableEffectType.transition)
                interactee.GetComponent<SpriteRenderer>().enabled = enabledState;
        }
    }

    public override void timerTriggered()
    {
        base.timerTriggered();
        switch (timerIndex)
        {
            case 1: //eject time
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
        controller.lockTransitions(false);
    }
    public override void onPlayerTransitionDelayed()
    {
        base.onPlayerTransitionDelayed();
        transitionIndex++;
        switch (transitionIndex)
        {
            case 1://start dialogue for grandma 05, after case 2 in main switch.
                //controller.lockTransitions(true);
                DialogueManager.Instance.StartDialogue(dialogueAfterHighlightTut);
                break;
            case 2://after case 3 in main switch, start calm talisman dialogue.
                DialogueManager.Instance.StartDialogue(dialogueCalmTalismanMoment);
                break;
        }
        
        
    }
    public override void onPlayerTransitionInstant()
    {
        updateTransitionSpriteVisibility(false);
    }

}
