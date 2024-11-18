using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    /* -ste: I've not decided yet if divide this class in two childs (RangedEnemyController and MeleeEnemyController) */
    public enum EnemyType {MELEE, RANGED};
    enum State {IDLE, FOLLOW, ATTACK, DIE}

    [Header("References")]
    [SerializeField] private Transform player; 
    [SerializeField] private PlayerHealthController phc; 


    [Header("Property")] 
    [SerializeField] private EnemyType enemyType;

    [Header("Movement")] 
    [SerializeField] private float engageDistance;
    [SerializeField] private float disengageDistance;

    [Header("Attack")] 
    [SerializeField] private Collider attackBox;
    [SerializeField] private float attackCooldown = 1;
    [SerializeField] private float attackDistance;
    [SerializeField] private int attackDamage;

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
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        stats = GetComponent<EntityStats>();

        agent.speed = stats.GetSpeed();
    }

    // Update is called once per frame
    void Update()
    {
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
                speaker.Speak(SymbolSpeaker.PhraseType.PLAYER_SPOTTED);
            }

            state = State.FOLLOW;
            animator.SetBool("following", true);
        }
    }
    
    void Follow(){
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
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
        if (distanceToPlayer <= engageDistance)
        {
            lastPlayerPos = player.position;
        }

        agent.isStopped = false;
        agent.SetDestination(lastPlayerPos);
    }

    void Attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackDistance)
        {
            state = State.FOLLOW;
            animator.SetBool("attacking", false);
        }
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            animator.SetTrigger("attack");
            //phc.TakeDamage(attackDamage);
            if (enemyType == EnemyType.MELEE){
                attackBox.enabled = true;
                StartCoroutine(disableSwordHitbox());
            }
        }
        
        Vector3 targetDirection = player.position - transform.position;
        int rotSpeed = 5;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * rotSpeed, 0.0f);
        newDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(newDirection);
        agent.isStopped = true;
    }

    void Dying(){
        
    }

    void Die(){
        print(this + " è morto");
        Destroy(gameObject);
    }

    public void TakeDamage(int damage){
        if (state == State.DIE)
            return;

        if (!stats.TryHurt(damage)){
            state = State.DIE;
            animator.SetTrigger("die");
        }

        //Aggiunta per iniziare a metterla nel player controller
        Debug.Log($"Hit {gameObject} for {damage}");
    }

    void ShootProjectile(){
        GameObject newProj = Instantiate(projectile, shootTransform.position, transform.rotation);
        newProj.GetComponent<Projectile>().SetSender(gameObject);
    }

    IEnumerator disableSwordHitbox(){   
        //Disables the attack hitbox after attacking
        yield return new WaitForSeconds(0.1f);
        attackBox.enabled = false;
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
