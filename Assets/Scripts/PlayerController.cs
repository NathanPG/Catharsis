using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerController : MonoBehaviour
{
    public InputDevice leftHand;
    public InputDevice rightHand;

    public bool inBulletTimePlayer = false;
    public float hp = 100;
    public bool dead;
    public GameObject weapon;

    //Skill
    public bool canUseBulletTime = true;

    //Move control
    public bool canMove = true;
    public bool canJump = true;
    public bool MoveState
    {
        get
        {
            return canMove;
        }
        set
        {
            canMove = value;
        }
    }

    public bool JumpState
    {
        get
        {
            return canJump;
        }
        set
        {
            canJump = value;
        }
    }


    private void Update()
    {
        if ((leftHand.isValid || rightHand.isValid) == false)
        {
            BindDevice();
            return;
        }
    }

    /// <summary>
    /// If any of the device is not available, try to bind the device
    /// </summary>
    void BindDevice()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);
        foreach (var device in inputDevices)
        {
            //Right Hand
            if (device.role.ToString() == "RightHanded")
            {
                rightHand = device;
            }
            else if (device.role.ToString() == "LeftHanded")
            {
                leftHand = device;
            }
        }
    }

    public void enableWeapon()
    {
        weapon.SetActive(true);
    }

}
