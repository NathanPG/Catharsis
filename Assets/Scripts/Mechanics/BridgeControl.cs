using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeControl : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        GetComponent<Animator>().Play("Bridge1Test");
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.other.name);
        //GetComponent<Animator>().Play("Bridge1Test");
    }
}
