using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeControl : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Animator>().Play("Bridge1Test");
    }
}
