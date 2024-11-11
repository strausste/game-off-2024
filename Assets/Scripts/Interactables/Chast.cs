using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] Animator animator;
    private bool open = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
