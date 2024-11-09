using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController cc;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] Animator animator;

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance && GameController.instance.GamePaused)
        {
            return;
        }
        
        Vector3 movement = Vector3.zero;
        
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0){
            transform.forward = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            movement = moveSpeed * transform.forward.normalized;
        }

        animator.SetFloat("Speed", movement.magnitude);

        movement += Vector3.down * 9.81f;
        
        cc.Move(Time.smoothDeltaTime * movement);

        if(Input.GetButtonDown("Fire1")){
            animator.SetTrigger("Attack");
        }
    }
}
