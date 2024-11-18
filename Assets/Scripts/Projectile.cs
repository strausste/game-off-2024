using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] int speed = 20;
    GameObject sender;
    EntityStats senderStats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        senderStats = sender.GetComponent<EntityStats>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void SetSender(GameObject sender){
        this.sender = sender;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            //Must take damage from stats
            //player.TakeDamage(senderStats.GetAttack());
        }
        Destroy(gameObject);
    }
}
