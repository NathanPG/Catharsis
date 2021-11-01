using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private bool isOpen = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !isOpen)
        {
            isOpen = true;
            GetComponent<Animation>().Play();
            GetComponent<AudioSource>().Play();
        }
    }
}
