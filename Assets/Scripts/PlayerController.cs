using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController cc;
    [SerializeField] EntityStats stats;
    [Header("Roll")]
    [SerializeField] float rollSpeed = 4f;
    [SerializeField] float rollCooldown = .5f;
    float lastRollTime = -1;

    [Header("Animation")] 
    [SerializeField] private Animator animator;
    
    [Header("Equipment")]
    [SerializeField] Transform weaponBone;
    [SerializeField] Transform shieldBone;
    [SerializeField] Transform bootsBoneLeft;
    [SerializeField] Transform bootsBoneRight;
    GameObject equippedWeaponObject = null;
    GameObject equippedShieldObject = null;
    GameObject[] equippedBootsObject = new GameObject[2];

    [Header("Effects")]
    [SerializeField] VisualEffect slashEffect;
    //[SerializeField] BoxCollider swordCollider; //Not needed, spawn dinamically in HandleAttack()
    List<GameObject> hitEnemies = new List<GameObject>();
    List<GameObject> hitObjects = new List<GameObject>(); 
    bool isRolling = false;
    [SerializeField] private int maxBlocks = 2;
    private int currentBlocks = 2;
    [SerializeField] private float gainBlockTime = 2;
    bool isBlocking = false;
    IEnumerator deathCoroutine;

    public UnityEvent<bool> onImmunityAction = new UnityEvent<bool>();
    public bool IsRolling
    {
        get
        {
            return isRolling;
        }
    }
    string lastAnimatorState = "";

    public UnityEvent onAttack = new UnityEvent();
    [SerializeField] TrailRenderer swordTrail;

    void Start(){
        deathCoroutine = Die();
        currentBlocks = maxBlocks;
        UIController.instance.UpdateMoney(Inventory.instance.Money);
    }

    private void OnEnable()
    {
        //Debug.Log("OnEnable");
        StartCoroutine(GainBlocks(gainBlockTime));
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance && (GameController.instance.GamePaused || Time.timeScale == 0) )
        {
            return;
        }

        if (stats.IsDead())
        {
            return;
        }
        
        Vector3 movement = Vector3.zero;

        bool canMove = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) &&
            !animator.GetCurrentAnimatorStateInfo(0).IsTag("Roll") && !animator.GetCurrentAnimatorStateInfo(1).IsTag("Attack");
        
        //Se non sta rollando e si sta muovendo
        if(canMove)
        {
            var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            
            transform.forward = input.normalized;

            movement = GameController.instance.GetCheatCodes().speedIncrease ? 100 * input : stats.GetSpeed() * input;
        }else if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Roll")){
            movement = GameController.instance.GetCheatCodes().speedIncrease ? 110 * transform.forward.normalized : 
                (rollSpeed + stats.GetSpeed()) * transform.forward.normalized;
            lastRollTime = Time.time;
        }

        var rolling = animator.GetCurrentAnimatorStateInfo(0).IsTag("Roll");
        if (rolling != isRolling)
        {
            onImmunityAction.Invoke(rolling);
            isRolling = rolling;
        }
        
        animator.SetFloat("Speed", movement.magnitude/stats.GetSpeed());        
        
        HandleAttack();
        HandleShield();
        
        //Se preme tast roll e cooldown roll finito setta il trigger
        if(Input.GetButtonDown("Roll") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Roll") && 
        Time.time - lastRollTime > rollCooldown){
            animator.SetTrigger("Roll");
        }

        movement += Vector3.down * 9.81f;
        cc.Move(Time.smoothDeltaTime * movement);
        
        lastAnimatorState = animator.GetCurrentAnimatorStateInfo(1).fullPathHash.ToString();
    }
    
    private float lastAttackTime;
    void HandleAttack(){
        if(Input.GetButtonDown("Fire1") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Roll")){
            animator.SetTrigger("Attack");
        }

        bool attackAnimation = animator.GetCurrentAnimatorStateInfo(1).IsTag("Attack");
        
        swordTrail.emitting = attackAnimation;
        
        //Should enter here only when is actually starting the attack
        if (attackAnimation && 
            lastAnimatorState != animator.GetCurrentAnimatorStateInfo(1).fullPathHash.ToString())
        {
            slashEffect.Play();
            hitEnemies.Clear();
            hitObjects.Clear();
            onAttack.Invoke();
        }


        //Mentre è in corso un animazione di attacco, attiva hitbox
        //To be hit enemies must be in the Enemies layer and have the Enemy tag (both set in the inspector)
        if (attackAnimation && Inventory.instance.EquippedWeapon)
        {
            //Debug.Log("Attacking");
            Vector3 position = transform.TransformPoint(Inventory.instance.EquippedWeapon.hitboxOffset);

            Collider[] hits = Physics.OverlapSphere(position,
                Inventory.instance.EquippedWeapon.hitboxSize, LayerMask.GetMask("Enemies", "Prop"));
            Debug.DrawLine(position,
                position + transform.forward * Inventory.instance.EquippedWeapon.hitboxSize, Color.red);
            Debug.DrawLine(position,
                position - transform.forward * Inventory.instance.EquippedWeapon.hitboxSize, Color.red);
            foreach (Collider hit in hits)
            {
                //Debug.Log(hit.gameObject.name);
                if (hit.CompareTag("Enemy") && !hitEnemies.Contains(hit.gameObject) && hit.TryGetComponent(out EnemyController enemy))
                {
                    hitEnemies.Add(hit.gameObject);
                    if (GameController.instance.GetCheatCodes().oneShot)
                    {
                        enemy.TakeDamage(100000);  //One shots any enemy (hopefully)
                    }
                    else
                    {
                        enemy.TakeDamage(stats.GetAttack());
                    }
                }

                //oggetti distruggibili
                if (hit.CompareTag("Prop") && !hitObjects.Contains(hit.gameObject) && hit.TryGetComponent(out DestructableObject obj))
                {
                    hitObjects.Add(hit.gameObject);
                    if (GameController.instance.GetCheatCodes().oneShot)
                    {
                        obj.TakeDamage(100000);  //One shots any enemy (hopefully)
                    }
                    else
                    {
                        obj.TakeDamage(stats.GetAttack());
                    }
                }

            }
        }
    }

    void HandleShield(){
        if (Input.GetButtonDown("Block") && currentBlocks > 0)
        {
            animator.SetBool("Block", true);
        }
        else if (Input.GetButtonUp("Block"))
        {
            animator.SetBool("Block", false);
            
            stats.SetImmune(false);
            isBlocking = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(1).IsTag("Block") && currentBlocks > 0)
        {
            stats.SetImmune(true);
            isBlocking = true;
        }
        else if (isBlocking)
        {
            stats.SetImmune(false);
            animator.SetBool("Block", false);
            isBlocking = false;
        }
    }

    IEnumerator GainBlocks(float gainTime)
    {
        //Debug.Log("Starting gaining blocks");
        
        //Debug.Log($"Gain time {gainBlockTime}");
        while (true)
        {
            //Debug.Log(currentBlocks);
            if (currentBlocks < maxBlocks)
            {
                currentBlocks++;
                //Debug.Log("Gained blocks");
            }
            yield return new WaitForSeconds(gainTime);
        }
        //Debug.Log("Ending gaining blocks");
    }

    void LoseBlocks()
    {
        //If is not blocking shouldn't lose blocks
        if (!isBlocking)
        {
            return;   
        }
        
        currentBlocks--;

        if (currentBlocks <= 0)
        {
            Debug.Log("Shield break");
            animator.SetTrigger("ShieldBreak");
            currentBlocks = 0;
        }
    }

    public void EquipWeapon(Weapon weapon){
        if(!weapon)
            return;

        if(equippedWeaponObject){
            Destroy(equippedWeaponObject);
        }

        equippedWeaponObject = Instantiate(weapon.prefab, weaponBone);
        equippedWeaponObject.transform.localScale = weapon.modelScale;
        equippedWeaponObject.transform.localPosition = weapon.modelOffset;
        equippedWeaponObject.transform.localRotation = Quaternion.Euler(weapon.modelRotation);

        stats.SetAttackLv(weapon.attack);
    }

    public void EquipShield(Shield shield){
        if(!shield)
            return;

        if(equippedShieldObject){
            Destroy(equippedShieldObject);
        }

        equippedShieldObject = Instantiate(shield.prefab, shieldBone);
        equippedShieldObject.transform.localScale = shield.modelScale;
        equippedShieldObject.transform.localPosition = shield.modelOffset;
        equippedShieldObject.transform.localRotation = Quaternion.Euler(shield.modelRotation);

        stats.SetDefenseLv(shield.damageProtection);
    }

    public void EquipBoots(Boots boots){
        if(!boots)
            return;

        if(equippedBootsObject[0]){
            Destroy(equippedBootsObject[0]);
            Destroy(equippedBootsObject[1]);
        }

        equippedBootsObject[0] = Instantiate(boots.prefab.transform.GetChild(0).gameObject, bootsBoneLeft);
        equippedBootsObject[1] = Instantiate(boots.prefab.transform.GetChild(1).gameObject, bootsBoneRight);
        
        equippedBootsObject[0].transform.localScale = boots.modelScale;
        equippedBootsObject[0].transform.localPosition = boots.modelOffsetLeft;
        equippedBootsObject[0].transform.localRotation = Quaternion.Euler(boots.modelRotationLeft);
        equippedBootsObject[1].transform.localScale = boots.modelScale;
        equippedBootsObject[1].transform.localPosition = boots.modelOffsetRight;
        equippedBootsObject[1].transform.localRotation = Quaternion.Euler(boots.modelRotationRight);

        stats.SetSpeedLv(boots.speed);
    }

    public void Equip(Item item){
        Inventory.instance.UseItem(item);
    }

    public void TakeDamage(int damage, Vector3 from){
        var relativePos = from - transform.position;

        relativePos.y = 0;
        
        relativePos = relativePos.normalized;
        
        var angle = Mathf.Abs(Vector3.Angle(relativePos, transform.forward));

        Debug.Log(angle);
        
        //If is blocking and roughly in front of enemy, block attack
        if (isBlocking && angle < 45)
        {
            animator.SetTrigger("Hit");
            LoseBlocks();
            return;
        }
        
        if (!stats.TryHurt(damage)){
            //print(gameObject.name + " è morto");
            animator.SetTrigger("die");
            StartCoroutine(deathCoroutine);
        }
        UIController.instance.UpdateHealthBar(stats.GetMaxHp(), stats.GetHp());
    }

    public void Heal(int amount){
        stats.IncreaseHp(amount);
        UIController.instance.UpdateHealthBar(stats.GetMaxHp(), stats.GetHp());
    }

    IEnumerator Die(){
        yield return new WaitForSeconds(4);
        GameController.instance.RestartLevel();
    }
}
