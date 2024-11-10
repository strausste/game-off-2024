using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SymbolsSelector : MonoBehaviour
{
    public UnityEvent<Symbol> pressedSymbolEvent = new UnityEvent<Symbol>();
    public Symbol pressedSymbol;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetSymbols();
        pressedSymbolEvent.AddListener((symbol) => { Debug.Log("Symbol pressed "+ symbol.getId()); });
    }

    void SetSymbols()
    {
        UIController.instance.SetSymbols(Language.instance.symbols.ToArray(), PressedSymbol);
    }

    void PressedSymbol(Symbol symbol)
    {
        pressedSymbol = symbol;
        pressedSymbolEvent.Invoke(symbol);
    }
}
