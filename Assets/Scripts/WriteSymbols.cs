using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WriteSymbols : MonoBehaviour
{
    [SerializeField]
    GameObject symbolPrefab;
    [SerializeField]
    HorizontalLayoutGroup layout;
    [SerializeField]
    string[] meanings;
    Language language;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        language = Language.instance;
        writeSymbols();
    }

    void writeSymbols(){
        List<Symbol> symbols = new List<Symbol>();
        foreach (string mean in meanings){
            symbols.AddRange(language.GetSymbol(mean));
        }

        foreach (Symbol symbol in symbols){
            //GameObject sprite = symbolPrefab.gameObject;
            GameObject instance = Instantiate(symbolPrefab, layout.transform);
            instance.GetComponent<Image>().sprite = symbol.getSprite();
        }
    }
}
