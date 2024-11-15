using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image itemImage;
    [SerializeField] GameObject equippedItem;
    private Item item;
    public UnityEvent<Item> OnClick;

    private void Start()
    {
        Inventory.instance.changedEquipment.AddListener(SetEquippedItem);
        SetEquippedItem();
    }

    public void SetItem(Item item)
    {
        itemImage.sprite = item.sprite;
        itemName.text = item.itemName;
        
        this.item = item;
    }

    public void ClickedItem()
    {
        OnClick.Invoke(item);
    }

    void SetEquippedItem()
    {
        bool isEquipped = Inventory.instance.EquippedShield == item ||
                          Inventory.instance.EquippedWeapon == item;
        
        equippedItem.SetActive(isEquipped);
    }
}
