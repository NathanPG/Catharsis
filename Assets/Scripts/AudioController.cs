using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip jumpClip;
    public AudioClip landClip;
    public AudioClip grappleClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void JumpSound()
    {
        //audioSource.PlayOneShot(jumpClip);
    }

    public void LandSound()
    {
        audioSource.PlayOneShot(landClip);
    }

    public void GrappleSound()
    {
        audioSource.PlayOneShot(grappleClip);
    }
}
