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
    //stats
    [SerializeField] private int totalPassengersCollected;
    [SerializeField] private float passengerTimer;
    [SerializeField] private float passengerEventTimer;

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
    }
    private void LoadPassenger()
    {
        if(passenger.activeSelf)
        {
            debugFakeNextPassenger = Instantiate(passenger);
            debugFakeNextPassenger.SetActive(false);
            debugFakeNextPassenger.GetComponent<jimmy_face_swapper>().selectNewFace();

            SpriteRenderer [] sprites = passenger.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer childrenSR in sprites)
            {
                childrenSR.sortingOrder += sortingOrderShift;
            }
            passenger.transform.position = gameObject.transform.position;
            totalPassengersCollected++;
            seatOccupied = true;

            foreach (GameObject model in insideModel)
            {
                model.SetActive(true);
            }
        }
        else
        {
            discoverPassenger();
        }
    }
    private void ejectPassenger()
    {
        print("ejecto seat");
        seatOccupied = false;
        Destroy(passenger);
        passengerTimer = 0f;
        passengerEventTimer = 0f;

        passenger = debugFakeNextPassenger;

        silhouetteModel.SetActive(true);
        foreach (GameObject model in insideModel)
        {
            model.SetActive(false);
        }
        FindFirstObjectByType<car_interior_controller>().silentTransition();
    }
    private void discoverPassenger()
    {
        passenger.SetActive(true);
        silhouetteModel.SetActive(false);
    }
    public void checkForDiscovery()
    {
        if( !seatOccupied & !passenger.activeSelf)
            discoverPassenger();
    }
    public void stepPassenger()
    {
        // Step through the next passenger process automatically. (called by transitions :) ) 
        //if seatOccupied variable is active, that means next step is to eject.
        //if passenger variable is active, Load Passenger into seat, else discover a new passenger.

        if (seatOccupied)
        {
          ejectPassenger();
        }
        else
        {
          LoadPassenger();
        }    
    }
}
