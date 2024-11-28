using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
    EntityStats playerStats;
    
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

    class SavedProperties {
        public List<Item> items;
        public int money;
        public Weapon weapon;
        public Shield shield;
        public Boots boots;
    }
    SavedProperties savedProperties;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("Destroying duplicate Inventory");
            Destroy(gameObject);
            return;
        }

        instance = this;
        Save();
        DontDestroyOnLoad(gameObject);
        
        CheatCodes.activatedCheat.AddListener(SetInfiniteMoney);

        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => InitPlayer();
        InitPlayer();
    }

    private void Update()
    {
        if (Input.GetButtonDown("OpenInventory"))
        {
            OpenInventory();
        }
    }

    /*public void InitPlayer(GameObject player)
    {
        this.player = player.GetComponent<PlayerController>();
        playerStats = player.GetComponent<EntityStats>();

        if (equippedWeapon) UseItem(equippedWeapon);
        if (equippedShield) UseItem(equippedShield);
        if (equippedBoots) UseItem(equippedBoots);
    }*/

    void InitPlayer()
    {
        if (!player){
            Debug.LogError("Player not found in scene");
            return;
        }
        player = FindFirstObjectByType<PlayerController>();
        playerStats = player.GetComponent<EntityStats>();

        if (equippedWeapon) UseItem(equippedWeapon);
        if (equippedShield) UseItem(equippedShield);
        if (equippedBoots) UseItem(equippedBoots);
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
            //Debug.Log("Equipped shield");
            equippedShield = (Shield)item;
            player.EquipShield(equippedShield);
        }
        else if (item.GetType() == typeof(Boots))
        {
            //Debug.Log("Equipped shield");
            equippedBoots = (Boots)item;
            player.EquipBoots(equippedBoots);
        }
        else if (item.GetType() == typeof(Potion))
        {
            var potion = (Potion)item;
            
            if (playerStats.GetHp() < playerStats.GetMaxHp())
            {
                //We could add other types of potions
                playerStats.IncreaseHp(potion.life);
                _items.Remove(item);
                UIController.instance.OpenInventory(true, _items.ToArray());    //Refresh UI
            }
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

    public void AddItems(Item []items)
    {
        var itemsList = items.ToList();
        
        var toAdd = itemsList.FindAll(i => i.GetType() != typeof(Money));
        
        _items.AddRange(items);
        
        var money = itemsList.FindAll(i => i.GetType() == typeof(Money));
        
        money.ForEach(m => this.money += ((Money)m).amount);
    }

    public void Save(){
        savedProperties = new SavedProperties();
        savedProperties.items = _items;
        savedProperties.money = money;
        savedProperties.weapon = equippedWeapon;
        savedProperties.shield = equippedShield;
        savedProperties.boots = equippedBoots;
    }

    public void Load(){
        _items = savedProperties.items;
        money = savedProperties.money;
        equippedWeapon = savedProperties.weapon;
        equippedShield = savedProperties.shield;
        equippedBoots = savedProperties.boots;
    }
}
