using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SimpleTrigger : MonoBehaviour
{
    [SerializeField] private string compareTag = null;
    public UnityEvent triggerEvent;
    
    void OnTriggerEnter(Collider collider)
    {
        if (compareTag != null && !collider.CompareTag(compareTag))
        {
            return;
        }
        
        triggerEvent.Invoke();
    }
}
