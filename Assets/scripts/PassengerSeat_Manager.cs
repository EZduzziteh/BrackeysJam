using UnityEngine;

public class PassengerSeat_Manager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject passenger;
    [SerializeField] private GameObject[] insideModel;
    [SerializeField] private GameObject silhouetteModel;
    //[SerializeField] private int sortingOrderShift=3;
    private GameObject debugFakeNextPassenger;
    private bool seatOccupied = false;
    car_interior_controller controller;
    //stats
    [SerializeField] private int totalPassengersCollected;
    [SerializeField] private float passengerTimer;
    [SerializeField] private float passengerEventTimer;
    [SerializeField] private float silhouetteEventTimer;
    [SerializeField] private float silhouetteMaxDuration=3f;
    [SerializeField] private float defaultPassengerStressRate = 20.0f;

    //dialogue
    private DialogueManager DM;
    [SerializeField] private DialogueCoreBundle dialogueBundle;
    [SerializeField] private DialogueCoreBundle rogueBundle;
    [SerializeField] private bool shouldTriggerBootEvent = false;

    //audio
    [SerializeField] private AudioSource doorCloseSFX;
    [SerializeField] private AudioSource doorOpenSFX;


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

            if(!DM.currentDialogueStage)
                passengerEventTimer += Time.deltaTime;
            
            if (passengerEventTimer >= 30f)
            {
                print("event triggered");
                passengerEventTimer = 0f;
                if(shouldTriggerBootEvent && DM.currentDialogueStage != dialogueBundle.bootDialogue)
                {
                    startBootDialogue();
                }
            }
        }
        if (needDiscovery())
        {
            silhouetteEventTimer += Time.deltaTime;
            if (silhouetteEventTimer >= silhouetteMaxDuration)
            {
                silhouetteEventTimer = 0f;
                controller.startNewTransition(car_interior_controller.transitionType.full_blink, car_interior_controller.transitionGameEffect.moveScene);
                FindFirstObjectByType<CarSpeedSystem>().StopDriving(); //This triggers the car to stop driving immediately and also sets candrive to false.
            }
        }
    }
    public void stepPassenger()
    {
        if (seatOccupied)
            ejectPassenger();
        else
        {
            if (silhouetteModel.activeSelf || passenger.activeSelf)
                LoadPassenger();
            else
                silhouetteModel.SetActive(true);
        }
    }
    private bool needDiscovery()
    {
        return !seatOccupied & !passenger.activeSelf & silhouetteModel.activeSelf;
    }

    public void checkForDiscovery()
    {
        print("attempting discover");
        if (needDiscovery())
        {
            controller.checkExpectedScene(false);
            print("Discovering Passenger");
            passenger.SetActive(true);
            silhouetteModel.SetActive(false);
            silhouetteEventTimer = 0f; // cleanup
            if (dialogueBundle.windowDialogue)
            {
                DM.StartDialogue(dialogueBundle.windowDialogue);
                DM.OnDialogueEnded.AddListener(loadListener);
                controller.lockTransitions(true); // lock transition until after boarded dialogue ends..
            }
        }
        else
        {//activate silhouette 
            if (!silhouetteModel.activeSelf & !seatOccupied & !passenger.activeSelf)
                silhouetteModel.SetActive(true);
        }
    }

    private void loadListener()
    {
        DM.OnDialogueEnded.RemoveListener(loadListener);
        if (doorOpenSFX) //play door open sound.
            doorOpenSFX.Play();
        controller.startNewTransition(car_interior_controller.transitionType.wide_blink, car_interior_controller.transitionGameEffect.passengerCutscene);
        //controller.lockTransitions(false); idk if lock/unlock should be here.
    }
    private void LoadPassenger()
    {
        if (passenger.activeSelf)
        {
            print("Loading Passenger");
            //move passenger into car


            foreach (GameObject model in insideModel)
            {
                model.SetActive(true);
                if (model.GetComponent<SpriteStateSwapper>())
                    model.GetComponent<SpriteStateSwapper>().spriteIndex = passenger.GetComponent<jimmy_face_swapper>().selectedFace;
            }

            if (passenger.GetComponent<jimmy_face_swapper>().selectedFace>0 || controller.skipTut)
            { 
                FindFirstObjectByType<StressSystem>().AddStressor(new StressSource()
                {
                    amount = defaultPassengerStressRate,
                    gameObjectReference = this.gameObject
                });
            }

            //update passenger for next time.
            passenger.SetActive(false);
            passenger.GetComponent<jimmy_face_swapper>().selectNewFace();

            //system updates
            totalPassengersCollected++;
            seatOccupied = true;

            //dialogue stuff
            if (dialogueBundle.boardedDialogue)
            { 
                DM.StartDialogue(dialogueBundle.boardedDialogue);
                DM.OnDialogueEnded.AddListener(enableTransitionAfterBoardDialogue); //allow transition after boarded dialogue ends.
            }

        }
        else
            checkForDiscovery();
    }
    private void enableTransitionAfterBoardDialogue()
    {
        DM.OnDialogueEnded.RemoveListener(enableTransitionAfterBoardDialogue);
        controller.lockTransitions(false);
    }
    
    public void startBootDialogue()
    {
        controller.checkExpectedScene(false);
        if (dialogueBundle.bootDialogue)
        {
            DM.StartDialogue(dialogueBundle.bootDialogue);
            controller.lockTransitions(true);
            DM.OnDialogueEnded.AddListener(ejectListener);
        }
        else
            ejectListener();
    }

    private void ejectListener()
    { 
        DM.OnDialogueEnded.RemoveListener(ejectListener);
        controller.startNewTransition(car_interior_controller.transitionType.wide_blink, car_interior_controller.transitionGameEffect.passengerCutscene);
        controller.lockTransitions(false);
        StressSystem stressSystem = FindFirstObjectByType<StressSystem>();
        stressSystem.RemoveStressor(stressSystem.IndexOfGameObjectStressor(gameObject));
        if (doorCloseSFX) //play door close sound.
            doorCloseSFX.Play();
        /* used for only select answer to eject. probably can be refactored into dialogue event handler if needed.
        print(DM.GetLastSelectedOption());
        switch (DM.GetLastSelectedOption())
        {
            case 1:
                controller.startNewTransition(car_interior_controller.transitionType.wide_blink, car_interior_controller.transitionGameEffect.passengerCutscene);
                break;
            default:
                break;
        } 
        */
    }
    private void ejectPassenger()
    {
        //send stats off.
        StatTracker.Instance.totalPassengerTime += passengerTimer;
        StatTracker.Instance.totalPassengersCollected = totalPassengersCollected;

        //cleanup passenger
        foreach (GameObject model in insideModel)
        {
            model.SetActive(false);
        }
        seatOccupied = false;
        passengerTimer = 0f;
        passengerEventTimer = 0f;

        //play final dialogue
        try
        {
            DialogueManager.Instance.StartDialogue(dialogueBundle.finalDialogue);
            DialogueManager.Instance.OnDialogueEnded.AddListener(setupForNextPassenger);
        }
        catch
        {
            print("skipping final dialogue");
            setupForNextPassenger();
        }

    }
    private void setupForNextPassenger()
    {//setup for next passenger -> transition into silhouette
        DialogueManager.Instance.OnDialogueEnded.RemoveListener(setupForNextPassenger);
        print("starting next passenger setup");
        setupNextDialogueBundle();
        silhouetteModel.SetActive(true);
        controller.silentTransition();
        if (passenger.GetComponent<jimmy_face_swapper>().selectedFace == 0)
        {//if we've looped the circuit, go to win condition.
            FindAnyObjectByType<LevelManager>().LoadScene("Menu_EndGame");
        }
    }

    public void setupNextDialogueBundle(bool rogue = false)
    {
        if (rogue || passenger.GetComponent<jimmy_face_swapper>().getDialogueBundle().windowDialogue==null)
            dialogueBundle = rogueBundle;
        else
        {
            dialogueBundle = passenger.GetComponent<jimmy_face_swapper>().getDialogueBundle();
            if (passenger.GetComponent<jimmy_face_swapper>().selectedFace >= 1 || FindFirstObjectByType<car_interior_controller>().skipTut)
                shouldTriggerBootEvent = true;
        }      
    }
}
