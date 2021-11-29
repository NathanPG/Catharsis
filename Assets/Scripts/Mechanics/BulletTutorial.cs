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
            GameManager.Instance.playerController.canMove = false;
            GameManager.Instance.playerController.canJump = false;
        }
    }

    private void Update()
    {
        if (isPlayerIn)
        {
            GameManager.Instance.playerController.canUseBulletTime = true;
            if (GameManager.Instance.playerController
                .rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed))
            {
                if (triggerPressed)
                {
                    GameManager.Instance.playerController.canMove = true;
                    GameManager.Instance.playerController.canJump = true;
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
