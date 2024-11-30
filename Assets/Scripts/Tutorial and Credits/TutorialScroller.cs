using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScroller : MonoBehaviour
{
    [SerializeField] GameObject[] imagesCanvas;
    [SerializeField] GameObject buttonCanvas;
    int activeCanvas = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonCanvas.SetActive(false);

        foreach (var image in imagesCanvas)
        {
            image.gameObject.SetActive(false);
        }
        imagesCanvas[0].SetActive(true);/**/
        
        StartCoroutine(WaitToPress());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            print("tasto premuto");
            if (activeCanvas < imagesCanvas.Length - 1)
            {
                print("esiste");
                imagesCanvas[activeCanvas].gameObject.SetActive(false);
                imagesCanvas[activeCanvas + 1].gameObject.SetActive(true);

                activeCanvas += 1;

                StartCoroutine(WaitToPress());
            }
            else
            {
                print("non esiste");
                SceneManager.LoadScene(2);
            }
        }


        // for(int i=0; i<imagesCanvas.Length; i++)
        // {
        //     print("INIZIO FOR");
        //     print(i);
        //     buttonCanvas.SetActive(false);      //dioooo

        //     if (buttonCanvas.activeSelf)
        //     {
        //         print("� attivo");
                
        //     }
        //     print("FINE FOR");
        // }

    }

    private IEnumerator WaitToPress()
    {
        buttonCanvas.SetActive(false);
        print("dentro");        

        yield return new WaitForSeconds(2f);

        buttonCanvas.SetActive(true);
        print("fuori");
    }
}
