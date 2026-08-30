using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StressSystem : MonoBehaviour
{
    [SerializeField]StressState currentStressState;

    float currentStress;
    [SerializeField] float stressThreshold = 100.0f;

    public List<StressSource> StressSources = new();

    public float StressTickInterval = 1.0f;
    float timeSinceLastTick;

    public UnityEvent OnMaxStressAchieved;
    public UnityEvent OnStressIncreased;
    public UnityEvent OnStressDecreased;

    float totalStressAccumulated;
    float totalStressRemoved;

    private void Start()
    {
        currentStress = 0.0f;
        timeSinceLastTick = 0.0f;
        currentStressState = StressState.None;
        OnStressDecreased.AddListener(UpdateTotemBasedOnStress);
        OnStressIncreased.AddListener(UpdateTotemBasedOnStress);

        OnMaxStressAchieved.AddListener(() =>
        {
            FindFirstObjectByType<PassengerSeat_Manager>().startBootDialogue();
        });

        UpdateTotemBasedOnStress();

    }

    private void Update()
    {
       timeSinceLastTick += Time.deltaTime;

        if(timeSinceLastTick > StressTickInterval)
        {
            TickStress();
            timeSinceLastTick = 0.0f;
        }

    }

    public StressState GetStressState()
    {
        return currentStressState;
    }
    public void TickStress()
    {
        
        foreach (var stressor in StressSources)
        {
            float stressAmount = 0.0f;
            stressAmount = MathF.Abs(stressor.amount);

            if (FindFirstObjectByType<CarSpeedSystem>().GetLookingAtPassenger() == true)
            {
                stressAmount *= -1;
            }

            currentStress += stressAmount;
            TrackStress(stressAmount);
        }

        CheckStressLevel();
    }
    private void CheckStressLevel()
    {
        if (currentStress <= 0)
        {
            DecreaseStressLevel();
            
        }

        if (currentStress >= stressThreshold)
        {
            IncreaseStressLevel();
        }
    }

    public void ModifyStress(float amount)
    {
        TrackStress(amount);
        currentStress += amount;
        CheckStressLevel();
    }

    void TrackStress(float amount)
    {
        if (amount > 0)
        {
            totalStressAccumulated += amount;
        }
        else
        {
            totalStressRemoved += amount;
        }
    }

    public void AddStressor(StressSource newSource)
    {
        StressSources.Add(newSource);
        Debug.Log("Stress added!");
    }

    public void RemoveStressor(int index)
    {
        if(index < 0)
        {
            return;
        }
        StressSources.RemoveAt(index);
        Debug.Log("Stress removed!");
    }


    public void IncreaseStressLevel()
    {
        if (currentStressState == StressState.Delirious) {
            currentStress = 0;
            currentStressState = 0;
            StressSources.Clear();
            OnMaxStressAchieved?.Invoke();
            return; //return becasue we arealready max stress
        }

        currentStressState++;
        currentStress -= stressThreshold;
        OnStressIncreased?.Invoke();
    }


    public void DecreaseStressLevel()
    {
        if (currentStressState == StressState.None) {
            currentStress = 1;
            return; //return because we arealready min stress
        }

        currentStressState--;
        currentStress += stressThreshold;
        OnStressDecreased?.Invoke();
    }


    public void UpdateTotemBasedOnStress()
    {
        totem_anim_handler.Instance.updateState((int)currentStressState);
    }


    internal int IndexOfGameObjectStressor(GameObject gameObject)
    {

        for(int i = 0;i <   StressSources.Count; i++) {
            if (StressSources[i].gameObjectReference == gameObject) {

                return i;
            }
        
        }

        return -1;

    }
}
