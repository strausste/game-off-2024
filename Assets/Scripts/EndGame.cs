using System;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.instance.EndGame();
            Destroy(this);
            return;
        }
    }
}
