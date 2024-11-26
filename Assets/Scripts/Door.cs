using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject destination;
    bool locked = false;
    /*[SerializeField] private float fadeDuration = 0.2f;
    
    private Image fadeImage;

    void Start()
    {
        GameObject fadeObject = GameObject.FindWithTag("UI_FadeImage");

        if (fadeObject != null)
        {
            fadeImage = fadeObject.GetComponent<Image>();
        }
        else
        {
            Debug.LogError("UI_FadeImage object not found!");
        }
    }
    */
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !locked)
        {
            other.gameObject.SetActive(false);
            other.gameObject.transform.position = destination.transform.position;
            other.gameObject.SetActive(true);

            //StartCoroutine(TeleportWithFade(other));
        }
    }
    /*
    private IEnumerator TeleportWithFade(Collider other)
    {
        // Fade out
        yield return StartCoroutine(FadeToBlack());

        other.gameObject.SetActive(false);
        other.gameObject.transform.position = destination.transform.position;
        other.gameObject.SetActive(true);

        // Fade in
        yield return StartCoroutine(FadeToClear());
    }
    
 private IEnumerator FadeToBlack()
    {
        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, timeElapsed / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1); // black at the end
    }

    private IEnumerator FadeToClear()
    {
        yield return new WaitForSeconds(0.25f); // TODO: Disable this when all the rooms are no more close to each others

        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, timeElapsed / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 0); // transparent at the end
    }
    */
    public void SetLocked(bool locked){
        this.locked = locked;
    }
}
