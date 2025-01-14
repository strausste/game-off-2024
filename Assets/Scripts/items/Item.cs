using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public GameObject prefab;
    public string itemName;
    public Sprite sprite;  //Preview in inventory
    public int price;

}
