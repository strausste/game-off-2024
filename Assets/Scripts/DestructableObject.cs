using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    [SerializeField] int lifePoints;
    private bool destroyed;
    [SerializeField] ParticleSystem dyingParticles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        destroyed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(destroyed)
        {
            StartDyingEffect();
            Destroy(gameObject);
        }
    }



    private void StartDyingEffect()
    {
        if (dyingParticles)
        {
            Instantiate(dyingParticles, transform.position, Quaternion.identity);
        }
    }

    public void TakeDamage(int damage)
    {
        lifePoints-=damage;
        if (lifePoints <= 0)
        {
            destroyed = true;
        }
    }
}
