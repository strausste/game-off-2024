using UnityEngine;

public class BalloonText : MonoBehaviour
{
    [SerializeField] Transform target;
    public Vector3 offset = new Vector3(1.5f,1.2f,0f);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
        transform.LookAt(Camera.main.transform);
    }
}
