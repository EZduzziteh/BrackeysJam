using UnityEngine;

public class anim_sillhouette_handler : MonoBehaviour
{
    private void silhouetteReachesCar()
    {
        GetComponent<Animator>().enabled = false;
        GetComponent<Animator>().enabled = true;
        FindFirstObjectByType<PassengerSeat_Manager>().stepPassenger();
    }
}
