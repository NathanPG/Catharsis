using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Rendering.PostProcessing;

public class Skills : MonoBehaviour
{
    public PostProcessVolume defaultVolume;
    public PostProcessVolume theWorldVolume;
    private InputDevice leftHand;
    private InputDevice rightHand;
    private bool bulletTimePressed = false;
    // Update is called once per frame
    void Update()
    {
        if ((leftHand.isValid && rightHand.isValid) == false)
        {
            BindDevice();
            return;
        }
        CheckBulletTime();
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

    void CheckBulletTime()
    {
        bool triggerValue;
        //tigger is being pressed
        if (rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue)
        {
            Time.timeScale = 0.1f;
            if (!bulletTimePressed)
            {
                //Call Once
                bulletTimePressed = true;
                //TODO: VISUAL
            }
        }
        else if (bulletTimePressed)
        {
            //Call Once
            bulletTimePressed = false;
            //TODO: VISUAL
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private IEnumerator 
}
