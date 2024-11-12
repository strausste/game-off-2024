using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image itemImage;
    
    public void SetItem(Item item)
    {
        itemImage.sprite = item.sprite;
        itemName.text = item.itemName;
    }
}
