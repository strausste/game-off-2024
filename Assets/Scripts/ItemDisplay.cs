using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] int itemTilt = 30; 
    [SerializeField] int itemScale = 2; 
    GameObject itemInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (item){
            DisplayItem();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (itemInstance){
            itemInstance.transform.RotateAround(transform.position, transform.up, 1);
        }
    }

    public Item GetItem(){
        return item;
    }

    public void SetItem(Item item){
        this.item = item;
        DisplayItem();
    }

    public void Clear(){
        if (itemInstance){
            Destroy(itemInstance);
        }
    }

    void DisplayItem(){
        Clear();
        itemInstance = Instantiate(item.prefab, transform);
        itemInstance.transform.localScale *= itemScale;
        itemInstance.transform.Rotate(Vector3.forward * itemTilt);
    }

}
