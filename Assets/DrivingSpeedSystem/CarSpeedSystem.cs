using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CarSpeedSystem : MonoBehaviour
{
    public UnityEvent OnDestinationReached;
    public UnityEvent OnCarStopped;
    [SerializeField] float maxSpeed = 100.0f;
    [SerializeField] float maxSpeedModifier = 0.0f;
    [SerializeField] float currentSpeed = 0.0f; // in km/hr
    float elapsedTime = 0.0f;
    float drivingTime = 0.0f;
    bool lookingAtPassenger = false;
    [SerializeField] bool canDrive = true;
    public bool CanDrive => canDrive;
    [SerializeField] float baseDeceleration = 8.0f;
    [SerializeField] float  baseAcceleration = 5.0f;

    public float accelerationMultiplier = 1.0f;
    public float decelerationMultiplier = 1.0f;

    float deceleration;
    float acceleration;


    float distanceTravelled = 0.0f;


    AudioSource aud;

    private void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    [SerializeField] float motorVolumeMax = 1.0f;
    [SerializeField] float motorVolumeMin = 0.0f;
    float motorVolume = 0.0f;
    [SerializeField] float motorVolumeIncreaseRate = 0.1f;
    [SerializeField] float motorVolumeDecreaseRate = 0.1f;


    public AudioSource roadAudioSource;
    [SerializeField] float roadVolumeMax = 1.0f;
    [SerializeField] float roadVolumeMin = 0.0f;
    float roadVolume = 0.0f;
    [SerializeField] float roadVolumeIncreaseRate = 0.1f;
    [SerializeField] float roadVolumeDecreaseRate = 0.1f;


    // Update is called once per frame
    void Update()
    {


        




        CalculateAccelerationMultiplier();

        deceleration = baseDeceleration * decelerationMultiplier;
        acceleration = baseAcceleration * accelerationMultiplier;
        

        if (!lookingAtPassenger && canDrive)
        {
            motorVolume += motorVolumeIncreaseRate * Time.deltaTime;
           
            if(motorVolume > motorVolumeMax)
            {
                motorVolume = motorVolumeMax;
            }

            roadVolume += roadVolumeIncreaseRate * Time.deltaTime;
            if (roadVolume > roadVolumeMax)
            {
                roadVolume = roadVolumeMax;
            }
        }
        else
        {
            motorVolume -= motorVolumeDecreaseRate*Time.deltaTime;
            if (motorVolume < motorVolumeMin)
            {
                motorVolume = motorVolumeMin;
            }
            roadVolume -= roadVolumeDecreaseRate * Time.deltaTime;
            if (roadVolume < roadVolumeMin)
            {
                roadVolume = roadVolumeMin;
            }
        }

        aud.volume = motorVolume;
        roadAudioSource.volume = roadVolume;

        
            if (!lookingAtPassenger && canDrive)
            {
                
                    currentSpeed += Time.deltaTime * acceleration;

                    if (currentSpeed > maxSpeed)
                    {
                        currentSpeed = maxSpeed;
                    }

                    DriveForward();
                    drivingTime += Time.deltaTime;

                   // Debug.Log(distanceRemaining);
                    elapsedTime += Time.deltaTime;
               
            }
            else
            {
                if (currentSpeed > 0)
                {
                    currentSpeed -= Time.deltaTime * deceleration;

                    if (currentSpeed <= 0)
                    {
                        currentSpeed = 0.0f;
                        OnCarStopped?.Invoke();
                    }
                }
            }
        }
    



    public void StopDriving()
    {
        currentSpeed = 0;
        SetCanDrive(false);
    }

    private void CalculateAccelerationMultiplier()
    {
   

        decelerationMultiplier = 1.0f;
        accelerationMultiplier = 1.0f + maxSpeedModifier / 4.0f;  //   1/4 of max speed modifier is the multiplier?


    }

    private void DriveForward()
    {
        distanceTravelled += Time.deltaTime / 3600f * currentSpeed;
    }

    public float CurrentSpeed => currentSpeed;

    public bool GetLookingAtPassenger()  {
        return lookingAtPassenger;
    }

    public void SetCanDrive(bool value)
    {
        canDrive = value;
        SetLookingAtPassenger(!value);
    }

    public void SetLookingAtPassenger(bool value)
    {
        lookingAtPassenger = value;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }
}
