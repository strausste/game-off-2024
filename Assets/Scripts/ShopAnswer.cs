using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(SymbolSpeaker))]
public class ShopAnswer : MonoBehaviour, IInteractable
{
    [SerializeField] public GameObject canvas;
    [SerializeField] ItemDisplay itemDisplay;
    [SerializeField] Item[] shopItems;
    PlayerController player;
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
        player = FindFirstObjectByType<PlayerController>();
        speaker = GetComponent<SymbolSpeaker>();
        animator = GetComponent<Animator>();
        canvas.SetActive(false);
        language = Language.instance;
        InitAnswers();
    }

    public void Answer(Symbol[] phrase){
        if (dealing){
            if (phrase.SequenceEqual(language.GetSymbol(Meaning.POSITIVE))){
                //Player wants to Buy
                if(Inventory.instance.Money >= toDisplay.price){
                    //Player has needed money
                    Inventory.instance.IncMoney(-toDisplay.price);
                    player.Equip(toDisplay);
                    speaker.Speak(new Meaning[]{Meaning.OBJECT, Meaning.HERE});
                }
                else{
                    //Player has not needed money
                    speaker.Speak(new Meaning[]{Meaning.MONEY, Meaning.NEGATIVE});
                }

                animator.SetTrigger("trade");
                CancelDeal();
            }
            else if (phrase.SequenceEqual(language.GetSymbol(Meaning.NEGATIVE))){
                //Player doesn't want to buy
                CancelDeal();
            }
            else{
                speaker.Speak(new Meaning[]{Meaning.QUESTION});
                UIController.instance.OpenSymbolSelector(true);
            }
        }
        else{
            if (answers.ContainsKey(phrase)){
                if (ShowRelativeItem(phrase))
                    return;
                speaker.Speak(answers[phrase]);
            }
            else {
                speaker.Speak(new Meaning[]{Meaning.QUESTION});
            }
            animator.SetTrigger("speak");
        }
    }

    void InitAnswers(){
        //Buyable objects will bypass the answer
        answers.Add(language.GetSymbol(Meaning.STRENGHT), new Meaning[]{Meaning.STRENGHT, Meaning.QUESTION});
        answers.Add(language.GetSymbol(Meaning.DEFENSE), new Meaning[]{Meaning.DEFENSE, Meaning.QUESTION});
        answers.Add(language.GetSymbol(Meaning.SPEED), new Meaning[]{Meaning.SPEED, Meaning.QUESTION});
        answers.Add(language.GetSymbol(Meaning.WEAPON) , new Meaning[]{});
        answers.Add(language.GetSymbol(Meaning.WEAPON, true) , new Meaning[]{});
        answers.Add(language.GetSymbol(Meaning.SHIELD) , new Meaning[]{});
        answers.Add(language.GetSymbol(Meaning.SHIELD, true) , new Meaning[]{});
        answers.Add(language.GetSymbol(Meaning.BOOTS) , new Meaning[]{});
        answers.Add(language.GetSymbol(Meaning.BOOTS, true) , new Meaning[]{});
        answers.Add(language.GetSymbol(Meaning.POTION) , new Meaning[]{});
        answers.Add(language.GetSymbol(Meaning.POTION, true) , new Meaning[]{});
        answers.Add(language.GetSymbol(Meaning.FRIEND) , new Meaning[]{Meaning.POSITIVE});
        answers.Add(language.GetSymbol(Meaning.FRIEND, true) , new Meaning[]{Meaning.POSITIVE});
        answers.Add(language.GetSymbol(Meaning.ENEMY) , new Meaning[]{Meaning.NEGATIVE});
        answers.Add(language.GetSymbol(Meaning.ENEMY, true) , new Meaning[]{Meaning.NEGATIVE});
        answers.Add(language.GetSymbol(Meaning.ME) , new Meaning[]{Meaning.YOU, Meaning.QUESTION});
        answers.Add(language.GetSymbol(Meaning.YOU) , new Meaning[]{Meaning.ME, Meaning.QUESTION});
        answers.Add(language.GetSymbol(Meaning.SHOP) , new Meaning[]{Meaning.SHOP, Meaning.HERE});
        answers.Add(language.GetSymbol(Meaning.SHOP, true) , new Meaning[]{Meaning.SHOP, Meaning.HERE});
        answers.Add(language.GetSymbol(Meaning.HERE) , new Meaning[]{Meaning.HERE, Meaning.SHOP});
        answers.Add(language.GetSymbol(Meaning.MONEY) , new Meaning[]{Meaning.MONEY, Meaning.MONEY, Meaning.MONEY});

        answers.Add(new Symbol[]{
            language.GetSymbol(Meaning.HERE)[0], language.GetSymbol(Meaning.OBJECT)[0]}, 
            new Meaning[]{Meaning.POSITIVE});
        answers.Add(new Symbol[]{
            language.GetSymbol(Meaning.OBJECT)[0], language.GetSymbol(Meaning.HERE)[0]}, 
            new Meaning[]{Meaning.POSITIVE});
        answers.Add(new Symbol[]{
            language.GetSymbol(Meaning.MONEY)[0], language.GetSymbol(Meaning.HERE)[0]}, 
            new Meaning[]{Meaning.NEGATIVE});
        answers.Add(new Symbol[]{
            language.GetSymbol(Meaning.HERE)[0], language.GetSymbol(Meaning.MONEY)[0]}, 
            new Meaning[]{Meaning.NEGATIVE});
        answers.Add(new Symbol[]{
            language.GetSymbol(Meaning.YOU)[0], language.GetSymbol(Meaning.GO)[0]}, 
            new Meaning[]{Meaning.NEGATIVE});
        answers.Add(new Symbol[]{
            language.GetSymbol(Meaning.ME)[0], language.GetSymbol(Meaning.GO)[0]}, 
            new Meaning[]{Meaning.POSITIVE});
        answers.Add(language.GetSymbol(Meaning.WEAPON).Concat(language.GetSymbol(Meaning.HERE)).ToArray(), 
            new Meaning[]{Meaning.POSITIVE});
            answers.Add(language.GetSymbol(Meaning.WEAPON, true).Concat(language.GetSymbol(Meaning.HERE)).ToArray(), 
            new Meaning[]{Meaning.POSITIVE});
        answers.Add(language.GetSymbol(Meaning.SHIELD).Concat(language.GetSymbol(Meaning.HERE)).ToArray(), 
            new Meaning[]{Meaning.POSITIVE});
        answers.Add(language.GetSymbol(Meaning.SHIELD, true).Concat(language.GetSymbol(Meaning.HERE)).ToArray(), 
            new Meaning[]{Meaning.POSITIVE});
        answers.Add(language.GetSymbol(Meaning.BOOTS).Concat(language.GetSymbol(Meaning.HERE)).ToArray(), 
            new Meaning[]{Meaning.POSITIVE});
        answers.Add(language.GetSymbol(Meaning.BOOTS, true).Concat(language.GetSymbol(Meaning.HERE)).ToArray(), 
            new Meaning[]{Meaning.POSITIVE});
        answers.Add(language.GetSymbol(Meaning.POTION).Concat(language.GetSymbol(Meaning.HERE)).ToArray(), 
            new Meaning[]{Meaning.POSITIVE});
        answers.Add(language.GetSymbol(Meaning.POTION, true).Concat(language.GetSymbol(Meaning.HERE)).ToArray(), 
            new Meaning[]{Meaning.POSITIVE});
        answers.Add(new Symbol[]{
            language.GetSymbol(Meaning.ME)[0], language.GetSymbol(Meaning.STRENGHT)[0]}, 
            new Meaning[]{Meaning.YOU, Meaning.STRENGHT});
        answers.Add(new Symbol[]{
            language.GetSymbol(Meaning.ME)[0], language.GetSymbol(Meaning.DEFENSE)[0]}, 
            new Meaning[]{Meaning.YOU, Meaning.DEFENSE});
        answers.Add(new Symbol[]{
            language.GetSymbol(Meaning.ME)[0], language.GetSymbol(Meaning.SPEED)[0]}, 
            new Meaning[]{Meaning.YOU, Meaning.SPEED});
    }

    public void Interact()
    {
        SymbolsSelector.inputSymbolsEvent.RemoveAllListeners();
        SymbolsSelector.inputSymbolsEvent.AddListener(Answer);
        UIController.instance.OpenSymbolSelector(true);
    }

    bool ShowRelativeItem(Symbol[] phrase){
        EntityStats stats = player.GetComponent<EntityStats>();

        bool foundFlag = false;
        foreach (Item item in shopItems){
                if (item is Weapon && language.GetMeaning(phrase) == "WEAPON"){
                    foundFlag = true;
                    Weapon weapon = (Weapon) item;
                    if (weapon.attack == stats.GetAttackLv() + 1){
                        toDisplay = item;
                        break;
                    }
                }
                if (item is Shield && language.GetMeaning(phrase) == "SHIELD"){
                    foundFlag = true;
                    Shield shield = (Shield) item;
                    if (shield.damageProtection == stats.GetDefenseLv() + 1){
                        toDisplay = item;
                        break;
                    }
                }
                if (item is Boots && language.GetMeaning(phrase) == "BOOTS"){
                    foundFlag = true;
                    Boots boots = (Boots) item;
                    if (boots.speed == stats.GetSpeedLv() + 1){
                        toDisplay = item;
                        break;
                    }
                }
    
                if (item is Potion && language.GetMeaning(phrase) == "POTION"){
                    foundFlag = true;
                    toDisplay = item;
                    break;
                }
            }
        
        if (toDisplay){
            itemDisplay.SetItem(toDisplay);
            //Non so perché ma se non metto il desync non si apre
            Invoke("Deal", 0.1f);

            List<Meaning> totPrice = new List<Meaning>();
            int n = toDisplay.price / 6;
            for (int i = 0; i < n; i++){
                totPrice.Add(Meaning.SIX);
            }
            
            if(toDisplay.price % 6 != 0){
                totPrice.Add((Meaning)(toDisplay.price % 6));
            }
            totPrice.Add(Meaning.MONEY);

            animator.SetTrigger("speak");
            speaker.Speak(totPrice.ToArray(), 5);
            return true;
        }
        else if (foundFlag){
            animator.SetTrigger("speak");
            speaker.Speak(new Meaning[]{Meaning.NEGATIVE, Meaning.OBJECT});
            return true;
        }
        else{
            return false;
        }
    }

    void Deal(){
        UIController.instance.OpenSymbolSelector(true);
        SymbolsSelector.inputSymbolsEvent.RemoveAllListeners();
        SymbolsSelector.inputSymbolsEvent.AddListener(Answer);
        dealing = true;
    }

    void CancelDeal(){
        SymbolsSelector.inputSymbolsEvent.RemoveAllListeners();
        dealing = false;
        toDisplay = null;
        itemDisplay.Clear();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out PlayerController player))
            canvas.SetActive(true);
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.TryGetComponent(out PlayerController player)){
            canvas.SetActive(false);
            SymbolsSelector.inputSymbolsEvent.RemoveAllListeners();
            UIController.instance.OpenSymbolSelector(false);
        }
    }

}
