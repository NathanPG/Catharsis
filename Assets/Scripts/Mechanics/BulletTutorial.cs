using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BulletTutorial : MonoBehaviour
{
    private bool isPlayerIn = false;
    private bool finished = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isPlayerIn && !finished)
        {
            isPlayerIn = true;
            GameManager.Instance.playerObject.GetComponent<PlayerController>().canMove = false;
            GameManager.Instance.playerObject.GetComponent<PlayerController>().canJump = false;
        }
    }

    private void Update()
    {
        if (isPlayerIn)
        {
            if(GameManager.Instance.playerObject.GetComponent<PlayerController>()
                .rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed))
            {
                if (triggerPressed)
                {
                    GameManager.Instance.playerObject.GetComponent<PlayerController>().canMove = true;
                    GameManager.Instance.playerObject.GetComponent<PlayerController>().canJump = true;
                    finished = true;
                    this.gameObject.SetActive(false);
                }
            } 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerIn = false;
            finished = true;
            this.gameObject.SetActive(false);
        }
    }
}
