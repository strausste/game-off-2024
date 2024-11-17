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
    private List<string> listaStringRisposta;

    UnityEvent<Symbol[]> eventoSimboliInput = new UnityEvent<Symbol[]>();
    UnityAction<Symbol[]> call;

    List<Symbol> temp;



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

        //Debug.Log(listaSimboliInput[0]);
    }

    public void Interact()
    {   
        
        if (!solved)
        {
            if (numberOfTries > 0) {
                // prende la lista dei simboli dati in input dal player
                UIController.instance.OpenSymbolSelector(true);
                print("fuori fuori");

            }
        }
    }

    public void GetInputSymbols(Symbol[] symbols)
    {
        print("Dentro");
        //Debug.Log(symbols[0].getId());

        List<int> ints = new List<int>();

        foreach (Symbol s in symbols)
        {
            ints.Add(s.getId());
        }

        listaStringInput.Add(Language.instance.GetMeaningByID(ints));

        Control();

    }

    public void Control()
    {
        print(listaStringInput[0]);
        print(listaStringRisposta[0]);
        if (listaStringInput.Equals(listaStringRisposta))
        {
            
            print("Corretto");
            //canvas.SetActive(false);
            //solved = true;
        }
        else
        {
            print("sbagliato");
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

