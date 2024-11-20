using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;  
    PlayerController pc;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (pc.IsRolling || GameController.instance.GetCheatCodes().noDamage) //Non prende danno se rotola, ho aggiunto ora sto controllo nel player controller
        {
            return;
        }
        
        currentHealth -= damage;
        Debug.Log("Player health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        Time.timeScale = 0; // Temporary lol
    }

    public int GetCurrentHealth() { return this.currentHealth; }

    public int GetMaxHealth() { return this.maxHealth; }

}
