using UnityEngine;
using UnityEngine.InputSystem;

public class Muro : MonoBehaviour, IInteractable
{
    [SerializeField] float numberOfTries;
    public GameObject canvas;
    public GameObject secretDoor;
    private bool solved = false;

    //[SerializeField] TipoListaSimboliInput[] listaSimboliInput;
    //[SerializeField] TipoListaSimboliRiposta[] listaSimboliRiposta;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas.SetActive(false);
        secretDoor.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (solved)
        {
            secretDoor.SetActive(true);
        }
    }

    public void Interact()
    {
        if (!solved)
        {
            //if (numberOfTries > 0) {
            
                // prende la lista dei simboli dati in input dal player
                // se la lista dei simboli in input è uguale a quella dei simboli della risposta allora
                       
                       canvas.SetActive(false);
                       solved = true;
            //}
        }
    }
    private void OnTriggerEnter()
    {
        if (!solved)
        {
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!solved)
        {
            canvas.SetActive(false);
        }
    }
}

