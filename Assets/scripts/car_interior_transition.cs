using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class car_interior_transition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject SceneCamera;
    private InputSystem_Actions controller;
    private Vector3 pos;
    private Animator blinker_animator;
    private void Awake() { controller = new InputSystem_Actions(); }
    private void OnEnable()
    {
        controller.Enable();
        controller.Player.Attack.performed += newTransition;
        controller.Player.LookAtPassenger.performed += newTransition;
    }
    private void OnDisable()
    {
        controller.Disable();
    }
    private void Start()
    {
        blinker_animator = GetComponent<Animator>();
    }
    private void newTransition(InputAction.CallbackContext context)
    {
        //start blink
        blinker_animator.enabled = true;
    }
    private void PerformTransition()
    {
        //called by blinker.
        pos = SceneCamera.transform.position;
        pos.x = getNextTransitionPosition();
        SceneCamera.transform.position = pos;
    }
    private float getNextTransitionPosition()
    {
        if(SceneCamera.transform.position.x==0)
            return 5f;
        return 0f; // default
    }
    private void resetBlinker()
    {
        blinker_animator.enabled = false;
    }
}
