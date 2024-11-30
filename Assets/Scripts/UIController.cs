using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private GameObject[] statsSymbols;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject healthSymbol;
    [SerializeField] private GameObject moneyLayout;
    [SerializeField] private GameObject moneySymbol;
    [SerializeField] private GameObject settingsPanel;
    
    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(this.gameObject);
        }
        
        instance = this;

        
        inventoryPanel.SetActive(false);
    }

    private void Start() {
        healthSymbol.GetComponent<Image>().sprite = Language.instance.GetSymbol(Meaning.LIFE)[0].getSprite();
        moneySymbol.GetComponent<Image>().sprite = Language.instance.GetSymbol(Meaning.MONEY)[0].getSprite();
    }

    private void Update() {
        if (Input.GetButtonDown("Cancel"))
        {
            Pause(!pauseMenu.activeSelf);
        }
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

            foreach (Transform child in statsSymbols[0].transform){
                Destroy(child.gameObject);
            }
            foreach (Transform child in statsSymbols[1].transform){
                Destroy(child.gameObject);
            }
            foreach (Transform child in statsSymbols[2].transform){
                Destroy(child.gameObject);
            }

            EntityStats stats = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStats>();
            Language language = Language.instance;

            GameObject atkSymbol = Instantiate(symbolPrefab, statsSymbols[0].transform);
            GameObject atkNum = Instantiate(symbolPrefab, statsSymbols[0].transform);
            atkSymbol.GetComponent<Image>().sprite = language.GetSymbol(Meaning.STRENGHT)[0].getSprite();
            atkNum.GetComponent<Image>().sprite = language.GetSymbol((Meaning)stats.GetAttackLv())[0].getSprite();
            Destroy(atkSymbol.GetComponent<Button>());
            Destroy(atkNum.GetComponent<Button>());

            GameObject defSymbol = Instantiate(symbolPrefab, statsSymbols[1].transform);
            GameObject defNum = Instantiate(symbolPrefab, statsSymbols[1].transform);
            defSymbol.GetComponent<Image>().sprite = language.GetSymbol(Meaning.DEFENSE)[0].getSprite();
            defNum.GetComponent<Image>().sprite = language.GetSymbol((Meaning)stats.GetDefenseLv())[0].getSprite();
            Destroy(defSymbol.GetComponent<Button>());
            Destroy(defNum.GetComponent<Button>());

            GameObject spdSymbol = Instantiate(symbolPrefab, statsSymbols[2].transform);
            GameObject spdNum = Instantiate(symbolPrefab, statsSymbols[2].transform);
            spdSymbol.GetComponent<Image>().sprite = language.GetSymbol(Meaning.SPEED)[0].getSprite();
            spdNum.GetComponent<Image>().sprite = language.GetSymbol((Meaning)stats.GetSpeedLv())[0].getSprite();
            Destroy(spdSymbol.GetComponent<Button>());
            Destroy(spdNum.GetComponent<Button>());
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

    public void UpdateHealthBar(int max, int value){
        healthBar.maxValue = max;
        healthBar.value = value;
    }

    public void UpdateMoney(int amount){
        foreach (Transform child in moneyLayout.transform){
            Destroy(child.gameObject);
        }

        if (amount <= 0)
            return;

        int six = amount / 6;
        for (int i = 0; i < six; i++){
            GameObject symbol = Instantiate(symbolPrefab, moneyLayout.transform);
            symbol.GetComponent<Image>().sprite = Language.instance.GetSymbol(Meaning.SIX)[0].getSprite();
            symbol.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 50);
            Destroy(symbol.GetComponent<Button>());
        }

        GameObject rest = Instantiate(symbolPrefab, moneyLayout.transform);
        rest.GetComponent<Image>().sprite = Language.instance.GetSymbol((Meaning)(amount % 6))[0].getSprite();
        rest.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 50);
        Destroy(rest.GetComponent<Button>());
    }

    public void MainMenu(){
        Destroy(Language.instance.gameObject);
        Destroy(GameController.instance.gameObject);
        Destroy(Inventory.instance.gameObject);
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void OpenSettings(bool open)
    {
        settingsPanel.SetActive(open);
    }

    public void Pause(bool open)
    {
        if (open){
            pauseMenu.SetActive(true);
            settingsPanel.SetActive(false);
            Time.timeScale = 0;
        }
        else{
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
