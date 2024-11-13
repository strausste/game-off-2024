using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] Animator animator;
    private bool open = false;

    public GameObject canvas;


    private void Start()
    {
        animator = GetComponent<Animator>();
        canvas.SetActive(false);
    }

    private void Update()
    {


    }

    public void Interact()
    {
        if (!open)
        {
            if (animator != null)
            {
                animator.SetTrigger("Trigger");
                open = true;
            }
        }
    }


    private void OnTriggerEnter()
    {
        if (!open)
        {
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!open)
        {
            canvas.SetActive(false);
        }
    }

}
