using System.Collections;
using UnityEngine;

public class EmptySfxDestroyer : MonoBehaviour
{
    private GameObject obj;
    // Update is called once per frame
    void Update()
    {
        obj = GameObject.FindGameObjectWithTag("EmptySoundObj");
        StartCoroutine(DestroyThis(obj));

    }

    private IEnumerator DestroyThis(GameObject obj)
    {
        yield return new WaitForSeconds(1.5f);

        Destroy(obj);
    }
}
