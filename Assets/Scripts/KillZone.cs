using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 spawnPoint = new Vector3(6.8499999f, 0, -2.04999995f);
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerTransform.position = spawnPoint;
        }
    }
}
