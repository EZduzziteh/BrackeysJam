using System;
using UnityEngine;

public class radio_audio_handler : MonoBehaviour
{
    [SerializeField] private AudioSource radioSFX;
    [SerializeField] private AudioSource onOffSFX;
    private SpriteStateSwapper swapperRef;
   
    private void Start()
    {
        swapperRef = GetComponent<SpriteStateSwapper>();
        swapperRef.spriteChanged.AddListener(updateRadioSound);
    }

    private void updateRadioSound()
    {
        print("update for radio sound heard");
        onOffSFX.Play();
        switch (swapperRef.spriteIndex)
        {
            case 0://off
                radioSFX.mute = true;
                break;
            case 1: //on
                if(radioSFX.isPlaying)
                    radioSFX.mute = false;
                else
                    radioSFX.Play();
                break;
        }
    }
}
