using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering;

public class Skills :MonoBehaviour
{
    public Volume defaultVolume;
    public Volume theWorldVolume;

    private PlayerController playerController;

    private bool bulletTimePressed = false;
    //Normal
    private bool canUseBulletTime = true;
    //Minimum CD
    private bool minCDMet = true;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    void Update()
    {
        
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
                playerController.rightHand = device;
            }
            else if (device.role.ToString() == "LeftHanded")
            {
                playerController.leftHand = device;
            }
        }
    }

    void CheckBulletTime()
    {
        bool triggerValue;
        //tigger is being pressed
        if (playerController.rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue)
        {
            if (!bulletTimePressed && canUseBulletTime)
            {
                //Call Once
                bulletTimePressed = true;
                
                //TODO: VISUAL
                canUseBulletTime = false;

                StartCoroutine(EnterTheWorld(0.2f));
            }

        }
        
        else if (bulletTimePressed)
        {
            //Call Once
            if (canUseBulletTime)
            {
                bulletTimePressed = false;
                Debug.Log("Exit Bullet");
                StartCoroutine(ExitTheWorld(0.2f));
            }
        }
    }

    private IEnumerator EnterTheWorld(float duration)
   {
        Debug.Log("Enter Bullet");
        float elapsedTime = 0;
        
        while (elapsedTime <= duration)
        {
            defaultVolume.weight = Mathf.Lerp(1, 0, elapsedTime / duration);
            theWorldVolume.weight = Mathf.Lerp(0, 1, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();

        }

        defaultVolume.weight = 0;
        theWorldVolume.weight = 1;
        defaultVolume.priority = 0;
        theWorldVolume.priority = 1;
        Time.timeScale = 0.1f;

        //SCALE IS 1, 0.2 = 2
        StartCoroutine(ResetBulletTime(0.2f));
    }

    private IEnumerator ExitTheWorld(float duration)
    {
        Debug.Log("Exit Bullet");
        Time.timeScale = 1f;
        float elapsedTime = 0;

        
        
        while (elapsedTime <= duration)
        {
            defaultVolume.weight = Mathf.Lerp(0, 1, elapsedTime / duration);
            theWorldVolume.weight = Mathf.Lerp(1, 0, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        defaultVolume.weight = 1;
        theWorldVolume.weight = 0;
        defaultVolume.priority = 1;
        theWorldVolume.priority = 0;
        
    }

    private IEnumerator ResetBulletTime(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Reset");
        canUseBulletTime = true;
    }
}
