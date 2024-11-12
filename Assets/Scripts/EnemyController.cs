using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    /* -ste: I've not decided yet if divide this class in two childs (RangedEnemyController and MeleeEnemyController) */
    public enum EnemyType {Melee, Ranged};

    [Header("References")]
    [SerializeField] private Transform player; 
    [SerializeField] private PlayerHealthController phc; 


    [Header("Property")] 
    [SerializeField] private EnemyType enemyType;

    [Header("Movement")] 
    [SerializeField] private float engageDistance;
    [SerializeField] private float disengageDistance;

    [Header("Attack")] 
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackDistance;
    [SerializeField] private int attackDamage;

    
    private NavMeshAgent agent;
    private float lastAttackTime = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Engage
        if (distanceToPlayer <= engageDistance)
        {
            agent.SetDestination(player.position);

            if (distanceToPlayer <= attackDistance)
            {
                Attack();

                //Debug.Log("Current player's health: " + phc.GetCurrentHealth());
            }
        }
        // Disengage
        else if (distanceToPlayer > disengageDistance)
        {
            agent.ResetPath();
        }
    }

    void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            //phc.TakeDamage(attackDamage);
        }
    }

    // Debug
    void OnDrawGizmos()
    {
        if (player != null)
        {
            // Engage distance
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, engageDistance);

            // Disengage distance
            Gizmos.color = Color.grey;
            Gizmos.DrawWireSphere(transform.position, disengageDistance);

            // Attack distance
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
        }
    }

    public void TakeDamage(int damage){
        //Aggiunta per iniziare a metterla nel player controller
        Debug.Log($"Hit {gameObject} for {damage}");
    }
}
