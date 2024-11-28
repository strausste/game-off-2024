using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyType {MELEE, RANGED};
    enum State {IDLE, FOLLOW, ATTACK, DIE}

    [Header("References")]
    [SerializeField] private Transform player; 

    [Header("Property")] 
    [SerializeField] private EnemyType enemyType;

    [Header("Movement")] 
    [SerializeField] private float engageDistance;
    [SerializeField] private float disengageDistance;

    [Header("Attack")] 
    [SerializeField] private Collider attackBox;
    [SerializeField] private float attackCooldown = 1;
    [SerializeField] private float attackDistance;

    [Header("Ranged")] 
    [SerializeField] GameObject projectile;
    [SerializeField] Transform shootTransform;
    
    private NavMeshAgent agent;
    private Animator animator;
    private EntityStats stats;
    private float lastAttackTime = 0f;
    private State state = State.IDLE;
    Vector3 lastPlayerPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        stats = GetComponent<EntityStats>();

        agent.speed = stats.GetSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
            return;
            
        switch (state){
            case State.IDLE: Idle();
            break;
            case State.FOLLOW: Follow();
            break;
            case State.ATTACK: Attack();
            break;
            case State.DIE: Dying();
            break;
        }
    }

    void Idle(){
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Engage
        if (distanceToPlayer <= engageDistance)
        {
            if (TryGetComponent<SymbolSpeaker>(out SymbolSpeaker speaker))
            {
                if (Random.Range(0,5) == 0)
                    speaker.Speak(new Meaning[]{Meaning.ENEMY});
                else if (Random.Range(0,5) == 0)
                    speaker.Speak(new Meaning[]{Meaning.STRENGHT});
            }

            state = State.FOLLOW;
            animator.SetBool("following", true);
        }
    }
    
    void Follow(){
        Vector3 targetDirection = agent.destination - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection.normalized, Time.deltaTime * agent.angularSpeed, 0.0f);
        newDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(newDirection);
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= engageDistance)
        {
            lastPlayerPos = player.position;
        }

        agent.isStopped = false;
        agent.SetDestination(lastPlayerPos);

        if (distanceToPlayer <= attackDistance)
        {
            state = State.ATTACK;
            animator.SetBool("attacking", true);         
        }

        if (distanceToPlayer > disengageDistance)
        {
            agent.ResetPath();
            state = State.IDLE;
            animator.SetBool("following", false);
        }
    }

    void Attack()
    {
        agent.ResetPath();
        agent.isStopped = true;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            animator.SetTrigger("attack");
        }
        
        Vector3 targetDirection = player.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection.normalized, Time.deltaTime * agent.angularSpeed, 0.0f);
        newDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(newDirection);
        
        if (distanceToPlayer > attackDistance && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            state = State.FOLLOW;
            animator.SetBool("attacking", false);
        }
    }

    void Dying(){
        
    }

    void Die(){
        //print(this + " è morto");
        Destroy(gameObject);
    }

    void LandAttack(){
        if (enemyType == EnemyType.MELEE){
            StartCoroutine(EnableAttackBox());
        }
    }

    public void TakeDamage(int damage){
        if (state == State.DIE)
            return;

        agent.isStopped = true;
        agent.ResetPath();

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            animator.SetTrigger("hit");

        if (!stats.TryHurt(damage)){
            state = State.DIE;
            animator.SetTrigger("die");
            if (TryGetComponent<SymbolSpeaker>(out SymbolSpeaker speaker) && Random.Range(0,4) == 0){
                speaker.Speak(new Meaning[]{Meaning.NEGATIVE});
            }
        }
    }

    void ShootProjectile(){
        GameObject newProj = Instantiate(projectile, shootTransform.position, transform.rotation);
        newProj.GetComponent<Projectile>().SetSender(gameObject);
    }

    IEnumerator EnableAttackBox(){  
        attackBox.enabled = true; 
        //Disables the attack hitbox after attacking
        yield return new WaitForSeconds(0.1f);
        attackBox.enabled = false;
    }

    // Debug
    void OnDrawGizmos()
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
