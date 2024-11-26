using UnityEngine;
using System.Collections.Generic;
using System;

public enum Meaning {
    QUESTION,
    ONE,
    TWO,
    THREE,
    FOUR,
    FIVE,
    SIX,
    GO, 
    ME,
    YOU,
    POSITIVE,
    NEGATIVE,
    STRENGHT,
    DEFENSE,
    SPEED,
    //OPEN,
    HERE,
    MONEY,
    //"DOOR",
    CHEST,
    //"GO",
    //"ROOM",
    SECRET,
    OBJECT,
    WEAPON,
    SHIELD,
    BOOTS,
    FRIEND,
    ENEMY,
    SHOP
};

public class Language : MonoBehaviour
{
    [SerializeField] Sprite[] symbolImages;
    [SerializeField] Sprite[] numbersImages;
    [SerializeField] Sprite questionSprite;
    //Symbols in the language
    public List<Symbol> symbols = new List<Symbol>();
    List<Symbol>numbers = new List<Symbol>();
    Symbol questionMark;
    List<string> singleMeanings = new List<string>(){
        "START", 
        "ME",
        "YOU",
        "POSITIVE",
        "NEGATIVE",
        "STRENGHT",
        "DEFENSE",
        "SPEED",
        //"OPEN",
        "HERE",
        "MONEY",
        //"DOOR",
        "CHEST",
        //"GO",
        "ROOM",
        "SECRET",
        "OBJECT"
    };

    List<string> multiMeanings = new List<string>(){
        "WEAPON", 
        "SHIELD",
        "BOOTS",
        "FRIEND",
        "ENEMY",
        "SHOP"
        // KEY = OBJECT OPEN
    };

    static SymbolsComparer comparer = new SymbolsComparer();
    Dictionary<Symbol[], string> language = new(comparer);
    Dictionary<string, Symbol[]> reversed_l = new Dictionary<string, Symbol[]>();

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
        //Randomize();
    }

    public void Randomize(){
        language.Clear();
        reversed_l.Clear();
        SetMeanings(true);
    }

    void GenSymbols(){
        int i = 0;

        //Adds question Mark
        questionMark = new Symbol(i, questionSprite);
        i++;

        //Adds Numbers
        foreach (Sprite image in numbersImages) {
            numbers.Add(new Symbol(i, image));
            i++;
        }
        //Adds Characters
        foreach (Sprite image in symbolImages) {
            symbols.Add(new Symbol(i, image));
            i++;
        }
    }

    void SetMeanings(bool random = false){
        //Sets meanings in order
        int index = 0;
        List<Symbol> tmpSymbols = new List<Symbol>(symbols);
        List<Symbol> currentSymbol = new List<Symbol>();
        
        //Single Symbol Meanings
        foreach (string m in singleMeanings){
            Symbol[] s = new Symbol[1]; 
            if (random)
                index = UnityEngine.Random.Range(0, tmpSymbols.Count);

            s[0] = tmpSymbols[index];
            currentSymbol.Add(tmpSymbols[index]);
            tmpSymbols.RemoveAt(index);
            language.Add(s, m);
        }

        //Removes the symbols excluded in this run
        symbols = currentSymbol;

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
        Symbol first = reversed_l["START"][0];
        Symbol second = reversed_l["START"][0];

        //Multiple Symbol Meanings
        foreach (string m in multiMeanings){
            switch (m){
                case "WEAPON": 
                    first = reversed_l["OBJECT"][0];
                    second = reversed_l["STRENGHT"][0];
                    break;
                case "SHIELD": 
                    first = reversed_l["OBJECT"][0];
                    second = reversed_l["DEFENSE"][0];
                    break;
                case "BOOTS":
                    first = reversed_l["OBJECT"][0];
                    second = reversed_l["SPEED"][0];
                    break;
                case "FRIEND":
                    first = reversed_l["YOU"][0];
                    second = reversed_l["POSITIVE"][0];
                    break;
                case "ENEMY":
                    first = reversed_l["YOU"][0];
                    second = reversed_l["NEGATIVE"][0];
                    break;
                case "SHOP":
                    first = reversed_l["MONEY"][0];
                    second = reversed_l["ROOM"][0];
                    break;
            }
            Symbol[] sym = {first, second};
            language.Add(sym, m);

            Symbol[] reversed = {second, first};
            language.Add(reversed, m); 
            reversed_l.Add(m, sym);
        }
    }

    //Gets a meaning from a list of Symbols
    public string GetMeaning(Symbol[] s){
        if (language.ContainsKey(s))
        {
            return language[s];
        }

         return null;
    }

    public Symbol[] GetSymbol(Meaning mean){
        if (mean == Meaning.QUESTION) {
            return new Symbol[]{questionMark};
        }
        if (((int)mean) <= numbers.Count){
            return new Symbol[]{GetNumber((int)mean)};
        }
        if (reversed_l.ContainsKey(Enum.GetName(typeof(Meaning), mean)))
            return reversed_l[Enum.GetName(typeof(Meaning), mean)];
        
        Debug.LogError("No symbol of meaning: " + mean + " found");
        return null;
    }

    public Symbol GetNumber(int number){
        if (number <= numbers.Count){
            return numbers[number-1];
        }
        Debug.LogError("Symbol of number " + number + " doesn't exist");
        return numbers[0];
    }
}