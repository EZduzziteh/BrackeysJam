using UnityEngine;
using System.Collections;
using System;

public class totem_anim_handler : MonoBehaviour
{
    public static totem_anim_handler Instance;
    private Animator animController;
    [SerializeField] private Animator sideViewController;

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
        animController = GetComponent<Animator>();
    }
    void Start()
    {
        //animController = GetComponent<Animator>();
        //StartCoroutine(showStates());
    }

    IEnumerator showStates()
    {
        updateState(4);
        yield return new WaitForSeconds(5);
        updateState(0);
        yield return new WaitForSeconds(5);
        StartCoroutine(showStates());
    }

    public void updateState(int state)
    {
        animController.SetInteger("state", state);
        sideViewController.SetInteger("state", state);

        Debug.Log("UPDATED STATE: " + state);
    }

}
