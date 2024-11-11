using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsSFX : MonoBehaviour 
{
    [SerializeField] CharacterController cc;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float maxPitchChange = .2f;
    
    [Header("RunSfx")]
    [SerializeField] AudioClip []runAudioClips;
    [SerializeField] float runStepsInterval = .5f;
    [SerializeField] float runMagnitude = 1f;

    [Header("WalkSfx")]
    [SerializeField] AudioClip []walkAudioClips;
    [SerializeField] float walkStepsInterval = .5f;
    [Range(0,1)]
    [SerializeField] private float walkVolume = .4f;
    
    private void Awake()
    {
    }

    float lastStepTime;
    int toPlay;
    // Update is called once per frame
    void Update()
    {
        if(cc.velocity.magnitude > .1f && cc.velocity.magnitude < runMagnitude && walkAudioClips.Length > 0 && 
           Time.time - lastStepTime > walkStepsInterval)
        {
            PlayRandom(walkAudioClips, walkVolume);
        }
        else if (cc.velocity.magnitude >= runMagnitude && Time.time - lastStepTime >= runStepsInterval)
        {
            PlayRandom(runAudioClips);
        }
    }

    void PlayRandom(AudioClip[] clips, float volume = 1)
    {
        toPlay = Random.Range(0, clips.Length);

        audioSource.volume = volume;
        audioSource.pitch = Random.Range(1 - maxPitchChange, 1 + maxPitchChange);
        audioSource.PlayOneShot(clips[toPlay]);

        lastStepTime = Time.time;
    }
}