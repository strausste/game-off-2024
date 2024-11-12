using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject symbolPanel;
    public static UIController instance;
    [SerializeField] GameObject symbolPrefab;
    [SerializeField] TMP_Text timer;
    private float timerStart;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventoryContentPanel;
    private List<GameObject> inventoryChilds = new List<GameObject>();
    [SerializeField] private GameObject inventoryItemPrefab;
    
    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(this.gameObject);
        }
        
        instance = this;

        timerStart = Time.time;
        
        inventoryPanel.SetActive(false);
    }

    public void SetSymbols(Symbol[] symbols, Action<Symbol> onClick)
    {
        foreach (Symbol symbol in symbols)
        {
            GameObject symbolUi = Instantiate(symbolPrefab, symbolPanel.transform);

            symbolUi.name = symbol.getId().ToString();
            var image = symbolUi.GetComponent<Image>();
            image.sprite = symbol.getSprite();
            
            var button = symbolUi.GetComponent<Button>();
            button.onClick.AddListener(() => onClick.Invoke(symbol));
        }
    }

    private void Update()
    {
    }

    void FixedUpdate()
    {
        int minutes = (int)(Time.time - timerStart) / 60;
        float seconds = (Time.time - timerStart) %60f;
        
        timer.SetText($"{minutes}:{String.Format("{0:00.00}", seconds)}");
    }

    public void OpenInventory(bool open, Item []items = null)
    {
        inventoryPanel.SetActive(open);
        
        if (open)
        {
            foreach (var item in items)
            {
                GameObject inventoryItem = Instantiate(inventoryItemPrefab, inventoryContentPanel.transform);

                var itemUI = inventoryItem.GetComponent<InventoryItemUI>();
                itemUI.SetItem(item);
                
                inventoryChilds.Add(inventoryItem);
            }
        }
        else
        {
            foreach (var child in inventoryChilds)
            {
                //Please officer I can explain
                Destroy(child);
            }
            inventoryChilds.Clear();
        }
    }
}
