using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StressEventManager : MonoBehaviour
{

    float timeSinceLastStressEvent = 0.0f;
    float timeSinceLastStressCheck = 0.0f;

    [SerializeField]float stressEventCheckInterval = 1.0f;

    [SerializeField] float minTimeBetweenEvents = 3.0f;

    [SerializeField] float maxTimeBetweenEvents = 10.0f;

    StressSystem stressSystem;



    public List<StressEventData> StressLevelNoneEvents = new();
    public List<StressEventData> StressLevelUneasyEvents = new();
    public List<StressEventData> StressLevelAnxiousEvents = new();
    public List<StressEventData> StressLevelPanickingEvents = new();
    public List<StressEventData> StressLevelDeliriousEvents = new();




    private void Start()
    {
        stressSystem = FindObjectOfType<StressSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSinceLastStressEvent > minTimeBetweenEvents)
        {
            if (timeSinceLastStressCheck > stressEventCheckInterval)
            {
                CheckForStressEvent();
                timeSinceLastStressCheck = 0.0f;
            }
        }
        timeSinceLastStressEvent += Time.deltaTime;
        timeSinceLastStressCheck += Time.deltaTime;
    }

    float eventBaseChance = 10.0f;



    private void CheckForStressEvent()
    {
        Debug.Log("Checking for stress event");
        if (stressSystem)
        {
            float eventChance = 0.0f;
            var stressState = stressSystem.GetStressState();
            switch (stressState)
            {
                case StressState.None:
                    eventChance = eventBaseChance;
                    break;
                case StressState.Uneasy:
                    eventChance = eventBaseChance + 10.0f;
                    break;
                case StressState.Anxious:
                    eventChance = eventBaseChance +20.0f;
                    break;
                case StressState.Panicking:
                    eventChance = eventBaseChance  + 30.0f;
                    break;
                case StressState.Delirious:
                    eventChance = eventBaseChance +  40.0f ; 
                    break;
            }

            float random = UnityEngine.Random.Range(0, 100.0f);


            Debug.Log("rolled: " + random + " | Chance: + "+eventChance);


            if (random <= eventChance || timeSinceLastStressEvent > maxTimeBetweenEvents)
            {
                Debug.Log("Stress EVENT!: Rolling on stress table: "+stressState);

                List<StressEventData> eventTable = new List<StressEventData>();
                switch (stressState)
                {
                    case StressState.None:
                        eventTable = StressLevelNoneEvents;
                        break;
                    case StressState.Uneasy:
                        eventTable = StressLevelUneasyEvents;
                        break;
                    case StressState.Anxious:
                        eventTable = StressLevelAnxiousEvents;
                        break;
                    case StressState.Panicking:
                        eventTable = StressLevelPanickingEvents;
                        break;
                    case StressState.Delirious:
                        eventTable = StressLevelDeliriousEvents;
                        break;
                }

                int r = UnityEngine.Random.Range(0, eventTable.Count);

                Debug.Log("Event: " + eventTable[r].stressEventID);
                timeSinceLastStressEvent = 0.0f;
            }


        }
    }
}
