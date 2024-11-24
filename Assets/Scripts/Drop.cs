using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class Drop : MonoBehaviour
{
    [SerializeField] ItemAndProbability[] possibleDrops;
    [SerializeField] private int maxItemDrops = 0; //If zero no limits
    
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
        if (maxItemDrops > 0 && maxItemDrops < droppedItems.Count)
        {
            droppedItems.Sort((a, b) => b.probability.CompareTo(a.probability));
            
            droppedItems.RemoveRange(maxItemDrops, droppedItems.Count);
        }
        
        SpawnDrops(droppedItems.ToArray());
    }

    void SpawnDrops(ItemAndProbability[] drops)
    {
        foreach (var drop in drops)
        {
            Instantiate(drop.item.prefab, transform.position, transform.rotation);   
        }
    }
}

[System.Serializable]
public class ItemAndProbability
{
    public Item item;
    [Range(0f, 1f)]
    public float probability;
}

