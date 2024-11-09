using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject destination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player trovato");
            other.gameObject.SetActive(false);
            other.gameObject.transform.position = destination.transform.position;
            other.gameObject.SetActive(true);
        }
    }
}
