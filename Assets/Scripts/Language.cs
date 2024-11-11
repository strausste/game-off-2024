using UnityEngine;
using System.Collections.Generic;

public class Language : MonoBehaviour
{
    public Sprite[] symbolImages; 
    //Symbols in the language
    public List<Symbol> symbols = new List<Symbol>();
    List<string> singleMeanings = new List<string>(){
        "START", 
        "ME",
        "YOU",
        "POSITIVE",
        "NEGATIVE",
        "STRENGHT",
        "DEFENSE",
        "SPEED",
        "OPEN",
        "HERE",
        "MONEY",
        //"DOOR",
        "CHEST",
        //"GO",
        //"ROOM",
        "SECRET",
        "OBJECT"
        // ?
    };

    List<string> multiMeanings = new List<string>(){
        "WEAPON", 
        "SHIELD",
        "BOOTS"
        // FRIEND = YOU POSITIVE
        // ENEMY = YOU NEGATIVE
        // SHOP = MONEY OBJECT o ROOM MONEY
        // KEY = OBJECT OPEN

    };
    
    public Dictionary<List<Symbol>, string> language = new Dictionary<List<Symbol>, string>();
    Dictionary<string, List<Symbol>> reversed_l = new Dictionary<string, List<Symbol>>();

    public static Language instance;
    
    void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        
        GenSymbols();
        SetMeanings();
    }

    void GenSymbols(){
        int i = 0;
        foreach (Sprite image in symbolImages) {
            symbols.Add(new Symbol(i, image));
            i++;
        }
    }

    void SetMeanings(){
        //Sets meanings in order
        int index = 0;
        List<Symbol> tmpSymbols = new List<Symbol>();
        tmpSymbols.AddRange(symbols);
        
        //Single Symbol Meanings
        foreach (string m in singleMeanings){
            List<Symbol> s = new List<Symbol>(){tmpSymbols[index]};
            tmpSymbols.RemoveAt(index);
            language.Add(s, m);
        }

        //Create reversed dictionary
        foreach (var l in language){
            reversed_l.Add(l.Value, l.Key);
        }

        //If OBJECT symbol is not set, return
        if (!reversed_l.ContainsKey("OBJECT")){
            Debug.LogError("[ERROR] Object symbol not found in dictionary");
            return;
        }
            
        //int obj = reversed_l["OBJECT"][0];
        Symbol obj = reversed_l["OBJECT"][0];

        //Multiple Symbol Meanings
        foreach (string m in multiMeanings){
            string stat;
            switch (m){
                case "WEAPON": stat = "STRENGHT";
                break;
                case "SHIELD": stat = "DEFENSE";
                break;
                case "BOOTS": stat = "SPEED";
                break;
                default: stat = "";
                break;
            }
            List<Symbol> sym = new List<Symbol>(){
                obj,
                reversed_l[stat][0]
            };

            language.Add(sym, m);
            List<Symbol> reversed = new List<Symbol>(){
                reversed_l[stat][0],
                obj
            };
            language.Add(reversed, m); 
            reversed_l.Add(m, sym);
        }
    }

    //Gets a meaning from a list of Symbols
    public string GetMeaning(List<Symbol> s){
        if (language.ContainsKey(s))
            return language[s];

        return null;
    }

    public List<Symbol> GetSymbol(string mean){
        if (reversed_l.ContainsKey(mean))
            return reversed_l[mean];
        
        return null;
    }
}