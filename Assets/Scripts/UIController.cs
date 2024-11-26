using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject symbolSelector;
    [SerializeField] private GameObject symbolPanel;
    public static UIController instance;
    [SerializeField] GameObject symbolPrefab;
    [SerializeField] TMP_Text timer;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventoryContentPanel;
    private List<GameObject> inventoryChilds = new List<GameObject>();
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private GameObject attackStat;
    [SerializeField] private GameObject defenseStat;
    [SerializeField] private GameObject speedStat;
    
    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(this.gameObject);
        }
        
        instance = this;

        
        inventoryPanel.SetActive(false);
    }

    public void SetSymbols(Symbol[] symbols, Action<Symbol> onClick)
    {
        for (int i = 0; i < symbolPanel.transform.childCount; i++)
        {
            Destroy(symbolPanel.transform.GetChild(i).gameObject);   
        }
        
        foreach (Symbol symbol in symbols)
        {
            GameObject symbolUi = Instantiate(symbolPrefab, symbolPanel.transform);

            symbolUi.name = symbol.getId().ToString();
            var image = symbolUi.GetComponent<Image>();
            image.sprite = symbol.getSprite();
            
            var button = symbolUi.GetComponent<Button>();
            button.onClick.AddListener(() => onClick.Invoke(symbol));

            if (GameController.instance.GetCheatCodes().unlockMeanings)
            {
                var meaningText = symbolUi.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                var symbolList = new Symbol[]{};
                symbolList.Append(symbol);
                meaningText.gameObject.SetActive(true);
                meaningText.text = Language.instance.GetMeaning(symbolList);
            }
        }
    }

    void ClearInventory()
    {
        foreach (var child in inventoryChilds)
        {
            //Please officer I can explain
            Destroy(child);
        }
        inventoryChilds.Clear();
    }
    
    public void OpenInventory(bool open, Item []items = null)
    {
        ClearInventory();
        inventoryPanel.SetActive(open);
        
        if (open)
        {
            foreach (var item in items)
            {
                GameObject inventoryItem = Instantiate(inventoryItemPrefab, inventoryContentPanel.transform);

                var itemUI = inventoryItem.GetComponent<InventoryItemUI>();
                itemUI.SetItem(item);
                itemUI.OnClick.AddListener(ClickedItem);
                
                inventoryChilds.Add(inventoryItem);
            }

            EntityStats stats = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStats>();
            Language language = Language.instance;

            GameObject atkSymbol = Instantiate(symbolPrefab, attackStat.transform);
            GameObject atkNum = Instantiate(symbolPrefab, attackStat.transform);
            atkSymbol.GetComponent<Image>().sprite = language.GetSymbol(Meaning.STRENGHT)[0].getSprite();
            atkNum.GetComponent<Image>().sprite = language.GetSymbol((Meaning)stats.GetAttackLv())[0].getSprite();

            GameObject defSymbol = Instantiate(symbolPrefab, defenseStat.transform);
            GameObject defNum = Instantiate(symbolPrefab, defenseStat.transform);
            defSymbol.GetComponent<Image>().sprite = language.GetSymbol(Meaning.DEFENSE)[0].getSprite();
            defNum.GetComponent<Image>().sprite = language.GetSymbol((Meaning)stats.GetDefenseLv())[0].getSprite();

            GameObject spdSymbol = Instantiate(symbolPrefab, speedStat.transform);
            GameObject spdNum = Instantiate(symbolPrefab, speedStat.transform);
            spdSymbol.GetComponent<Image>().sprite = language.GetSymbol(Meaning.SPEED)[0].getSprite();
            spdNum.GetComponent<Image>().sprite = language.GetSymbol((Meaning)stats.GetSpeedLv())[0].getSprite();
        }
        else {
            foreach (Transform child in attackStat.transform){
                Destroy(child.gameObject);
            }
            foreach (Transform child in defenseStat.transform){
                Destroy(child.gameObject);
            }
            foreach (Transform child in speedStat.transform){
                Destroy(child.gameObject);
            }
        }
    }

    void ClickedItem(Item item)
    {
        Inventory.instance.UseItem(item);
    }

    public void OpenSymbolSelector(bool open)
    {
        symbolSelector.SetActive(open);
    }

    public void SetTimer(float timeSinceStart)
    {
        int minutes = (int)timeSinceStart / 60;
        float seconds = timeSinceStart % 60f;
        
        timer.SetText($"{minutes}:{String.Format("{0:00.00}", seconds)}");
    }
}
