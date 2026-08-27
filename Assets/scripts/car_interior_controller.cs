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
    [SerializeField] private bool debugStartOnSideView = false;
    private Vector3 pos;

    private interactable highlightedElement;
    private void Awake() { controller = new InputSystem_Actions(); }
    private void OnEnable()
    {
        controller.Enable();
        controller.Player.Attack.performed += checkInteraction;
        controller.Player.LookAtPassenger.performed += hotkeyTransition;
        controller.Player.MousePos.performed += mouseMoved;

        controller.debugs.LoadPassenger.performed += loadPass;
    }
    private void OnDisable()
    {
        controller.Disable();
    }

    private void Start()
    {
        if (debugStartOnSideView)
            silentTransition();
    }
    //car scene transitions

    public enum transitionType {full_blink,wide_blink}
    public enum transitionGameEffect {none,moveScene,passengerCutscene}
    private transitionGameEffect currentEffect;
    [Header("transition animators")]
    [SerializeField] private Animator full_blink_animator;
    [SerializeField] private Animator wide_blink_animator;

    public void startNewTransition(transitionType style=transitionType.full_blink, transitionGameEffect effect=transitionGameEffect.none)
    {
        currentEffect = effect;
        switch (style)
        {
            case transitionType.full_blink:
                full_blink_animator.enabled = true;
                break;
            case transitionType.wide_blink:
                wide_blink_animator.enabled = true;
                break;
        }
    }
    private void hotkeyTransition(InputAction.CallbackContext context) { startNewTransition(default,transitionGameEffect.moveScene); }

    public void animPerformTransition()
    {
        //called by blinker animator script at anim mid-point.
        switch (currentEffect)
        {
            case transitionGameEffect.none:
                break;
            case transitionGameEffect.moveScene:
                //moveScene = false;
                pos = SceneCamera.transform.position;
                pos.x = getNextTransitionPosition();
                SceneCamera.transform.position = pos;
                if (pos.x > 0) //check for discovery on side scene.
                    FindFirstObjectByType<PassengerSeat_Manager>().checkForDiscovery();
                break;
            case transitionGameEffect.passengerCutscene:
                FindFirstObjectByType<PassengerSeat_Manager>().stepPassenger();
                break;
        }
    }
    private float getNextTransitionPosition()
    {
        if(SceneCamera.transform.position.x==0)
            return cameraOffset;
        return 0f; // default
    }
    public void silentTransition()
    {
        currentEffect = transitionGameEffect.moveScene;
        animPerformTransition();
    }
    private void loadPass(InputAction.CallbackContext context) { startNewTransition(transitionType.wide_blink, transitionGameEffect.passengerCutscene); }

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
