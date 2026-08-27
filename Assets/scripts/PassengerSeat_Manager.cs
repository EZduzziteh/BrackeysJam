using UnityEngine;

public class PassengerSeat_Manager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject passenger;
    [SerializeField] private GameObject[] insideModel;
    [SerializeField] private GameObject silhouetteModel;
    [SerializeField] private int sortingOrderShift=3;
    private GameObject debugFakeNextPassenger;
    private bool seatOccupied = false;
    car_interior_controller controller;
    //stats
    [SerializeField] private int totalPassengersCollected;
    [SerializeField] private float passengerTimer;
    [SerializeField] private float passengerEventTimer;
    [SerializeField] private float silhouetteEventTimer;
    [SerializeField] private float silhouetteMaxDuration=3f;

    //dialogue
    private DialogueManager DM;
    [SerializeField] private DialogueCoreBundle dialogueBundle;

    private void Start()
    {
        controller=  FindFirstObjectByType<car_interior_controller>();
        DM = FindFirstObjectByType<DialogueManager>();
    }
    private void Update()
    {
        if(seatOccupied)
        {
            passengerTimer += Time.deltaTime;
            passengerEventTimer += Time.deltaTime;
            if (passengerEventTimer >= 30f)
            {
                print("event triggered");
                if(dialogueBundle.bootDialogue)
                {
                    DM.StartDialogue(dialogueBundle.bootDialogue);
                    lockTransitions(true);
                    DM.OnDialogueEnded.AddListener(ejectListener);
                }   
                passengerEventTimer = 0f;
            }
        }
        if (needDiscovery())
        {
            silhouetteEventTimer += Time.deltaTime;
            if (silhouetteEventTimer >= silhouetteMaxDuration)
            {
                controller.startNewTransition(car_interior_controller.transitionType.full_blink, car_interior_controller.transitionGameEffect.moveScene);
            }
        }
    }
    private void LoadPassenger()
    {
        if (passenger.activeSelf)
        {
            //create next passenger for later.
            debugFakeNextPassenger = Instantiate(passenger);
            debugFakeNextPassenger.SetActive(false);
            debugFakeNextPassenger.GetComponent<jimmy_face_swapper>().selectNewFace();

            //move passenger into car
            SpriteRenderer[] sprites = passenger.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer childrenSR in sprites)
            {
                childrenSR.sortingOrder += sortingOrderShift;
            }
            passenger.transform.position = gameObject.transform.position;
            foreach (GameObject model in insideModel)
            {
                model.SetActive(true);
            }

            //system updates
            silhouetteEventTimer = 0f; // cleanup
            totalPassengersCollected++;
            seatOccupied = true;

            //dialogue stuff
            if (dialogueBundle.boardedDialogue)
                DM.StartDialogue(dialogueBundle.boardedDialogue);
        }
        else
            checkForDiscovery();
    }
    private void ejectPassenger()
    {
        //cleanup passenger
        Destroy(passenger);
        foreach (GameObject model in insideModel)
        {
            model.SetActive(false);
        }
        seatOccupied = false;
        passengerTimer = 0f;
        passengerEventTimer = 0f;

        //setup for next passenger -> transition into silhouette
        passenger = debugFakeNextPassenger;
        silhouetteModel.SetActive(true);
        controller.silentTransition();
    }
    public void checkForDiscovery()
    {
        if (needDiscovery())
        {
            passenger.SetActive(true);
            silhouetteModel.SetActive(false);
            if (dialogueBundle.windowDialogue)
            {
                DM.StartDialogue(dialogueBundle.windowDialogue);
                DM.OnDialogueEnded.AddListener(loadListener);
            }
        }
    }
    private bool needDiscovery()
    {
        return !seatOccupied & !passenger.activeSelf;
    }
    private void loadListener() { DM.OnDialogueEnded.RemoveListener(loadListener); controller.startNewTransition(car_interior_controller.transitionType.wide_blink, car_interior_controller.transitionGameEffect.passengerCutscene); }

    private void ejectListener()
    { 
        DM.OnDialogueEnded.RemoveListener(ejectListener);
        print(DM.GetLastSelectedOption());
        lockTransitions(false);
        switch (DM.GetLastSelectedOption())
        {
            case 1:
                controller.startNewTransition(car_interior_controller.transitionType.wide_blink, car_interior_controller.transitionGameEffect.passengerCutscene);
                break;
            default:
                break;
        }  
    }

    public void stepPassenger()
    {
        if (seatOccupied)
          ejectPassenger();
        else
          LoadPassenger();    
    }
    private void lockTransitions(bool shouldLock)
    {
        foreach (interactable interactee in FindObjectsByType<interactable>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (interactee.effectName == interactable.interactableEffectType.transition)
                interactee.gameObject.SetActive(!shouldLock);
        }
    }
}
