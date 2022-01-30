using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SceneBoundSingletonBehaviour<SoundManager>
{
    [SerializeField]
    Soundbank soundbank;
    [SerializeField]
    AudioSource[] sources;
    int sourceIndex = 0;

    void Awake() {
        sources = GetComponents<AudioSource>();
    }

    public void playShooting()
    {
        playAudioClip(soundbank.launching);
    }

    public void playPolarity() {
        playAudioClip(soundbank.polarity);
    }
    
    public void PlayRespawnSound() 
    {
        playAudioClip(soundbank.puff);
    }

    public void playCanHittingSound() 
    {
        playAudioClip(soundbank.canHitting);
    }

    private void playAudioClip(AudioClip clip, bool loop = false)
    {
        sources[sourceIndex].loop = loop;
        sources[sourceIndex].clip = clip;
        sources[sourceIndex].Play();
        incrementSourceIndex();
    }
    
    private void StopAudioClip(AudioClip clip)
    {
        AudioSource audioSource = GetSourceWithClip(clip);
        if (audioSource != null)
            audioSource.Stop();
    }

    private AudioSource GetSourceWithClip(AudioClip clip)
    {
        for (int i = 0; i < sources.Length; i++)
        {
            if (sources[i].clip == clip)
                return sources[i];
        }

        return null;
    }

    void incrementSourceIndex() {
        sourceIndex = (sourceIndex + 1) % sources.Length;
    }
}
