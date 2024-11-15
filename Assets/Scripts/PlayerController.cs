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
    GameObject equippedWeaponObject = null;
    GameObject equippedShieldObject = null;

    [Header("Effects")]
    [SerializeField] VisualEffect slashEffect;
    //[SerializeField] BoxCollider swordCollider; //Not needed, spawn dinamically in HandleAttack()

    void Start(){
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
        //To be hit enemies must be in the Enemies layer and have the Enemy tag (both set in the inspector)
        if(animator.GetCurrentAnimatorStateInfo(1).IsTag("Attack") && Inventory.instance.EquippedWeapon){
            Collider []hits = Physics.OverlapSphere(transform.position + Inventory.instance.EquippedWeapon.hitboxOffset, 
                Inventory.instance.EquippedWeapon.hitboxSize, LayerMask.GetMask("Enemies"));
            Debug.DrawLine(transform.position + Inventory.instance.EquippedWeapon.hitboxOffset, 
                transform.position + Inventory.instance.EquippedWeapon.hitboxOffset + transform.forward * Inventory.instance.EquippedWeapon.hitboxSize, Color.red);
            Debug.DrawLine(transform.position + Inventory.instance.EquippedWeapon.hitboxOffset, 
                transform.position + Inventory.instance.EquippedWeapon.hitboxOffset - transform.forward * Inventory.instance.EquippedWeapon.hitboxSize, Color.red);
            foreach(Collider hit in hits){
                if (hit.CompareTag("Enemy") && hit.TryGetComponent(out EntityStats enemy))
                {
                    enemy.TryHurt(Inventory.instance.EquippedWeapon.attack);
                }
            }
        }
    }

    void HandleShield(){

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
    }

    // IEnumerator disableSwordHitbox(){    //
    //     //Disables the attack hitbox after attacking
    //     yield return new WaitForSeconds(0.1f);
    //     swordCollider.enabled = false;
    // }
}
