using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [SerializeField] Boots equippedBoots;

    public Boots EquippedBoots
    {
        get
        {
            return equippedBoots;
        }
    }

    PlayerController player;
    
    int money = 0;

    public int Money
    {
        get
        {
            return money;
        }
        set
        {
            money = value;
        }
    }
    
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
        
        CheatCodes.activatedCheat.AddListener(SetInfiniteMoney);

        if (equippedWeapon) UseItem(equippedWeapon);
        if (equippedShield) UseItem(equippedShield);
        if (equippedBoots) UseItem(equippedBoots);
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
            //Debug.Log("Equipped weapon");
            equippedWeapon = (Weapon)item;
            player.EquipWeapon(equippedWeapon);
        }
        else if (item.GetType() == typeof(Shield))
        {
            Debug.Log("Equipped shield");
            equippedShield = (Shield)item;
            player.EquipShield(equippedShield);
        }
        else if (item.GetType() == typeof(Boots))
        {
            equippedBoots = (Boots)item;
            player.EquipBoots(equippedBoots);
        }
        else
        {
            Debug.Log("Implement item use here");
            //We could just spawn the prefab attached to the object and handle the behaviour in there
        }
        changedEquipment.Invoke();
    }

    void SetInfiniteMoney()
    {
        if (GameController.instance.GetCheatCodes().getMoney)
        {
            money = 1000000000;
        }
    }
}
