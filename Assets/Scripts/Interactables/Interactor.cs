using UnityEngine;

//ho seguito un tutorial, don't ask

//interfaccia dalla quale, l'oggetto che viene interagito,
//  eredita la funzione Interact, specificata nella classe dell'oggetto
interface IInteractable {
    public void Interact();
}


public class Interactor : MonoBehaviour
{
    [SerializeField] float interactRange;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(transform.position, transform.forward);
            if(Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
            {   
                //se trova un oggetto di fronte a se che eredita l'interfaccia IInteractable
                if(hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {   
                    //chiama la funzione di quell'oggetto
                    interactObj.Interact();
                }
            }
        }
    }
}
