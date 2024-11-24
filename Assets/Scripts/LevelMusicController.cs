using UnityEngine;

public class LevelMusicController : MonoBehaviour
{
    [SerializeField] AudioClip levelMusic;
    [SerializeField] private float fadeTime = 2f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.instance.PlayMusic(levelMusic, fadeTime);
        Destroy(gameObject);
    }
}
