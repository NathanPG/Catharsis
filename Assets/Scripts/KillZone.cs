using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private Transform playerTransform;
    private GameManager gameManager;
    private void Awake()
    {
        
        
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        playerTransform = gameManager.playerTransform;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            //Make Sure Gravity is On
            gameManager.movementControl.ToggleGravity(true);
            //Clear Force
            gameManager.playerRigidbody.velocity = Vector3.zero;
            gameManager.playerRigidbody.angularVelocity = Vector3.zero;
            //Respawn
            playerTransform.position = GameManager.Instance.spawnPoint;
        }
    }
}
