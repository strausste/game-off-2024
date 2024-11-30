using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour
{
    [SerializeField] GameObject buttonCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(WaitToPress());
        if (Input.GetKeyDown(KeyCode.E))
            SceneManager.LoadScene("Main Menu");
    }

    private IEnumerator WaitToPress()
    {
        yield return new WaitForSeconds(2f);

        buttonCanvas.SetActive(true);
    }
}
