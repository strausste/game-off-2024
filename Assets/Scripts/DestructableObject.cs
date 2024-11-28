using System.Collections;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class DestructableObject : MonoBehaviour
{
    [Header("Misc")]
    [SerializeField] int lifePoints;

    [Header("Death")]
    [SerializeField] ParticleSystem dyingParticles;
    [SerializeField] AudioClip deathSfx;

    [Header("Hit")]
    [SerializeField] Renderer meshRenderer;
    [SerializeField] ParticleSystem hitParticles;
    float flashTime = 0.1f;
    [SerializeField] Material originalMaterial;
    [SerializeField] Material flashMaterial;



    private void OnDestroy()
    {
        if (lifePoints<=0)  //non togliere o fa un casino nero caricando le nuove scene
        {
            StartDyingEffect();
            StartDyingSFX();
        }
        
    }

    private void StartDyingEffect()
    {
        if (dyingParticles)
        {
            Instantiate(dyingParticles, transform.position, Quaternion.identity);
        }
    }

    private void StartDyingSFX()
    {
        if (deathSfx)
        {
            GameObject aud = new GameObject();
            aud.gameObject.tag = "EmptySoundObj";
            aud.AddComponent<AudioSource>();
            aud.GetComponent<AudioSource>().clip = deathSfx;
            aud.GetComponent<AudioSource>().volume = 0.2f;
            aud.GetComponent<AudioSource>().Play();
        }
        
    }

    public void TakeDamage(int damage)
    {
        lifePoints-=damage;

        StartCoroutine(Flash());

        if (lifePoints <= 0)
        {
            Destroy(gameObject);
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
