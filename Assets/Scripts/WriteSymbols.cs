using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class WriteSymbols : MonoBehaviour
{
    [SerializeField]
    GameObject symbolPrefab;
    [SerializeField]
    HorizontalLayoutGroup layout;
    [SerializeField]
    Meaning[] meanings;
    Language language;
    
    enum Meaning {
        START, 
        ME,
        YOU,
        POSITIVE,
        NEGATIVE,
        STRENGHT,
        DEFENSE,
        SPEED,
        OPEN,
        HERE,
        MONEY,
        //"DOOR",
        CHEST,
        //"GO",
        //"ROOM",
        SECRET,
        OBJECT
        // ?
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        language = Language.instance;
        writeSymbols();
    }

    void writeSymbols(){
        List<Symbol> symbols = new List<Symbol>();
        foreach (Meaning mean in meanings){
            symbols.AddRange(language.GetSymbol(Enum.GetName(typeof(Meaning), mean)));
        }

        foreach (Symbol symbol in symbols){
            //GameObject sprite = symbolPrefab.gameObject;
            GameObject instance = Instantiate(symbolPrefab, layout.transform);
            instance.GetComponent<Image>().sprite = symbol.getSprite();
        }
    }
}
