using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image itemImage;
    private Item item;
    public UnityEvent<Item> OnClick;
    
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
}
