using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class NextSceneDoor : MonoBehaviour
{
    [SerializeField] string nextScene;

    [SerializeField] private float fadeDuration;
    
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

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TeleportWithFade(other));
        }
    }

    private IEnumerator TeleportWithFade(Collider other)
    {
        // Fade out
        yield return StartCoroutine(FadeToBlack());

        //cambio scena
        GameController.instance.LoadLevel(nextScene);

        // Fade in
        //yield return StartCoroutine(FadeToClear());    //non funziona, bisognerebbe chiamarlo nella nuova scena, gana
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
}
