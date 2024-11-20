using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[RequireComponent(typeof(SymbolSpeaker))]
public class ShopAnswer : MonoBehaviour, IInteractable
{
    [SerializeField] public GameObject canvas;
    [SerializeField] ItemDisplay itemDisplay;
    [SerializeField] Item[] shopItems;
    Language language;
    SymbolSpeaker speaker;
    Animator animator;
    static SymbolsComparer comparer = new();
    Dictionary<Symbol[], Meaning[]> answers = new(comparer);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SymbolsSelector.inputSymbolsEvent.AddListener(Answer);
        speaker = GetComponent<SymbolSpeaker>();
        animator = GetComponent<Animator>();
        canvas.SetActive(false);
        language = Language.instance;
        InitAnswers();
    }

    public void Answer(Symbol[] phrase){
        if (answers.ContainsKey(phrase)){
            speaker.Speak(SymbolSpeaker.PhraseType.CUSTOM, answers[phrase]);
            Deal(phrase);
        }
        else {
            speaker.Speak(SymbolSpeaker.PhraseType.CUSTOM, new Meaning[]{Meaning.QUESTION});
        }
        animator.SetTrigger("speak");
    }

    void InitAnswers(){
        answers.Add(language.GetSymbol("STRENGHT"), new Meaning[]{Meaning.STRENGHT, Meaning.QUESTION});
        answers.Add(language.GetSymbol("DEFENSE"), new Meaning[]{Meaning.DEFENSE, Meaning.QUESTION});
        answers.Add(language.GetSymbol("SPEED"), new Meaning[]{Meaning.SPEED, Meaning.QUESTION});

        answers.Add(new Symbol[]{
            language.GetSymbol("HERE")[0], 
            language.GetSymbol("OBJECT")[0]}, 
            new Meaning[]{Meaning.POSITIVE});

        answers.Add(language.GetSymbol("WEAPON") , new Meaning[]{Meaning.POSITIVE});
        answers.Add(language.GetSymbol("SHIELD") , new Meaning[]{Meaning.POSITIVE});
        answers.Add(language.GetSymbol("BOOTS") , new Meaning[]{Meaning.POSITIVE});
        answers.Add(language.GetSymbol("FRIEND") , new Meaning[]{Meaning.POSITIVE});
        answers.Add(language.GetSymbol("ENEMY") , new Meaning[]{Meaning.NEGATIVE});
    }

    public void Interact()
    {
        UIController.instance.OpenSymbolSelector(true);
    }

    void Deal(Symbol[] phrase){
        Item toDisplay = null;
        foreach (Item item in shopItems){
                print(item.GetType());
                if (item is Weapon && ArrayUtility.ArrayEquals(phrase, language.GetSymbol("WEAPON"))){
                    print("WEAPON");
                    toDisplay = item;
                    break;
                }
                if (item is Shield && ArrayUtility.ArrayEquals(phrase, language.GetSymbol("SHIELD"))){
                    print("SHIELD");
                    toDisplay = item;
                    break;
                }
                //SHOULD BE BOOTS
                if (item is Shield && ArrayUtility.ArrayEquals(phrase, language.GetSymbol("BOOTS"))){
                    toDisplay = item;
                    break;
                }
            }
        
        if (toDisplay){
            itemDisplay.SetItem(toDisplay);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
            canvas.SetActive(true);
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
            canvas.SetActive(false);
    }

}
