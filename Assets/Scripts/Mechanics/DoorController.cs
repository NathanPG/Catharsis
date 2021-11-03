using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private bool alreadyInteracted = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !alreadyInteracted)
        {
            alreadyInteracted = true;
            GetComponent<Animation>().Play();
            GetComponent<AudioSource>().Play();
        }
    }
}
