using Unity.VisualScripting;
using UnityEngine;

public class InvisibleObjectEnigma : MonoBehaviour
{
    [SerializeField] GameObject door;
    //[SerializeField] GameObject invisibleObject;
    private bool solved;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        solved = false;
        door.SetActive(false);
    }

    private void OnDestroy()
    {
        if (!solved)
        {
            door.SetActive(true);
            solved = true;
        }
    }
}
