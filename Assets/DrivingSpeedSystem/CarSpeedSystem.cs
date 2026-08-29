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
    [SerializeField] float targetDistance = 100.0f;
    float distanceRemaining = 100.0f; //in km
    float elapsedTime = 0.0f;
    float drivingTime = 0.0f;
    bool destinationReached = false;
    bool decelerating = false;
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

    float motorVolumeMax = 1.0f;
    float motorVolumeMin = 0.0f;
    float motorVolume = 0.0f;
    [SerializeField] float motorVolumeIncreaseRate = 0.1f;
    [SerializeField] float motorVolumeDecreaseRate = 0.1f;

    // Update is called once per frame
    void Update()
    {
        CalculateAccelerationMultiplier();

        deceleration = baseDeceleration * decelerationMultiplier;
        acceleration = baseAcceleration * accelerationMultiplier;

        if (Input.GetKey(KeyCode.W) && canDrive)
        {
            decelerating = false;
        }
        else
        {
            decelerating = true;
        }

        if (!decelerating)
        {
            motorVolume += motorVolumeIncreaseRate * Time.deltaTime;
            if(motorVolume > motorVolumeMax)
            {
                motorVolume = motorVolumeMax;
            }
        }
        else
        {
            motorVolume -= motorVolumeDecreaseRate*Time.deltaTime;
            if (motorVolume < motorVolumeMin)
            {
                motorVolume = motorVolumeMin;
            }
        }

        aud.volume = motorVolume;



        if (!destinationReached)
        {
            if (!decelerating && canDrive)
            {
                if (distanceRemaining > 0)
                {
                    currentSpeed += Time.deltaTime * acceleration;

                    if (currentSpeed > maxSpeed)
                    {
                        currentSpeed = maxSpeed;
                    }

                    DriveForward();
                    drivingTime += Time.deltaTime;

                    Debug.Log(distanceRemaining);
                    elapsedTime += Time.deltaTime;
                }
                else
                {
                    destinationReached = true;
                    distanceRemaining = 0;
                    Debug.Log("Destination Reached!");
                    OnDestinationReached?.Invoke();
                }
            }
            else if (decelerating && canDrive)
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
    }

    private void CalculateAccelerationMultiplier()
    {
   

        decelerationMultiplier = 1.0f;
        accelerationMultiplier = 1.0f + maxSpeedModifier / 4.0f;  //   1/4 of max speed modifier is the multiplier?


    }

    private void DriveForward()
    {
        distanceTravelled += Time.deltaTime / 3600f * currentSpeed;
        distanceRemaining -= Time.deltaTime / 3600f * currentSpeed;
    }

    public float CurrentSpeed => currentSpeed;

    public void SetLookingAtPassenger(bool value)
    {
        decelerating = value;
    }


    public void SetCanDrive(bool value)
    {
        canDrive = value;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }
}
