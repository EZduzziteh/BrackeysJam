using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DialogueGameEventHandler : MonoBehaviour
{
    public car_interior_controller controller;
    public PassengerSeat_Manager seat;
    [SerializeField] private float playerTransitionDelay = 1.5f;
    protected virtual void Start()
    {
        DialogueManager.Instance.OnCustomDialogueEventTriggered.AddListener(dialogueHandler);
        //DialogueManager.Instance.OnCustomDialogueEndEventTriggered.AddListener(dialogueHandler);
        controller = FindFirstObjectByType<car_interior_controller>();
        seat = FindFirstObjectByType<PassengerSeat_Manager>();

    }
    private void dialogueHandler()
    {
        if(DialogueManager.Instance.currentDialogueStage.customEventStepID>0)
            dialogue_gameEvents();
    }
    public virtual void dialogue_gameEvents()
    {

    }


    //Transition watching functionality.
    private List<interactable> watchedTransitions = new List<interactable>();
    public virtual void watchForTransitions()
    {
        foreach (interactable interactee in FindObjectsByType<interactable>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (interactee.effectName == interactable.interactableEffectType.transition)
            {
                interactee.transitionedTriggered.AddListener(delayTransitionTrigger);
                watchedTransitions.Add(interactee);
            }
        }

    }
    public virtual void clearWatchedTransitions()
    {
        foreach (interactable interactee in watchedTransitions)
        {
            interactee.transitionedTriggered.RemoveListener(onPlayerTransition);
        }
    }
    private void delayTransitionTrigger()
    {
        StartCoroutine(waitToTriggerTransition());
    }
    IEnumerator waitToTriggerTransition()
    {
        yield return new WaitForSeconds(playerTransitionDelay);
        onPlayerTransition();
    }
    public virtual void onPlayerTransition()
    {
        clearWatchedTransitions();
        //called when transitions trigger, if the watch was setup.
    }


    //general timer functionality.
    public float timerForEvents;
    [SerializeField] private bool _enableTimer = false;
    private float timerTriggerDuration = 5.0f;
    public bool enableTimer
    {
        get { return _enableTimer; }
        set {
            _enableTimer = value;
            if (!_enableTimer)
                timerForEvents = 0f;
        }
    }
    public void startTimer(float duration)
    {
        timerTriggerDuration = duration;
        enableTimer = true;
    }
    void Update()
    {
        if (enableTimer)
        {
            timerForEvents += Time.deltaTime;
            if (timerForEvents > timerTriggerDuration)
            { enableTimer = false; timerTriggered(); } 
        }
    }
    public virtual void timerTriggered()
    { 
        //called when the timer trigger duration is met.
    }
    private void OnValidate()
    {
        enableTimer = _enableTimer;
    }





}