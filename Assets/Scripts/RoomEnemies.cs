using System.Collections.Generic;
using UnityEngine;

public class RoomEnemies : MonoBehaviour
{
    List<GameObject> enemies;
    List<GameObject> doors;
    List<GameObject> zones;

    private void Start() {
        enemies = new List<GameObject>();
        doors = new List<GameObject>();
        zones = new List<GameObject>();
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
        if (other.CompareTag("Enemy")){
            enemies.Add(other.gameObject);
        }
        if (other.CompareTag("Door")){
            doors.Add(other.gameObject);
        }
        if (other.CompareTag("CameraZone")){
            zones.Add(other.gameObject);
        }
        if (other.CompareTag("Player")){
            Lock();
        }
    }

    void Lock(){
        foreach(GameObject door in doors){
            if (door.TryGetComponent<Door>(out Door doorComponent))
                doorComponent.SetLocked(true);
        }
        foreach(GameObject zone in zones){
            zone.SetActive(false);
        }
    }

    void Unlock(){
        print(doors.Count);
        foreach(GameObject door in doors){
            if (door.TryGetComponent<Door>(out Door doorComponent))
                doorComponent.SetLocked(false);
        }
        foreach(GameObject zone in zones){
            zone.SetActive(true);
        }
    }
}
