using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.playerController.canJump = false;
        GameManager.Instance.playerController.canMove = false;
        GameManager.Instance.playerController.canUseBulletTime = false;
        GameManager.Instance.playerRigidbody.useGravity = false;
    }


}
