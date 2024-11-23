using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] SoundClip[] musicClips;
    List<AudioSource> musicSources = new List<AudioSource>();
    AudioSource musicSourcePlaying = null;
    [SerializeField] AudioMixerGroup musicGroup = null;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        
        instance = this;

        foreach (var clip in musicClips)
        {
            var source = MakeSourceFromClip(clip);
            
            musicSources.Add(source);
        }
    }
    
    public void PlayMusic(string clipName, float fadeTime = 0)
    {
        var clip = musicClips.ToList().Find(m => m.name == clipName)?.clip;

        if (clip != null)
        {
            PlayMusic(clip, fadeTime);
        }
    }
    
    public void PlayMusic(AudioClip clip, float fadeTime = 0)
    {
        var audioSource = musicSources.Find(s => s.clip == clip);

        Debug.Log("Playing music" + audioSource.clip.name);
        
        if (audioSource == null)
        {
            return;
        }
        
        if (fadeTime > 0)
        {
            StartCoroutine(FadeMusic(musicSourcePlaying, audioSource, fadeTime));
        }
        else
        {
            audioSource.Play();
            musicSourcePlaying.Stop();
            musicSourcePlaying = audioSource;
        }
    }

    IEnumerator FadeMusic(AudioSource from, AudioSource to, float fadeTime)
    {
        float startTime = Time.time;
        float fromVolume = from?.volume ?? 0;
        float toVolume = to.volume;
        
        if (from == to)
        {
            yield break;
        }
        
        musicSourcePlaying = to;
        to.Play();
        
        while (Time.time - startTime < fadeTime)
        {
            float t = (Time.time - startTime)/fadeTime;

            if (from != null)
            {
                from.volume = Mathf.Lerp(fromVolume, 0, t);
            }
            to.volume = Mathf.Lerp(0, toVolume, t);
            
            yield return null;
        }

        if (from != null)
        {
            from.volume = fromVolume;
            from.Stop();
        }
    }

    AudioSource MakeSourceFromClip(SoundClip music)
    {
        var source = gameObject.AddComponent<AudioSource>();
        source.clip = music.clip;
        source.playOnAwake = false;
        source.loop = true;
        source.volume = music.volume;
        source.outputAudioMixerGroup = musicGroup;
        
        return source;
    }
}

[System.Serializable]
class SoundClip
{
    public string name;
    public AudioClip clip = null;
    public float volume = 1.0f;
}