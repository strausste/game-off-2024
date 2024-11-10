using System;
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
        int minutes = (int)Time.time / 60;
        float seconds = Time.time %60f;
        
        timer.SetText($"{minutes}:{String.Format("{0:00.00}", seconds)}");
        
    }
}
