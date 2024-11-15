using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Item> _items = new List<Item>();
    public static Inventory instance;
    private bool open = false;
    
    [SerializeField]  Weapon equippedWeapon;
    public UnityEvent changedEquipment;
    
    public Weapon EquippedWeapon
    {
        get
        {
            return equippedWeapon;
        }
    }
    [SerializeField] Shield equippedShield;

    public Shield EquippedShield
    {
        get
        {
            return equippedShield;
        }
    }
    PlayerController player;
    
    private void Start()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("Destroying duplicate Inventory");
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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

    public void UseItem(Item item)
    {
        if (item.GetType() == typeof(Weapon))
        {
            Debug.Log("Equipped weapon");
            equippedWeapon = (Weapon)item;
            player.EquipWeapon(equippedWeapon);
        }
        else if (item.GetType() == typeof(Shield))
        {
            equippedShield = (Shield)item;
            player.EquipShield(equippedShield);
        }
        else
        {
            Debug.Log("Implement item use here");
            //We could just spawn the prefab attached to the object and handle the behaviour in there
        }
        changedEquipment.Invoke();
    }
}
