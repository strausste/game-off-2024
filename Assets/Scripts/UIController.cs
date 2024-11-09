using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject symbolPanel;
    public static UIController instance;
    [SerializeField] GameObject symbolPrefab;
    
    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(this.gameObject);
        }
        
        instance = this;
    }

    public void SetSymbols(Symbol[] symbols, Action<Symbol> onClick)
    {
        foreach (Symbol symbol in symbols)
        {
            Debug.Log(symbol.getId());
            GameObject symbolUi = Instantiate(symbolPrefab, symbolPanel.transform);

            symbolUi.name = symbol.getId().ToString();
            var image = symbolUi.GetComponent<Image>();
            image.sprite = symbol.getSprite();
            
            var button = symbolUi.GetComponent<Button>();
            button.onClick.AddListener(() => onClick.Invoke(symbol));
            button.onClick.AddListener(() => Debug.Log("pressed button"));
        }
    }
}
