using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private Transform playerTransform;
    private GameManager gameManager;
    private void Awake()
    {
        gameManager = GameManager.Instance;
        playerTransform = gameManager.playerTransform;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            //Make Sure Gravity is On
            gameManager.playerObject.GetComponent<MovementControl>().ToggleGravity(true);
            //Clear Force
            gameManager.playerObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameManager.playerObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            //Respawn
            playerTransform.position = GameManager.Instance.spawnPoint;
        }
    }
}
