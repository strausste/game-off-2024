using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    Vector3 offset;
    [SerializeField] private float speed = 5;
    [SerializeField] private Transform targetTransform;
    private Vector3 originalRotation;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = transform.position - player.position;
        originalRotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (GameController.instance && GameController.instance.GamePaused)
        {
            return;
        }

        UpdatePosition();
    }

    void UpdatePosition()
    {
        var targetPosition = player.position + offset;
        var targetRotation = Quaternion.Euler(originalRotation);

        if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
            targetRotation = targetTransform.rotation;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.smoothDeltaTime);
        transform.position = Vector3.Slerp(transform.position, targetPosition, Time.smoothDeltaTime * speed);
    }

    public void SetTarget(Transform target)
    {
        targetTransform = target;
    }
}