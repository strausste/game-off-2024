using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] Animator animator;
    private bool open = false;

    public GameObject canvas;               
    [SerializeField] public float detectionRadius;
    private LayerMask mask;             // Layer per limitare i rilevamenti (es. solo il Player)

    private void Start()
    {
        animator = GetComponent<Animator>();
        canvas.SetActive(false);
        mask = LayerMask.GetMask("Player");
    }

    private void Update()
    {
        // Direzione dello SphereCast (in questo caso, verso il basso) ??
        Vector3 direction = Vector3.down;
        if (!open) {
            /*
            if (Physics.SphereCast(transform.position, detectionRadius, direction, out RaycastHit hit, detectionRadius, mask))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Player trovato");
                    canvas.SetActive(true);
                }
                else
                {
                    canvas.SetActive(false);
                }
            }*/


            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    Debug.Log("Player trovato");
                    canvas.SetActive(true);
                }
                else
                {
                    canvas.SetActive(false);
                }
            }
        }
        else
        {
            canvas.SetActive(false);
        }

    }

    private void OnDrawGizmos()
    {
        // Disegna una sfera per visualizzare il raggio di rilevamento
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void Interact()
    {
        Debug.Log("Dentro Interact Cassa");
        if (!open)
        {
            Debug.Log("Cassa non Chiusa");
            if (animator != null)
            {
                Debug.Log("Animator non nullo");
                Debug.Log(transform.position);
                animator.SetTrigger("Trigger");
                open = true;
            }
        }
    }
}
