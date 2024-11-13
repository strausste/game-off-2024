using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController cc;
    [SerializeField] float moveSpeed = 10f;
    [Header("Roll")]
    [SerializeField] float rollSpeed = 12f;
    [SerializeField] float rollCooldown = .5f;
    float lastRollTime = -1;

    [Header("Animation")] 
    [SerializeField] private Animator animator;
    
    [Header("Equipment")]
    [SerializeField] Transform weaponBone;
    [SerializeField] Transform shieldBone;
    [SerializeField] Weapon equippedWeapon;
    GameObject equippedWeaponObject = null;
    [SerializeField] Shield equippedShield;
    GameObject equippedShieldObject = null;

    [Header("Effects")]
    [SerializeField] VisualEffect slashEffect;
    //[SerializeField] BoxCollider swordCollider; //Not needed, spawn dinamically in HandleAttack()

    void Start(){
        //Init weapon and shield
        EquipWeapon(equippedWeapon);
        EquipShield(equippedShield);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance && GameController.instance.GamePaused)
        {
            return;
        }
        
        Vector3 movement = Vector3.zero;
        
        bool canMove = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) &&
            !animator.GetCurrentAnimatorStateInfo(0).IsTag("Roll") &&
            !animator.GetCurrentAnimatorStateInfo(1).IsTag("Attack");
        
        //Se non sta rollando e si sta muovendo
        if(canMove)
        {
            var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            
            transform.forward = input.normalized;

            movement = moveSpeed * input;
        }else if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Roll")){
            movement = rollSpeed * transform.forward.normalized;
            lastRollTime = Time.time;
        }

        animator.SetFloat("Speed", movement.magnitude/moveSpeed);        

        HandleAttack();

        //Se preme tast roll e cooldown roll finito setta il trigger
        if(Input.GetButtonDown("Roll") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Roll") && 
        Time.time - lastRollTime > rollCooldown){
            animator.SetTrigger("Roll");
        }

        movement += Vector3.down * 9.81f;
        cc.Move(Time.smoothDeltaTime * movement);
    }
    
    private float lastAttackTime;
    void HandleAttack(){
        if(Input.GetButtonDown("Fire1") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Roll")){
            animator.SetTrigger("Attack");
            slashEffect.Play();
            //swordCollider.enabled = true;
            //StartCoroutine(disableSwordHitbox());
        }

        //Mentre è in corso un animazione di attacco, attiva hitbox
        if(animator.GetCurrentAnimatorStateInfo(1).IsTag("Attack") && equippedWeapon){
            Collider []hits = Physics.OverlapSphere(transform.position + equippedWeapon.hitboxOffset, equippedWeapon.hitboxSize);

            foreach(Collider hit in hits){
                if(!hit.CompareTag("Enemy"))
                    return;

                EntityStats enemy = hit.GetComponent<EntityStats>();

                enemy.TryHurt(equippedWeapon.attack);
            }
        }
    }

    void HandleShield(){

    }

    void EquipWeapon(Weapon weapon){
        if(!weapon)
            return;

        if(equippedWeaponObject){
            Destroy(equippedWeaponObject);
        }

        equippedWeapon = weapon;
        equippedWeaponObject = Instantiate(weapon.prefab, weaponBone);
        equippedWeaponObject.transform.localScale = weapon.modelScale;
        equippedWeaponObject.transform.localPosition = weapon.modelOffset;
    }

    void EquipShield(Shield shield){
        if(!shield)
            return;

        if(equippedShieldObject){
            Destroy(equippedWeaponObject);
        }

        equippedShield = shield;
        equippedWeaponObject = Instantiate(shield.prefab, shieldBone);
    }

    // IEnumerator disableSwordHitbox(){    //
    //     //Disables the attack hitbox after attacking
    //     yield return new WaitForSeconds(0.1f);
    //     swordCollider.enabled = false;
    // }
}
