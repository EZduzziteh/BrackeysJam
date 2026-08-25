using UnityEngine;

public class blinker_anim_handler : MonoBehaviour
{

    private void resetAnimator () { GetComponent<Animator>().enabled = false; }
    private void transitionInteriorScene() { FindFirstObjectByType<car_interior_controller>().animPerformTransition(); }
}
