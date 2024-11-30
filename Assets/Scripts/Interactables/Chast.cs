using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] Animator animator;
    private bool open = false;

    public GameObject canvas;

    public GameObject player;

    [SerializeField] Item item;


    private void Start()
    {
        animator = GetComponent<Animator>();
        canvas.SetActive(false);
    }

    public void Interact()
    {
        if (!open)
        {
            if (animator != null)
            {
                animator.SetTrigger("Trigger");
                open = true;
                canvas.SetActive(false);
                Inventory.instance.AddItem(item);
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
