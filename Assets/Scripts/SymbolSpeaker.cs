using System.Collections;
using UnityEngine;

public class SymbolSpeaker : MonoBehaviour
{
    [SerializeField] GameObject balloonPrefab;
    [SerializeField] Meaning[] playerSpottedPhrase;
    [SerializeField] Meaning[] diePhrase;
    [SerializeField] Meaning[] generalPhrase;

    public enum PhraseType {
        PLAYER_SPOTTED,
        DIE,
        GENERAL
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(ShowBalloon(generalPhrase));
    }
    
    public void Speak(PhraseType phrase_type){
        switch (phrase_type){
            case PhraseType.PLAYER_SPOTTED: 
                StartCoroutine(ShowBalloon(generalPhrase));
                break;
            case PhraseType.DIE: 
                StartCoroutine(ShowBalloon(diePhrase));
                break;
            case PhraseType.GENERAL: 
                StartCoroutine(ShowBalloon(generalPhrase));
                break;
        }
        
    }

    IEnumerator ShowBalloon(Meaning[] phrase){
        GameObject balloon = Instantiate(balloonPrefab);
        BalloonText text = balloon.GetComponent<BalloonText>();
        text.SetTarget(transform);
        text.Write(phrase);
        yield return new WaitForSeconds(2);
        Destroy(balloon.gameObject);
    }
}
