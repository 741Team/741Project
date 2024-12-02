using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    AudioSource audioSource;
    AudioClip woodsFootstep;
    AudioClip stoneFootstep;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootstepSound(string groundType)
    {
        if (groundType == "Woods")
        {
            audioSource.clip = woodsFootstep;
        }
        else if (groundType == "Stone")
        {
            audioSource.clip = stoneFootstep;
        }
        audioSource.Play();
    }
}
