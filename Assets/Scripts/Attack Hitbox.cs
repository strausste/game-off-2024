using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    EntityStats stats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stats = GetComponentInParent<EntityStats>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.TakeDamage(stats.GetAttack());
        }
    }
}
