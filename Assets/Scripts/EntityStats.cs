using System.Collections;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [SerializeField] int maxHp = 10;
    [SerializeField] int attackLv = 1;
    [SerializeField] int defenseLv = 1;
    [SerializeField] int speedLv = 1;
    
    Rigidbody rigidbody;
    MeshRenderer meshRenderer;
    Color origColor;
    float flashTime = 0.1f;

    int hp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        origColor = meshRenderer.material.color;
        hp = maxHp;
    }

    public void TryHurt(int damage){
        int trueDamage = damage - GetDefense();
        if (trueDamage > 0 && hp >= 0){
            Hurt(trueDamage);
        }

        Push(5);
    }

    int Hurt(int damage){
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
    }

    IEnumerator Flash(){
        meshRenderer.material.color = Color.white;
        yield return new WaitForSeconds(flashTime);
        meshRenderer.material.color = origColor;
    }

    public void Push(int knockBackForce){
        //Pushes Back the Entity
        Vector3 force = -transform.forward * knockBackForce;
        rigidbody.AddForce(force, ForceMode.Impulse);
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
