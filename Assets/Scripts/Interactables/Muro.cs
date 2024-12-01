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
    bool openSelector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        solved = false;
        canvas.SetActive(false);
        secretDoor.SetActive(false);

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
            if (numberOfTries > 0 && !openSelector) {
                // prende la lista dei simboli dati in input dal player
                UIController.instance.OpenSymbolSelector(true);
                SymbolsSelector.inputSymbolsEvent.AddListener(GetInputSymbols);
                openSelector = true;
            }
        }
    }

    public void GetInputSymbols(Symbol[] symbols)
    {
        Debug.Log("Get input muro");            

        listaStringInput.Add(Language.instance.GetMeaning(symbols));

        Control();
        listaStringInput.Clear();
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
            canvas.SetActive(false);
            solved = true;
            
            UIController.instance.OpenSymbolSelector(false);    
            SymbolsSelector.inputSymbolsEvent.RemoveAllListeners();
        }

        print("tentativi-1");
        numberOfTries--;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!solved && other.CompareTag("Player"))
        {
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!canvas.activeInHierarchy || !other.CompareTag("Player")){
            return;
        }

        openSelector = false;
        canvas.SetActive(false);            
        UIController.instance.OpenSymbolSelector(false);    
        SymbolsSelector.inputSymbolsEvent.RemoveAllListeners();
    }
}

