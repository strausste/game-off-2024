using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SymbolsSelector : MonoBehaviour
{
    public static UnityEvent<Symbol[]> inputSymbolsEvent = new UnityEvent<Symbol[]>();
    //If not empty filters symbols to leave only those that matter in this context
    [SerializeField] private List<string> possibleAnswers = new List<string>();
    private List<Symbol> symbols = new List<Symbol>();
    [SerializeField] private bool singleMode = false; //Select only one symbol
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        ClearSymbols();
    }

    void SetSymbols()
    {
        UIController.instance.SetSymbols(Language.instance.symbols.ToArray(), PressedSymbol);
    }

    void PressedSymbol(Symbol symbol)
    {
        if (singleMode)
        {
            symbols.Clear();
        }
        symbols.Add(symbol);
    }

    public void ConfirmSymbols()
    {
        inputSymbolsEvent.Invoke(symbols.ToArray());
        ClearSymbols();
        UIController.instance.OpenSymbolSelector(false);
    }

    public void ClearSymbols()
    {
        symbols.Clear();
        SetSymbols();
    }
}
