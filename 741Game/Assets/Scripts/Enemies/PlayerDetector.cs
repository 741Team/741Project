using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public bool playerInRange;

    private void Start()
    {
        playerInRange = false;
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
