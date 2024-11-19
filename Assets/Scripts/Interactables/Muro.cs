using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Muro : MonoBehaviour, IInteractable
{
    [SerializeField] float numberOfTries;
    public GameObject canvas;
    public GameObject secretDoor;
    private bool solved;

    private List<Symbol> listaSimboliInput = new List<Symbol>();
    private List<string> listaStringInput = new List<string>();

    [SerializeField] Meaning[] risposta;
    private List<string> listaStringRisposta = new List<string>();

    UnityEvent<Symbol[]> eventoSimboliInput = new UnityEvent<Symbol[]>();




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        solved = false;
        canvas.SetActive(false);
        secretDoor.SetActive(false);
        SymbolsSelector.inputSymbolsEvent.AddListener(GetInputSymbols);

        foreach (Meaning risp in risposta)
        {
            listaStringRisposta.Add(Enum.GetName(typeof(Meaning), risp));
        }
        
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
            if (numberOfTries > 0) {
                // prende la lista dei simboli dati in input dal player
                UIController.instance.OpenSymbolSelector(true);

            }
        }
    }

    public void GetInputSymbols(Symbol[] symbols)
    {
        
        listaStringInput.Add(Language.instance.HardGetMeaning(symbols));

        Control();
    }

    public void Control()
    {
        bool flag = true;
        for (int i = 0; i < listaStringRisposta.Count; i++)
        {

            if (!(listaStringInput[i] == listaStringRisposta[i]))
            {

                flag = false;
                break;
            }
            
        }
        if(flag == true)
        {
            canvas.SetActive(true);
            solved = true;
        }
        
        numberOfTries--;
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

