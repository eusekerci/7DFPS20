#define AUDIO_ENABLED

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GlitchSfxType
{
    Regular,
    End
}

public class Sfx : MonoBehaviour
{
    public AudioSource WalkAudioSource;
    public AudioSource OtherAudioSource;
    public AudioSource MusicAudioSource;

    [Space]

    public AudioClip JumpClip;
    public AudioClip LandClip;
    public AudioClip LandFromHeightClip;
    public AudioClip EmptyFireClip;
    public AudioClip ClickClip;
    public AudioClip ButtonClip;

    public List<AudioClip> Footsteps;

    void Start()
    {
#if !AUDIO_ENABLED
        MusicAudioSource.volume = 0;
#endif
    }

    public void OnMusicVolumeChanged(float normalizedValue)
    {
        MusicAudioSource.volume = normalizedValue;
    }

    public void OnSfxVolumeChanged(float normalizedValue)
    {
        OtherAudioSource.volume = normalizedValue;
    }

    public void Jump()
    {
#if AUDIO_ENABLED
        OtherAudioSource.PlayOneShot(JumpClip);
#endif
        return;
    }

    public void Land()
    {
        OtherAudioSource.PlayOneShot(LandClip);
    }

    public void LandFromHeight()
    {
        OtherAudioSource.PlayOneShot(LandFromHeightClip);
    }

    public void Footstep()
    {
#if !AUDIO_ENABLED
        return;
#endif
        WalkAudioSource.PlayOneShot(Footsteps[0]);

        var playedSound = Footsteps[0];

        Footsteps.RemoveAt(0);
        Footsteps.Shuffle();
        Footsteps.Add(playedSound);
    }

    public void Button()
    {
        OtherAudioSource.PlayOneShot(ButtonClip);
    }
}

