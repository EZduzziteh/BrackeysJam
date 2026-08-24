using UnityEngine;

public class interactable : MonoBehaviour
{
    public string effectName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void triggerEffect()
    {
        switch (effectName)
        {
            case "transition":
                FindFirstObjectByType<car_interior_controller>().startNewTransition();
                return;
            case "radio":
                print("jamming jamming jamming, what's brackening party ppl?");
                return;
            default:
                print("you interacted to no effect!");
                return;
        }
    }
}
