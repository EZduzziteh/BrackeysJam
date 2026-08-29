using System.Collections;
using UnityEngine;

public class CarSpeedOdometerWrapper : MonoBehaviour
{
    [SerializeField] CarSpeedSystem carSpeedSystem;
    [SerializeField] Odometer_UI odometer;
    [SerializeField] float kilometersPerOdometerNumber = 0.01f;
    float distanceSinceLastNumber;
    bool odometerIsMoving;

    void Update()
    {
        if (!carSpeedSystem.CanDrive)
        {
            return;
        }
        distanceSinceLastNumber += carSpeedSystem.CurrentSpeed * Time.deltaTime / 3600f;
        if (!odometerIsMoving && distanceSinceLastNumber >= kilometersPerOdometerNumber)
        {
            distanceSinceLastNumber -= kilometersPerOdometerNumber;
            StartCoroutine(IncreaseOdometer());
        }
    }

    IEnumerator IncreaseOdometer()
    {
        odometerIsMoving = true;
        yield return odometer.IncreaseFullNumberCoroutine();
        odometerIsMoving = false;
    }
}