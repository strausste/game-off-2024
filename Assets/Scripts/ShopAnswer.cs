using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SymbolSpeaker))]
public class ShopAnswer : MonoBehaviour, IInteractable
{
    [SerializeField] public GameObject canvas;
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
            if (phrase.Equals(language.GetSymbol("WEAPON")) || phrase.Equals(language.GetSymbol("SHIELD")) ||
                phrase.Equals(language.GetSymbol("BOOTS"))){

                animator.SetTrigger("placeItem");
            }
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

        answers.Add(new Symbol[]{language.GetSymbol("HERE")[0], language.GetSymbol("OBJECT")[0]}, new Meaning[]{Meaning.POSITIVE});

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


    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
            canvas.SetActive(true);
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
            canvas.SetActive(false);
    }

}
