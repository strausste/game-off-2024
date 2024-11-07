using UnityEngine;
using System.Collections.Generic;

public class Language : MonoBehaviour
{
    public Texture2D[] symbolImages; 
    //Symbols in the language
    [SerializeField]
    List<Symbol> symbols = new List<Symbol>();
    List<string> singleMeanings = new List<string>(){
        "START", 
        "ME",
        "YOU",
        "YES",
        "NO",
        "STRENGHT",
        "DEFENSE",
        "SPEED",
        "OBJECT"
    };

    List<string> multiMeanings = new List<string>(){
        "WEAPON", 
        "SHIELD",
        "BOOTS"
    };
    
    Dictionary<List<int>, string> language = new Dictionary<List<int>, string>();
    Dictionary<string, List<int>> reversed_l = new Dictionary<string, List<int>>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        genSymbols();
        setMeanings();
    }

    void genSymbols(){
        int i = 0;
        foreach (Texture2D image in symbolImages) {
            symbols.Add(new Symbol(i, image));
            i++;
        }
    }

    void setMeanings(){
        //Sets meanings in order
        int index = 0;
        //Sets meanings randomly
        //int index = Rand

        //Single Symbol Meanings
        foreach (string m in singleMeanings){
            List<int> id = new List<int>(){symbols[index].getId()};
            symbols.RemoveAt(index);
            language.Add(id, m);
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
            
        int obj = reversed_l["OBJECT"][0];

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
            List<int> id = new List<int>(){
                obj,
                reversed_l[stat][0]
            };
            symbols.RemoveAt(index);

            language.Add(id, m);
            List<int> reversed = new List<int>(){
                reversed_l[stat][0],
                obj
            };
            language.Add(reversed, m); 
        }
    }

    //Gets a meaning from a list of Symbols
    public string getMeaning(List<Symbol> s){
        List<int> key = new List<int>();
        foreach (Symbol symbol in s){
            key.Add(symbol.getId());
        }

        if (language.ContainsKey(key)){
            return language[key];
        }

        return null;
    }
    
    //Gets a meaning from a list of Symbol ids
    public string getMeaningById(List<int> ids){
        if (language.ContainsKey(ids)){
            return language[ids];
        }
        return null;
    }
}
