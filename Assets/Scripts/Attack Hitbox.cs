using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        print("Attacca");
        if (other.gameObject.TryGetComponent<EntityStats>(out EntityStats entity))
        {
            //Must take damage from stats
            entity.TryHurt(5);
        }
    }
}
