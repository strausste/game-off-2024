using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(SymbolSpeaker))]
public class ShopAnswer : MonoBehaviour, IInteractable
{
    [SerializeField] public GameObject canvas;
    [SerializeField] ItemDisplay itemDisplay;
    [SerializeField] Item[] shopItems;
    [SerializeField] PlayerController player;
    Language language;
    SymbolSpeaker speaker;
    Animator animator;
    static SymbolsComparer comparer = new();
    Dictionary<Symbol[], Meaning[]> answers = new(comparer);
    Item toDisplay = null;
    bool dealing = false;

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
        if (dealing){
            if (phrase.SequenceEqual(language.GetSymbol(Meaning.POSITIVE))){
                player.Equip(toDisplay);

                dealing = false;
                itemDisplay.Clear();
            }
            else if (phrase.SequenceEqual(language.GetSymbol(Meaning.NEGATIVE))){
                dealing = false;
                itemDisplay.Clear();
            }
            else{
                speaker.Speak(SymbolSpeaker.PhraseType.CUSTOM, new Meaning[]{Meaning.QUESTION});
                UIController.instance.OpenSymbolSelector(true);
            }
        }
        else{
            if (answers.ContainsKey(phrase)){
                speaker.Speak(SymbolSpeaker.PhraseType.CUSTOM, answers[phrase]);
                ShowRelativeItem(phrase);
            }
            else {
                speaker.Speak(SymbolSpeaker.PhraseType.CUSTOM, new Meaning[]{Meaning.QUESTION});
            }
            animator.SetTrigger("speak");
        }
    }

    void InitAnswers(){
        answers.Add(language.GetSymbol(Meaning.STRENGHT), new Meaning[]{Meaning.STRENGHT, Meaning.QUESTION});
        answers.Add(language.GetSymbol(Meaning.DEFENSE), new Meaning[]{Meaning.DEFENSE, Meaning.QUESTION});
        answers.Add(language.GetSymbol(Meaning.SPEED), new Meaning[]{Meaning.SPEED, Meaning.QUESTION});
        answers.Add(language.GetSymbol(Meaning.WEAPON) , new Meaning[]{Meaning.MONEY, Meaning.QUESTION});
        answers.Add(language.GetSymbol(Meaning.SHIELD) , new Meaning[]{Meaning.MONEY, Meaning.QUESTION});
        answers.Add(language.GetSymbol(Meaning.BOOTS) , new Meaning[]{Meaning.MONEY, Meaning.QUESTION});
        answers.Add(language.GetSymbol(Meaning.FRIEND) , new Meaning[]{Meaning.POSITIVE});
        answers.Add(language.GetSymbol(Meaning.ENEMY) , new Meaning[]{Meaning.NEGATIVE});

        answers.Add(new Symbol[]{
            language.GetSymbol(Meaning.HERE)[0], 
            language.GetSymbol(Meaning.OBJECT)[0]}, 
            new Meaning[]{Meaning.POSITIVE});

    }

    public void Interact()
    {
        UIController.instance.OpenSymbolSelector(true);
    }

    void ShowRelativeItem(Symbol[] phrase){
        foreach (Item item in shopItems){
                print(item.GetType());
                if (item is Weapon && phrase.SequenceEqual(language.GetSymbol(Meaning.WEAPON))){
                    toDisplay = item;
                    break;
                }
                if (item is Shield && phrase.SequenceEqual(language.GetSymbol(Meaning.SHIELD))){
                    toDisplay = item;
                    break;
                }
                if (item is Boots && phrase.SequenceEqual(language.GetSymbol(Meaning.BOOTS))){
                    toDisplay = item;
                    break;
                }
            }
        
        if (toDisplay){
            itemDisplay.SetItem(toDisplay);
            //Non so perché ma se non metto il desync non si apre
            Invoke("Deal", 0.1f);
        }
    }

    void Deal(){
        UIController.instance.OpenSymbolSelector(true);
        dealing = true;
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
