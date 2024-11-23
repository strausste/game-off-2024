using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayRandomSound : MonoBehaviour
{
    public AudioClip[] audioClips;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float volume = 1f;
    [Range(0, 2)] [SerializeField] private float maxPitch = 1.25f;
    [Range(0, 1)] [SerializeField] private float minPitch = 0.85f;
    [SerializeField] AudioMixerGroup outputGroup; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = outputGroup;
        }
    }

    public void PlayRandom()
    {
        int toPlay = Random.Range(0, audioClips.Length);

        audioSource.volume = volume;
        audioSource.pitch = Random.Range(maxPitch, minPitch);
        audioSource.PlayOneShot(audioClips[toPlay]);
    }
}
