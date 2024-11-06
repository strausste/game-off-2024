using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    Vector3 offset;
    [SerializeField] private float speed = 5;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = transform.position - player.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(GameController.instance && GameController.instance.GamePaused){
            return;
        }

        transform.position = Vector3.Slerp(transform.position, player.position + offset, Time.smoothDeltaTime * speed);
    }
}
