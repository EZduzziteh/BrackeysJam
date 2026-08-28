using UnityEngine;

public class grandma_DialogueGameEventHandler : DialogueGameEventHandler
{
    [SerializeField] private GameObject[] totem;

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
        //intro & Grandma.
        switch (DialogueManager.Instance.currentDialogueStage.customEventStepID)
        {
            case 1:
                //radio ends - silly spawn (at the end of dialogue.
                DialogueManager.Instance.OnDialogueEnded.AddListener(delaySilhouetteSpawn);
                break;
            case 2:
                showTotem(true);
                //...looking at me line - activate highlight swapper. after clicking this, it triggers next line (05_grandma)
                break;
            case 3:
                //Hehehe line -- tunable timer/wait 15s.
                //tut: force player to look at grandma. start "interesting" dialogue.
                break;
            case 4:
                //"interesting" dialogue -- activate timer, wait 5-10s
                //start eject logic. (move oh honey to boot dialogue) startBootDialogue()
                break;
            default:
                break;
        }

    }

    private void delaySilhouetteSpawn()
    {
        DialogueManager.Instance.OnDialogueEnded.RemoveListener(delaySilhouetteSpawn);
        PassengerSeat_Manager seat = FindFirstObjectByType<PassengerSeat_Manager>();
        seat.stepPassenger();
        seat.lockTransitions(false);
    }
}
