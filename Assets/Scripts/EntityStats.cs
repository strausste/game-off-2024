using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [Header ("Stats")]
    [SerializeField] int maxHp = 10;
    [SerializeField] int attackLv = 1;
    [SerializeField] int defenseLv = 1;
    [SerializeField] int speedLv = 1;
    
    [Header ("Effects")]
    [SerializeField] ParticleSystem hitParticles;
    [SerializeField] Renderer[] meshRenderers;
    [SerializeField] Material flashMaterial;

    public UnityEvent onHit = new UnityEvent();
    public UnityEvent onDamaged = new UnityEvent();
    Rigidbody rb;
    List<Material> origMaterial = new List<Material>();
    float flashTime = 0.1f;

    int hp;
    bool isImmune = false;
    bool isDead = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (meshRenderers.Length > 0) {
            foreach (Renderer renderer in meshRenderers){
                origMaterial.Add(renderer.material);
            }
        }
        hp = maxHp;
    }

    public bool TryHurt(int damage){
        onHit.Invoke();
        
        if (isImmune) 
            return true;

        
        int trueDamage = damage - GetDefense();
        int rest = GetHp();
        trueDamage = Mathf.Clamp(trueDamage, 1, damage);
        if (hp >= 0){
            rest = Hurt(trueDamage);
        }

        Push(5);

        if (rest <= 0)
            return false;
        return true;
    }

    int Hurt(int damage){
        //print(damage);
        int rest = hp - damage;
        hp -= damage;

        if (hp <= 0){
            Die();
        }
        
        StartCoroutine(Flash());
        
        onDamaged.Invoke();
        
        return rest;
    }

    void Die(){
        //print(gameObject.name + " è morto");
        //onDeath.Invoke();
        isImmune = true;
        isDead = true;
    }

    IEnumerator Flash(){
        if (hitParticles)
            Instantiate(hitParticles, transform.position, Quaternion.identity);
            
        foreach (Renderer renderer in meshRenderers){
            renderer.material = flashMaterial;
        }
        yield return new WaitForSeconds(flashTime);

        for (int i = 0; i < meshRenderers.Count(); i++){
            meshRenderers[i].material = origMaterial[i];
        }
    }

    public void Push(int knockBackForce){
        //Pushes Back the Entity
        Vector3 force = -transform.forward * knockBackForce;
        rb.AddForce(force, ForceMode.Impulse);
    }

    public void SetImmune(bool immune){
        isImmune = immune;
    }

    public void SetAttackLv(int level){
        attackLv = level;
    }

    public void SetDefenseLv(int level){
        defenseLv = level;
    }


    public void SetSpeedLv(int level){
        speedLv = level;
    }


    public int GetHp(){
        return hp;
    }

    public int GetMaxHp()
    {
        return maxHp;
    }
    public int GetAttack(){
        return (int)(attackLv * Globals.globalAttackScaling);
    }
    public int GetDefense(){
        return (int)(defenseLv * Globals.globalDefenseScaling);
    }
    public int GetSpeed(){
        return (int)(Globals.baseSpeed + speedLv * Globals.globalSpeedScaling);
    }
    public int GetAttackLv(){
        return attackLv;
    }
    public int GetDefenseLv(){
        return defenseLv;
    }
    public int GetSpeedLv(){
        return speedLv;
    }
    public bool IsDead(){
        return isDead;
    }

    public void IncreaseHp(int amount)
    {
        hp += amount;
    }
}
