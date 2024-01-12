using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlaySoundFXButton : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip playClip;

    public void PlayFX()
    {
        audioSource.clip = playClip;
        audioSource.Play();
    }
}
