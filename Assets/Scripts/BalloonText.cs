using UnityEngine;

public class BalloonText : MonoBehaviour
{
    [SerializeField] Transform target;
    WriteSymbols canvas;
    public Vector3 offset = new Vector3(1.5f,2f,0f);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GetComponentInChildren<WriteSymbols>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target){
            transform.position = target.position + offset;
        }
        
        transform.LookAt(Camera.main.transform);
    }

    public void SetTarget(Transform target){
        this.target = target;
    }

    public void Write(Meaning[] meanings){
        canvas = GetComponentInChildren<WriteSymbols>();
        canvas.SetMeanings(meanings);
    }
}
