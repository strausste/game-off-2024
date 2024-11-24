using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using Random = UnityEngine.Random;

public class Loot : MonoBehaviour
{
    [SerializeField] ItemAndProbability[] possibleDrops;
    [SerializeField] private int maxItemDrops = 0; //If zero no limits
    [SerializeField] GameObject lootPrefab; //Dropbag in prefabs
    
    private void OnDestroy()
    {
        DropItems();
    }

    public void DropItems()
    {
        List<ItemAndProbability> droppedItems = new List<ItemAndProbability>();    
        
        foreach (var drop in possibleDrops)
        {
            float rand = Random.Range(0f, 1f);

            if (rand < drop.probability)
            {
                droppedItems.Add(drop);
            }
        }

        //Dropped more common items if more drops than max
        if (maxItemDrops > 0 && maxItemDrops < droppedItems.Count - 1)
        {
            droppedItems.Sort((a, b) => b.probability.CompareTo(a.probability));
            
            Debug.Log(droppedItems.Count);
            droppedItems.RemoveRange(maxItemDrops, droppedItems.Count-1);
        }
        
        SpawnDrops(droppedItems.ToArray());
    }

    void SpawnDrops(ItemAndProbability[] drops)
    {
        var dropBag =Instantiate(lootPrefab, transform.position, transform.rotation);
        
        var dropComponent = dropBag.GetComponent<Drop>();

        List<Item> dropList = new List<Item>();
        foreach (var drop in drops)
        {
            dropList.Add(drop.item);
        }
        
        dropComponent.SetDrop(dropList.ToArray());
    }
}

[System.Serializable]
public class ItemAndProbability
{
    public Item item;
    [Range(0f, 1f)]
    public float probability;
}

