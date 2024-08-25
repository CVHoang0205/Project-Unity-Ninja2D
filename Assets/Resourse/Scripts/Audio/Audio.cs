using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource musicAudioSource;
    public AudioSource vfxAudio;

    public AudioClip musicClip;
    public AudioClip cointClip;
    public AudioClip WinClip;
    // Start is called before the first frame update
    void Start()
    {
        musicAudioSource.clip = musicClip;
        musicAudioSource.Play();
    }

    public void PlaySFX(AudioClip sfxClip) 
    {
        vfxAudio.clip = sfxClip;
        vfxAudio.PlayOneShot(sfxClip);
    }
}
