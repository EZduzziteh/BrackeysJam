using UnityEngine;

public class PassengerSeat_Manager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject passenger;
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
        debugFakeNextPassenger = Instantiate(passenger);
        debugFakeNextPassenger.SetActive(false);
        debugFakeNextPassenger.GetComponent<jimmy_face_swapper>().selectNewFace();

        SpriteRenderer [] sprites = passenger.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer childrenSR in sprites)
        {
            childrenSR.sortingOrder += 2;
        }
        passenger.transform.position = gameObject.transform.localPosition;
        totalPassengersCollected++;
        seatOccupied = true;
    }
    public void ejectPassenger()
    {
        print("ejecto seat");
        seatOccupied = false;
        Destroy(passenger);
        passengerTimer = 0f;
        passengerEventTimer = 0f;

        passenger = debugFakeNextPassenger;
        passenger.SetActive(true);
    }
}
