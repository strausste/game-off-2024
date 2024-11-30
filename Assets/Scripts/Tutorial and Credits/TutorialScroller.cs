using System.Collections;
using UnityEngine;

public class TutorialScroller : MonoBehaviour
{
    [SerializeField] GameObject[] imagesCanvas;
    [SerializeField] GameObject buttonCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonCanvas.SetActive(false);

        foreach (var image in imagesCanvas)
        {
            image.gameObject.SetActive(false);
        }
        imagesCanvas[0].SetActive(true);/**/
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<imagesCanvas.Length; i++)
        {
            print("INIZIO FOR");
            print(i);
            StartCoroutine(WaitToPress());

            if (buttonCanvas.activeSelf)
            {
                print("è attivo");
                if (Input.GetKeyDown(KeyCode.E))
                {
                    print("tasto premuto");
                    if (imagesCanvas[i + 1])
                    {
                        print("esiste");
                        imagesCanvas[i].gameObject.SetActive(false);
                        imagesCanvas[i + 1].gameObject.SetActive(true);
                    }
                    else
                    {
                        print("non esiste");
                        GameController.instance.LoadLevel("Livello 1");
                    }
                }
            }
            print("FINE FOR");
        }

    }

    private IEnumerator WaitToPress()
    {
        print("dentro");
        buttonCanvas.SetActive(false);

        yield return new WaitForSeconds(2f);

        buttonCanvas.SetActive(true);
        print("fuori");
    }
}
