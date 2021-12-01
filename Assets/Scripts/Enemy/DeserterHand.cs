using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeserterHand : MonoBehaviour
{
    private Deserter deserter;

    private void Start()
    {
        deserter = GetComponentInParent<Deserter>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {
            Debug.Log("Collision");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Trigger");
        }
    }
}
