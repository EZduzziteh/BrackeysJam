using Unity.VisualScripting;
using UnityEngine;

public class BackgroundAnimationController : MonoBehaviour
{

    public float animationSpeed = 0.0f;

    public float animationSpeedCheckInterval = 1.0f;
    float animationSpeedCheckTimer = 0.0f;

    CarSpeedSystem carSpeedSystem;

    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        carSpeedSystem = FindFirstObjectByType<CarSpeedSystem>();

        UpdateAnimationSpeed();
    }




    // Update is called once per frame
    void Update()
    {
        animationSpeedCheckTimer += Time.deltaTime;
        if (animationSpeedCheckTimer > animationSpeedCheckInterval)
        {
            UpdateAnimationSpeed();
            animationSpeedCheckTimer = 0.0f;
        }
    }

    private void UpdateAnimationSpeed()
    {
        animationSpeed = carSpeedSystem.CurrentSpeed / carSpeedSystem.GetMaxSpeed();
        anim.speed = animationSpeed;
    }
}
