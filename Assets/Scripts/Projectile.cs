using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] int speed = 20;
    GameObject sender;
    EntityStats senderStats;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void SetSender(GameObject sender){
        this.sender = sender;
        senderStats = sender.GetComponent<EntityStats>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.TakeDamage(senderStats.GetAttack(), sender.transform.position);
        }
        Destroy(gameObject);
    }
}
