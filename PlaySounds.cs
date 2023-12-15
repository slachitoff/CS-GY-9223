using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySounds : MonoBehaviour
{
    private AudioSource clickSound;
    private AudioSource loopSound;

    void Start()
    {
        clickSound = GetComponents<AudioSource>()[0];  
        loopSound = GetComponents<AudioSource>()[1];  
    }

    public void ClickSound()
    {
        clickSound?.Play();
    }

    public void ToggleLoopSound()
    {
        if (loopSound.isPlaying)
        {
            loopSound.Stop();
        }
        else
        {
            loopSound.Play();
        }
    }
}