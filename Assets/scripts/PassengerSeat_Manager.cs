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

    private void Start()
    {
        controller=  FindFirstObjectByType<car_interior_controller>();
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
                controller.startNewTransition(car_interior_controller.transitionType.full_blink, car_interior_controller.transitionGameEffect.moveScene);
            }
        }
    }
    private void LoadPassenger()
    {
        if(passenger.activeSelf)
        {
            //create next passenger for later.
            debugFakeNextPassenger = Instantiate(passenger);
            debugFakeNextPassenger.SetActive(false);
            debugFakeNextPassenger.GetComponent<jimmy_face_swapper>().selectNewFace();

            //move passenger into car
            SpriteRenderer [] sprites = passenger.GetComponentsInChildren<SpriteRenderer>();
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
        }
        else
            discoverPassenger();
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
    private void discoverPassenger()
    {
        passenger.SetActive(true);
        silhouetteModel.SetActive(false);
    }
    public void checkForDiscovery()
    {
        if(needDiscovery())
            discoverPassenger();
    }
    private bool needDiscovery()
    {
        return !seatOccupied & !passenger.activeSelf;
    } 
    public void stepPassenger()
    {
        if (seatOccupied)
          ejectPassenger();
        else
          LoadPassenger();    
    }
}
