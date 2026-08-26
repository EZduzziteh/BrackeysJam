using UnityEngine;

public class PassengerSeat_Manager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject passenger;
    [SerializeField] private int sortingOrderShift=3;
    private GameObject debugFakeNextPassenger;
    [SerializeField] private int totalPassengersCollected;
    [SerializeField] private float passengerTimer;
    [SerializeField] private float passengerEventTimer;
    private bool seatOccupied = false;

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
    public void LoadPassenger()
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
        }
        else
        {
            discoverPassenger();
        }
    }
    public void ejectPassenger()
    {
        print("ejecto seat");
        seatOccupied = false;
        Destroy(passenger);
        passengerTimer = 0f;
        passengerEventTimer = 0f;

        passenger = debugFakeNextPassenger;    
    }
    public void discoverPassenger()
    {
        passenger.SetActive(true);
    }

    // Step through the next passenger process automatically. (called by transitions :) ) 
    public void stepPassenger()
    {
        //if seatOccupied variable is active, that means next step is to eject.
        //if passenger variable is not active? if not, discovr, if so, load.

        if (seatOccupied)
        {
            ejectPassenger();
        }
        else
        {
            if(passenger.activeSelf)
            {
                LoadPassenger();
            }
            else
            {
                discoverPassenger();
            }
        }    
    }
}
