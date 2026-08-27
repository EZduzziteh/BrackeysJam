using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CarSpeedSystem : MonoBehaviour
{
    public UnityEvent OnDestinationReached;
    [SerializeField] float maxSpeed = 100.0f;
    [SerializeField] float maxSpeedModifier = 0.0f;
    [SerializeField] float currentSpeed = 0.0f; // in km/hr
    [SerializeField] float targetDistance = 100.0f;
    float distanceRemaining = 100.0f; //in km
    float elapsedTime = 0.0f;
    float drivingTime = 0.0f;
    bool destinationReached = false;
    bool lookingAtPassenger = false;
    [SerializeField] float baseDeceleration = 8.0f;
    [SerializeField] float  baseAcceleration = 5.0f;

    public float accelerationMultiplier = 1.0f;
    public float decelerationMultiplier = 1.0f;

    float deceleration;
    float acceleration;


    // Update is called once per frame
    void Update()
    {
        CalculateAccelerationMultiplier();

        acceleration = baseAcceleration * accelerationMultiplier;
        deceleration = baseDeceleration * decelerationMultiplier;

        if (Input.GetKey(KeyCode.D))
        {
            lookingAtPassenger = true;
        }
        else
        {
            lookingAtPassenger = false;
        }

        if (!destinationReached)
        {
            if (lookingAtPassenger)
            {
                if (distanceRemaining > 0)
                {
                    currentSpeed += Time.deltaTime * acceleration;

                    if(currentSpeed > maxSpeed)
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
            else
            {
                currentSpeed -= Time.deltaTime * deceleration;
                if(currentSpeed <= 0)
                {
                    currentSpeed = 0.0f;
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
        distanceRemaining -= Time.deltaTime / 3600f * currentSpeed;
    }
}
