using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Item> _items = new List<Item>();
    public static Inventory instance;
    private bool open = false;
    
    private void Start()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetButtonDown("OpenInventory"))
        {
            OpenInventory();
        }
    }

    public void AddItem(Item item)
    {
        _items.Add(item);
    }

    public Item[] GetItems()
    {
        return _items.ToArray();
    }

    void OpenInventory()
    {
        open = !open;
            
        Time.timeScale = open ? 0 : 1;
        GameController.instance.PauseGame(open);
            
        Item[] items = null;
        if (open)
            items = GetItems();
        UIController.instance.OpenInventory(open, items);
    }
}
