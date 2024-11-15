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
    [SerializeField] SkinnedMeshRenderer meshRenderer;
    [SerializeField] Material flashMaterial;

    Rigidbody rb;
    Material origMaterial;
    float flashTime = 0.1f;

    int hp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        origMaterial = meshRenderer.material;
        hp = maxHp;
    }

    public void TryHurt(int damage){
        int trueDamage = damage - GetDefense();
        trueDamage = Mathf.Clamp(trueDamage, 1, damage);
        if (hp >= 0){
            Hurt(trueDamage);
        }

        Push(5);
    }

    int Hurt(int damage){
        print(damage);
        int rest = hp - damage;
        hp -= damage;

        if (hp <= 0){
            Die();
        }
        
        StartCoroutine(Flash());
        
        return rest;
    }

    void Die(){
        print(this + " è morto");
        //Should call death animation first
        Destroy(gameObject);
    }

    IEnumerator Flash(){
        Instantiate(hitParticles, transform.position, Quaternion.identity);
        meshRenderer.material = flashMaterial;
        yield return new WaitForSeconds(flashTime);
        meshRenderer.material = origMaterial;
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
        return attackLv * 2;
    }
    public int GetDefense(){
        //Sostituire con valori globali
        return defenseLv * 1;
    }
    public int GetSpeed(){
        //Sostituire con valori globali
        return speedLv * 10;
    }
}
