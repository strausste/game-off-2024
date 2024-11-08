using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    /* -ste: I've not decided yet if divide this class in two childs (RangedEnemyController and MeleeEnemyController) */
    public enum EnemyType {Melee, Ranged};

    [Header("References")]
    [SerializeField] private Transform player; 
    
    [Header("Enemy properties")] 
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private float engageDistance;
    [SerializeField] private float disengageDistance;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackDistance;
    
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

            // TODO
            Debug.Log("Attack");
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
}
