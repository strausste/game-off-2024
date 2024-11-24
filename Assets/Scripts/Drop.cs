using System;
using UnityEngine;

public class Drop : MonoBehaviour
{
    private Item[] drops;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory.instance.AddItems(drops);
            AudioManager.instance.PlaySound("GotDrop");
            Destroy(gameObject);
        }
    }

    public void SetDrop(Item []drops)
    {
        this.drops = drops;
    }
}
