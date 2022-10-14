using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource_1;
    [SerializeField] private AudioSource audioSource_2;

    public void PlayWalkSound()
    {
        if (!audioSource_1.isPlaying)
        {
            audioSource_1.Play();
        }
        else 
        {
            audioSource_2.Play();
        }
    }
}
