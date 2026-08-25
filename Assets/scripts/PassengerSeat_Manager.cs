using UnityEngine;

public class PassengerSeat_Manager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject passenger;
    public void LoadPassenger()
    {
        SpriteRenderer [] sprites = passenger.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer childrenSR in sprites)
        {
            childrenSR.sortingOrder += 2;
        }
        passenger.transform.position = gameObject.transform.localPosition;
    }
}
