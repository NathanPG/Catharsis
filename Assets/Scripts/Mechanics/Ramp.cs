using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramp : MonoBehaviour
{
    // Update is called once per frame
    private bool playerInRamp = false;
    private MovementControl playerMovement = null;
    private PlayerController playerRef = null;
    private Vector3 forwardDirection;
    private void Start()
    {
        playerMovement = GameManager.Instance.movementControl;
        playerRef = GameManager.Instance.playerController;
        forwardDirection = transform.TransformDirection(Vector3.forward) * 10;
    }

    void Update()
    {
        Debug.DrawRay(transform.position, forwardDirection, Color.red);

        if (playerInRamp)
        {
            if(playerMovement == null) Debug.LogError("Movement control missing");
            playerMovement.Ramp(forwardDirection);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerRef.MoveState = false;
            playerInRamp = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerRef.MoveState = true;
            playerInRamp = false;
        }
    }

    private void OnDrawGizmos()
    {
        
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.TransformDirection(Vector3.forward) * 20f);
    }
}
