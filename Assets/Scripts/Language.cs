using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

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
    ROOM,
    SECRET,
    OBJECT,
    LIFE,
    ELEMENT,
    WEAPON,
    SHIELD,
    BOOTS,
    FRIEND,
    ENEMY,
    SHOP,
    POTION,
    FIRE,
    WATER,
    EARTH,
    AIR
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
        "GO", 
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
        "ROOM",
        "SECRET",
        "OBJECT",
        "LIFE",
        "ELEMENT"
    };

    List<string> multiMeanings = new List<string>(){
        "POTION",
        "WEAPON", 
        "SHIELD",
        "BOOTS",
        "FRIEND",
        "ENEMY",
        "SHOP",
        "FIRE",
        "EARTH",
        "WATER",
        "AIR"
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
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        
        GenSymbols();
        SetMeanings();
        //Randomize();
        //UIController.instance.InitUISymbols();
    }

    public void Randomize(){
        language.Clear();
        reversed_l.Clear();
        SetMeanings(true);
        //UIController.instance.InitUISymbols();
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

        #if UNITY_EDITOR
        foreach (var l in language)
        {
            var s = "";
            foreach (var key in l.Key)
            {
                s += key?.getId();
            }
            
            Debug.Log(s + ", " + l.Value);
        }
        Debug.Log("End of language");
        #endif
        
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
            
        Symbol first = reversed_l["GO"][0];
        Symbol second = reversed_l["GO"][0];

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
                case "POTION": 
                    first = reversed_l["OBJECT"][0];
                    second = reversed_l["LIFE"][0];
                    break;
                case "FIRE": 
                    first = reversed_l["ELEMENT"][0];
                    second = reversed_l["STRENGHT"][0];
                    break;
                case "EARTH": 
                    first = reversed_l["ELEMENT"][0];
                    second = reversed_l["DEFENSE"][0];
                    break;
                case "WATER": 
                    first = reversed_l["ELEMENT"][0];
                    second = reversed_l["LIFE"][0];
                    break;
                case "AIR": 
                    first = reversed_l["ELEMENT"][0];
                    second = reversed_l["SPEED"][0];
                    break;
            }
            Symbol[] sym = {first, second};
            
            try
            {
                language.Add(sym, m);
                reversed_l.Add(m, sym);
            }
            catch (Exception e)
            {
                Debug.Log(m);
                var s1 = $"{first}, ${second}";
                Debug.Log(s1);
                
                foreach (var l in language)
                {
                    var s = "";
                    foreach (var key in l.Key)
                    {
                        s += key?.getId();
                    }
                    
                    Debug.Log(s + ": "+ l.Value);
                }
            }

            Symbol[] reversed = {second, first};
            language.Add(reversed, m); 
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

    public Symbol[] GetSymbol(Meaning mean, bool reversed = false){
        //print("Cerca simbolo " + Enum.GetName(typeof(Meaning), mean));
        if (mean == Meaning.QUESTION) {
            return new Symbol[]{questionMark};
        }
        if (((int)mean) <= numbers.Count){
            return new Symbol[]{GetNumber((int)mean)};
        }
        if (reversed_l.ContainsKey(Enum.GetName(typeof(Meaning), mean))){
            if (reversed){
                Symbol[] symbols = reversed_l[Enum.GetName(typeof(Meaning), mean)];
                symbols = symbols.Reverse().ToArray();
                if (language.ContainsKey(symbols)) 
                    return symbols;
                print("Can't express the meaning " + Enum.GetName(typeof(Meaning), mean + " in 2 ways"));
            }
            return reversed_l[Enum.GetName(typeof(Meaning), mean)];
        }
            
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