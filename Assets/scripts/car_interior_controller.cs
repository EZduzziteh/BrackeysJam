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
    public bool skipTut = true;
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
        if(skipTut)
        {
            lockTransitions(false);
            FindFirstObjectByType<PassengerSeat_Manager>().checkForDiscovery();
            FindFirstObjectByType<PassengerSeat_Manager>().setupNextDialogueBundle();
        }
    }
    //car scene transitions

    public enum transitionType {full_blink,wide_blink}
    public enum transitionGameEffect {none,moveScene,passengerCutscene}
    private transitionGameEffect currentEffect;
    [Header("transition animators")]
    [SerializeField] private Animator full_blink_animator;
    [SerializeField] private Animator wide_blink_animator;
    [SerializeField]AudioClip swapViewSoundEffect;

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
    private void hotkeyTransition(InputAction.CallbackContext context)
    { 
        if(canTransition())
            startNewTransition(default,transitionGameEffect.moveScene);
    }

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

                FindFirstObjectByType<CarSpeedSystem>()?.SetCanDrive(!(pos.x > 0));
                SoundEffectManager.PlaySoundEffectWithRandomPitch(swapViewSoundEffect);

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
    public void checkExpectedScene(bool expectingDriverView)
    {
        if (SceneCamera.transform.position.x == 0 && expectingDriverView || SceneCamera.transform.position.x > 0 && !expectingDriverView)
        {
            //matching, need driver view, is driver view. no issue.
        }
        else if (SceneCamera.transform.position.x == 0 && !expectingDriverView || SceneCamera.transform.position.x > 0 && expectingDriverView)
        {
            //is driver, expecting side || is side, expecting driver
            startNewTransition(default, transitionGameEffect.moveScene);
        }

    }
    private void loadPass(InputAction.CallbackContext context) { startNewTransition(transitionType.wide_blink, transitionGameEffect.passengerCutscene); }
    public void lockTransitions(bool shouldLock)
    {
        foreach (interactable interactee in FindObjectsByType<interactable>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (interactee.effectName == interactable.interactableEffectType.transition)
                interactee.gameObject.SetActive(!shouldLock);
        }
    }
    private bool canTransition()
    {
        foreach (interactable interactee in FindObjectsByType<interactable>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (interactee.effectName == interactable.interactableEffectType.transition)
            {
                if (!interactee.gameObject.activeSelf)
                    return false; //block transition if any are deactivated.
            }   
        }
        return true;
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
