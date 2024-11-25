using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject destination;
    [SerializeField] private float fadeDuration = 1.5f;

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

        // Teleport the player, -Ste: TODO - disable player's CC (by caching it) 
        //other.gameObject.SetActive(false);
        other.gameObject.transform.position = destination.transform.position;
        //other.gameObject.SetActive(true);

        // Fade in
        yield return StartCoroutine(FadeToClear());
    }

    private IEnumerator FadeToBlack()
    {
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = t / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1); // black at the end
    }

    private IEnumerator FadeToClear()
    {
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = 1 - (t / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 0); // transparent at the end
    }
}
