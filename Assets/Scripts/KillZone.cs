using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public Transform playerStartTransform;
    private Vector3 spawnPoint;

    private void Start()
    {
        spawnPoint = playerStartTransform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag);
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.playerObject.GetComponent<MovementControl>().ToggleGravity(true);
            playerStartTransform.position = spawnPoint;
        }
    }
}
