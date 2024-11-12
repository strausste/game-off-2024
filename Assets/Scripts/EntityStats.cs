using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [SerializeField] int maxHp = 10;
    int hp;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hp = maxHp;
    }

    public void TryHurt(int damage){
        if (damage > 0 && hp >= 0){
            Hurt(damage);
        }
    }

    int Hurt(int damage){
        int rest = hp - damage;
        hp -= damage;
        if (hp <= 0){
            Kill();
        }
        return rest;
    }

    void Kill(){
        print(this + " è morto");
    }
}
