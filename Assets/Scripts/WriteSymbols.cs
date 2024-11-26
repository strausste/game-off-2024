using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class WriteSymbols : MonoBehaviour
{
    [SerializeField] GameObject symbolPrefab;
    [SerializeField] HorizontalLayoutGroup layout;
    [SerializeField] Meaning[] meanings;
    Language language;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        language = Language.instance;
        writeSymbols();
    }

    void writeSymbols(){
        language = Language.instance;
        List<Symbol> symbols = new List<Symbol>();
        foreach (Meaning mean in meanings){
            symbols.AddRange(language.GetSymbol(mean));
        }

        foreach (Symbol symbol in symbols){
            GameObject instance = Instantiate(symbolPrefab, layout.transform);
            instance.GetComponent<Image>().sprite = symbol.getSprite();
            instance.GetComponent<Image>().color = Color.black;
        }
    }

    public void SetMeanings(Meaning[] newMeanings){
        meanings = newMeanings;
    }
}
