using System.Collections;
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

    Rigidbody rb;
    Material origMaterial;
    float flashTime = 0.1f;

    int hp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (meshRenderers.Length > 0) {
            origMaterial = meshRenderers[0].material;
        }
        hp = maxHp;
    }

    public bool TryHurt(int damage){
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
        
        return rest;
    }

    void Die(){
        
    }

    IEnumerator Flash(){
        Instantiate(hitParticles, transform.position, Quaternion.identity);
        foreach (Renderer renderer in meshRenderers){
            renderer.material = flashMaterial;
        }
        yield return new WaitForSeconds(flashTime);

        foreach (Renderer renderer in meshRenderers){
            renderer.material = origMaterial;
        }
    }

    public void Push(int knockBackForce){
        //Pushes Back the Entity
        Vector3 force = -transform.forward * knockBackForce;
        rb.AddForce(force, ForceMode.Impulse);
    }

    public int GetHp(){
        return hp;
    }
    public int GetAttack(){
        //Sostituire con valori globali 
        //ES: global_attack_modifier
        return (int)(attackLv * Globals.globalAttackScaling);
    }
    public int GetDefense(){
        //Sostituire con valori globali
        return (int)(defenseLv * Globals.globalDefenseScaling);
    }
    public int GetSpeed(){
        //Sostituire con valori globali
        return (int)(Globals.baseSpeed + speedLv * Globals.globalSpeedScaling);
    }
}
