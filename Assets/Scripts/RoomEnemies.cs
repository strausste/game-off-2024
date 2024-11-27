using System.Collections.Generic;
using UnityEngine;

public class RoomEnemies : MonoBehaviour
{
    List<GameObject> enemies = new List<GameObject>();
    List<Door> doors = new List<Door>();

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
        if (other.CompareTag("Enemy")){
            enemies.Add(other.gameObject);
        }
        if (other.CompareTag("Door")){
            doors.Add(other.GetComponent<Door>());
        }
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
