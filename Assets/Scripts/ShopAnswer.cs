using UnityEngine;
using System.Collections.Generic;

public class ShopAnswer : MonoBehaviour
{
    Language language;
    Dictionary<List<Symbol>, List<Symbol>> answers = new Dictionary<List<Symbol>, List<Symbol>>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        language = FindFirstObjectByType<Language>();
        initAnswers();
    }

    public List<Symbol> Answer(List<Symbol> phrase){
        List<Symbol> ans = new List<Symbol>(){language.symbols[^1]};
        if (answers.ContainsKey(phrase)){
            return answers[phrase];
        }
        return ans;
    }

    void initAnswers(){
        answers.Add(language.GetSymbol("STRENGHT"), new List<Symbol>(language.GetSymbol("STRENGHT")){language.symbols[^1]});
        answers.Add(language.GetSymbol("DEFENSE"), new List<Symbol>(language.GetSymbol("DEFENSE")){language.symbols[^1]});
        answers.Add(language.GetSymbol("SPEED"), new List<Symbol>(language.GetSymbol("SPEED")){language.symbols[^1]});

        answers.Add(new List<Symbol>(language.GetSymbol("HERE")){language.GetSymbol("OBJECT")[0]}, language.GetSymbol("POSITIVE"));

        answers.Add(language.GetSymbol("WEAPON") , language.GetSymbol("POSITIVE"));
        answers.Add(language.GetSymbol("SHIELD") , language.GetSymbol("POSITIVE"));
        answers.Add(language.GetSymbol("BOOTS") , language.GetSymbol("POSITIVE"));
    }
}
