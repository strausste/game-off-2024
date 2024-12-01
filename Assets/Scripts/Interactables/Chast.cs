using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] Animator animator;
    private bool open = false;

    public GameObject canvas;

    public GameObject player;
    [SerializeField] AudioSource chestSound;    

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
                Inventory.instance.IncMoney(5);
                chestSound?.PlayDelayed(0.5f);
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
