using System.Collections;
using UnityEngine;

public class SymbolSpeaker : MonoBehaviour
{
    GameObject balloon;
    [SerializeField] GameObject balloonPrefab;
    public void Speak(Meaning[] customPhrase, int time = 2){
        StartCoroutine(ShowBalloon(customPhrase, time));
    }

    IEnumerator ShowBalloon(Meaning[] phrase, int time){
        if(balloon)
            Destroy(balloon);
            
        balloon = Instantiate(balloonPrefab);
        BalloonText text = balloon.GetComponent<BalloonText>();
        text.SetTarget(transform);
        text.Write(phrase);
        yield return new WaitForSeconds(time);
        Destroy(balloon.gameObject);
    }
}
