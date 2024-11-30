using System.Collections;
using UnityEngine;

public class SymbolSpeaker : MonoBehaviour
{
    GameObject balloon;
    [SerializeField] GameObject balloonPrefab;
    [SerializeField] Vector3 offset;
    [SerializeField] bool flip = false;
    public void Speak(Meaning[] customPhrase, int time = 2){
        StartCoroutine(ShowBalloon(customPhrase, time));
    }

    IEnumerator ShowBalloon(Meaning[] phrase, int time){
        if(balloon)
            Destroy(balloon);
            
        balloon = Instantiate(balloonPrefab);
        BalloonText text = balloon.GetComponent<BalloonText>();
        text.SetTarget(transform);
        if (offset != Vector3.zero)
            text.SetOffset(offset);
        if (flip)
            text.Flip();
        text.Write(phrase);
        yield return new WaitForSeconds(time);
        Destroy(balloon.gameObject);
    }
}
