using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitch_Slider : MonoBehaviour
{
    public float startingPitch = 1;
    public float timeToIncrease = 10;
    AudioSource audioSource;

    void Start()
    {
        //Fetch the AudioSource from the GameObject
        audioSource = GetComponent<AudioSource>();

        //Initialize the pitch
        audioSource.pitch = startingPitch;
    }

    void Update()
    {
        //While the pitch is over 0, increases it as time passes.
        if(audioSource.pitch > 0)
        {
            audioSource.pitch += Time.deltaTime * startingPitch / timeToIncrease;
        }
    }
}