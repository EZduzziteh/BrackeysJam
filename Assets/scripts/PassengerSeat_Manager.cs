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
    [SerializeField] private DialogueCoreBundle rogueBundle;

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
                passengerEventTimer = 0f;
            }
        }
        if (needDiscovery())
        {
            silhouetteEventTimer += Time.deltaTime;
            if (silhouetteEventTimer >= silhouetteMaxDuration)
            {
                silhouetteEventTimer = 0f;
                controller.startNewTransition(car_interior_controller.transitionType.full_blink, car_interior_controller.transitionGameEffect.moveScene);
            }
        }
    }

    private void LoadPassenger()
    {
        if (passenger.activeSelf)
        {
            print("Loading Passenger");
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
        passenger = debugFakeNextPassenger; //update ref for new passenger.
        foreach (GameObject model in insideModel)
        {
            model.SetActive(false);
        }
        seatOccupied = false;
        passengerTimer = 0f;
        passengerEventTimer = 0f;

        //play final dialogue
        DialogueManager.Instance.StartDialogue(dialogueBundle.finalDialogue);
        DialogueManager.Instance.OnDialogueEnded.AddListener(setupForNextPassenger);
    }
    private void setupForNextPassenger()
    {//setup for next passenger -> transition into silhouette
        DialogueManager.Instance.OnDialogueEnded.RemoveListener(setupForNextPassenger);
        moveIntoBasicDialogue(); // this should load next passenger dialogue bundle, placeholder for now to turn off grandma dialogue.
        silhouetteModel.SetActive(true);
        controller.silentTransition();
    }
    public void checkForDiscovery()
    {
        print("attempting discover");
        if (needDiscovery())
        {
            print("Discovering Passenger");
            passenger.SetActive(true);
            silhouetteModel.SetActive(false);
            silhouetteEventTimer = 0f; // cleanup
            if (dialogueBundle.windowDialogue)
            {
                DM.StartDialogue(dialogueBundle.windowDialogue);
                DM.OnDialogueEnded.AddListener(loadListener);
            }
        }
        else
        {//activate silhouette 
            if(!silhouetteModel.activeSelf & !seatOccupied & !passenger.activeSelf)
                silhouetteModel.SetActive(true);
        }
    }
    private bool needDiscovery()
    {
        return !seatOccupied & !passenger.activeSelf & silhouetteModel.activeSelf;
    }
    private void loadListener() { DM.OnDialogueEnded.RemoveListener(loadListener); controller.startNewTransition(car_interior_controller.transitionType.wide_blink, car_interior_controller.transitionGameEffect.passengerCutscene); }

    public void startBootDialogue()
    {
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

        /* used for only select answer to eject. probably can be refactored into dialogue event handler.
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

    public void moveIntoBasicDialogue()
    {
        dialogueBundle = rogueBundle;
    }
}
