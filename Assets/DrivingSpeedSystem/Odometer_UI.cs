
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Odometer_UI : MonoBehaviour
{
    public float spriteHeight = 0.8f;
    public float pixelDistance = 0.01f;
    public List<SpriteRenderer> OdometerObjects = new();

    [SerializeField] int activeOdometerNumber = 1;

    public UnityEvent OnOdometerTickOver;
    public UnityEvent OnOdometerTickUnder;

    public UnityEvent OnOdometerValueDecreased;
    public UnityEvent OnOdometerValueIncreased;

    public List<Sprite> OdometerSprites = new List<Sprite>();

    public bool UsePlayerInput = false; //#TODO remove this, this is just for debugging
    bool isMoving;

    /*private float YScale => Mathf.Abs(transform.lossyScale.y);*/

    // Update is called once per frame
    void Update()
    {
        /*
        if (UsePlayerInput && !isMoving)
        {
            timeSinceLastOdometerTick += Time.deltaTime;
            if (timeSinceLastOdometerTick > OdometerTickInterval)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    IncreaseOdometer();
                    timeSinceLastOdometerTick = 0.0f;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    DecreaseOdometer();
                    timeSinceLastOdometerTick = 0.0f;
                }
            }
        }*/

    }

    public void IncreaseFullNumber()
    {
        StartCoroutine(IncreaseFullNumberCoroutine());

    }
    public IEnumerator IncreaseFullNumberCoroutine()
    {
        bool completed = false;
        int maxAttempts = 100;
        int attempts = 0;
        while (!completed)
        {
            yield return new WaitForSeconds(0.01f);
            completed = IncreaseOdometer();
            attempts++;

            if (attempts >= maxAttempts)
            {
                completed = true;
                Debug.Log("Something went wrong and we went over our max attempts/.");
            }
        }
    }
    public void DecreaseFullNumber()
    {
        StartCoroutine(DecreaseFullNumberCoroutine());
    }
    public IEnumerator DecreaseFullNumberCoroutine()
    {
        bool completed = false;
        int maxAttempts = 100;
        int attempts = 0;
        while (!completed)
        {
            yield return new WaitForSeconds(0.01f);
            completed = DecreaseOdometer();
            attempts++;
            if(attempts >= maxAttempts)
            {
                completed = true;
                Debug.Log("Something went wrong and we went over our max attempts/.");
            }
        }
    }
    private bool DecreaseOdometer()
    {
        foreach(var o in OdometerObjects)
        {
            o.transform.Translate(0, -pixelDistance /** YScale*/, 0);
        }


        float distToCenter = Vector3.Distance(OdometerObjects[0].transform.position, transform.position);


        if (distToCenter <= (pixelDistance / 2.0f)/** YScale*/)
        {
            activeOdometerNumber--;
            if (activeOdometerNumber < 0)
            {
                activeOdometerNumber = 9;

                OnOdometerTickUnder?.Invoke();

            }
            //#TODO swap the odometer objects around, change displayed numbers.

            //get the last object in odometer
            var temp = OdometerObjects[2];
            //move it up 2x odometer positions
            temp.transform.Translate(0, (spriteHeight * 3) /** YScale*/, 0); //sprite height * 3 because there are 3 odometer numbers

            //shift odometer list
            OdometerObjects[2] = OdometerObjects[1];
            OdometerObjects[1]= OdometerObjects[0];
            OdometerObjects[0] = temp;


            //Update Sprites

            if(activeOdometerNumber > 0)
            {
                OdometerObjects[0].sprite = OdometerSprites[activeOdometerNumber - 1];
            }
            else
            {
                OdometerObjects[0].sprite = OdometerSprites[OdometerSprites.Count - 1];
            }
            
            
            OdometerObjects[1].sprite = OdometerSprites[activeOdometerNumber];

            if(activeOdometerNumber < 9)
            {

                OdometerObjects[2].sprite = OdometerSprites[activeOdometerNumber + 1];
            }
            else
            {
                OdometerObjects[2].sprite = OdometerSprites[0];
            }

            OnOdometerValueDecreased?.Invoke();
            return true;
        }

        return false;

        


    }


    private bool IncreaseOdometer()
    {
        foreach (var o in OdometerObjects)
        {
            o.transform.Translate(0, pixelDistance /** YScale*/, 0);
        }

    
        float distToCenter = Vector3.Distance(OdometerObjects[2].transform.position, transform.position);


        if (distToCenter <= (pixelDistance/2)  /** YScale*/)
        {
            activeOdometerNumber++;
            if (activeOdometerNumber > 9)
            {
                activeOdometerNumber = 0;

                OnOdometerTickOver?.Invoke();
            }
            //#TODO swap the odometer objects around, change displayed numbers.

            //get the first object in odometer
            var temp = OdometerObjects[0];
            //move it down 3x odometer positions
            temp.transform.Translate(0, -(spriteHeight * 3)  /** YScale*/, 0); //sprite height * 3 because its odometyer number height * 3\

            //shift odometer list
            OdometerObjects[0] = OdometerObjects[1];
            OdometerObjects[1] = OdometerObjects[2];
            OdometerObjects[2] = temp;


            //Update Sprites

            if (activeOdometerNumber > 0)
            {
                OdometerObjects[0].sprite = OdometerSprites[activeOdometerNumber - 1];
            }
            else
            {
                OdometerObjects[0].sprite = OdometerSprites[OdometerSprites.Count - 1];
            }


            OdometerObjects[1].sprite = OdometerSprites[activeOdometerNumber];

            if (activeOdometerNumber < 9)
            {

                OdometerObjects[2].sprite = OdometerSprites[activeOdometerNumber + 1];
            }
            else
            {
                OdometerObjects[2].sprite = OdometerSprites[0];
            }


            OnOdometerValueIncreased?.Invoke();
            return true;

        }

        return false;


    }


}
