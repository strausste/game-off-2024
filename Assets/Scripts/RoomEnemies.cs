using System.Collections.Generic;
using UnityEngine;

public class RoomEnemies : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies;
    [SerializeField] Door[] doors;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject enemy in enemies){
            if (enemy != null){
                return;     
            }
        }

        Unlock();
        Destroy(this);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")){
            Lock();
        }
    }

    void Lock(){
        foreach(Door door in doors){
            door.SetLocked(true);
        }
    }

    void Unlock(){
        foreach(Door door in doors){
            door.SetLocked(false);
        }
    }
}
