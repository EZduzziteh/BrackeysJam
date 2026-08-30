using System;
using UnityEngine;

public class StatTracker : MonoBehaviour
{
    public static StatTracker Instance;
    public float distanceTravelled;
    public float murders;

    //collected from PassengerSeat_Manager during eject.
    public int totalPassengersCollected;
    public float totalPassengerTime;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        if(level == 0)
        {
            ResetStats();
        }
    }

    public void ResetStats()
    {
        distanceTravelled = 0.0f;
        totalPassengersCollected = 0;
    }
}
