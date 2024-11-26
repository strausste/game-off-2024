using System.Collections;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    [SerializeField] int lifePoints;
    private bool destroyed;
    [SerializeField] ParticleSystem dyingParticles;

    [Header("Hit")]
    [SerializeField] Renderer meshRenderer;
    [SerializeField] ParticleSystem hitParticles;
    float flashTime = 0.1f;
    [SerializeField] Material originalMaterial;
    [SerializeField] Material flashMaterial;

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

        StartCoroutine(Flash());

        if (lifePoints <= 0)
        {
            destroyed = true;
        }
    }

    IEnumerator Flash()
    {
        if (hitParticles)
            Instantiate(hitParticles, transform.position, Quaternion.identity);

        meshRenderer.material = flashMaterial;

        yield return new WaitForSeconds(flashTime);

        meshRenderer.material = originalMaterial;
    }
}
