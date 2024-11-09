using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SymbolsSelector : MonoBehaviour
{
    public UnityEvent pressedSymbolEvent = new UnityEvent();
    public Symbol pressedSymbol;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetSymbols();
    }

    void SetSymbols()
    {
        UIController.instance.SetSymbols(Language.instance.symbols.ToArray(), PressedSymbol);
    }

    void PressedSymbol(Symbol symbol)
    {
        Debug.Log(symbol);
        pressedSymbol = symbol;
        pressedSymbolEvent.Invoke();
    }
}
