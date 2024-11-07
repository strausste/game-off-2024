using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController cc;
    [SerializeField] float moveSpeed = 10f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance && GameController.instance.GamePaused)
        {
            return;
        }
        
        Vector3 movement = moveSpeed * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        movement += Vector3.down * 9.81f;
        
        cc.Move(Time.smoothDeltaTime * movement);
    }
}
