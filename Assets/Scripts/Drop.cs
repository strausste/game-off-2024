using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] ItemAndProbability[] possibleDrops;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class ItemAndProbability
{
    public Item item;
    [Range(0f, 1f)]
    public float probability;
}

