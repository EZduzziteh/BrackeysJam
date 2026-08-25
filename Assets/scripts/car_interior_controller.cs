using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class car_interior_controller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private InputSystem_Actions controller;

    [SerializeField] private GameObject SceneCamera;
    [SerializeField] private float cameraOffset=5f;
    private Vector3 pos;
    private Animator blinker_animator;

    private interactable highlightedElement;

    //Startup functions
    private void Awake() { controller = new InputSystem_Actions(); }
    private void OnEnable()
    {
        controller.Enable();
        controller.Player.Attack.performed += checkInteraction;
        controller.Player.LookAtPassenger.performed += hotkeyTransition;
        controller.Player.MousePos.performed += mouseMoved;
    }

    private void OnDisable()
    {
        controller.Disable();
    }

    // Start & Update
    private void Start()
    {
        blinker_animator = GetComponent<Animator>();
    }

    //car scene transitions
    public void startNewTransition()
    {
        blinker_animator.enabled = true;
    }
    private void hotkeyTransition(InputAction.CallbackContext context)
    {
        //start blink
        startNewTransition();
    }
    private void PerformTransition()
    {
        //called by blinker animator.
        pos = SceneCamera.transform.position;
        pos.x = getNextTransitionPosition();
        SceneCamera.transform.position = pos;
    }
    private float getNextTransitionPosition()
    {
        if(SceneCamera.transform.position.x==0)
            return cameraOffset;
        return 0f; // default
    }
    private void resetBlinker()
    {
        blinker_animator.enabled = false;
    }

    //interaction
    private void checkInteraction(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(controller.Player.MousePos.ReadValue<Vector2>());
        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);
        if (hit2D.collider != null)
        {
            Debug.Log(hit2D.collider.gameObject);
            try
            {
                interactable interactee = hit2D.collider.gameObject.GetComponent<interactable>();
                interactee.triggerEffect();
                if (highlightedElement && interactee.shouldHideCursorAfterUse)
                    highlightedElement.hideOutline();
            }
            catch
            {
                Debug.Log("missing interactable component");
            }
        }
    }
    private void mouseMoved(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(controller.Player.MousePos.ReadValue<Vector2>());
        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);
        if (hit2D.collider != null)
        {
            Debug.Log(hit2D.collider.gameObject);
            if (hit2D.collider.gameObject.GetComponent<interactable>())
            {
                if (highlightedElement && highlightedElement.gameObject != hit2D.collider.gameObject)
                    highlightedElement.hideOutline();
                highlightedElement = hit2D.collider.gameObject.GetComponent<interactable>();
                highlightedElement.showOutline();
                return;
            }
            else { Debug.Log("missing interactable component"); }
        }
        if (highlightedElement)//cleanup outlines if we did not highlight something new, and if we've highlighted something before.
            highlightedElement.hideOutline();
    }
}
