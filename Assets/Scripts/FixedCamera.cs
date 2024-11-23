using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    [SerializeField] CameraController camera;
    Transform fixedTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fixedTransform = transform.GetChild(0).transform;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other) {
        if (camera && other.CompareTag("Player")){
            camera.SetTarget(fixedTransform);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (camera && other.CompareTag("Player")){
            camera.SetTarget(null);
        }
    }
}
