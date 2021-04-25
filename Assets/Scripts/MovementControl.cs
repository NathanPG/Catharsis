using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MovementControl : LocomotionProvider
{
    public ContinuousMoveProviderBase continuousMoveProvider;
    public Transform cameraTransform;
    private Rigidbody rigidbody;
    private CapsuleCollider capsuleCollider;

    private CharacterController characterController = null;
    private GameObject head = null;

    private InputDevice leftHand;
    private InputDevice rightHand;

    //Jump
    public bool isGrounded = true;
    public float jumpForce;
    public float jumpCD;
    private bool jumpPressed = false;
    //private bool canJump = true;
    private Vector3 characterCenter;
    bool ForceCleared = true;

    //Grapple
    public Grapple grappleController;
    private bool grapplePressed = false;
    private bool grapplling = false;

    //Wall Running
    public LayerMask whatIsWall;
    public float wallRunForce, maxWallSpeed;
    public bool isWallRight, isWallLeft;
    public bool isWallRunning;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        head = GetComponent<XRRig>().cameraGameObject;
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);
    }

    // Start is called before the first frame update
    void Start()
    {
        isWallRunning = false;
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        PositionController();
    }

    // Update is called once per frame
    void Update()
    {
        PositionController();

        if((leftHand.isValid && rightHand.isValid) == false)
        {
            BindDevice();
            return;
        }
        CheckJump();
        CheckWall();
        CheckWallRun();
        CheckGrapple();
    }

    void CheckWallRun()
    {
        if (isWallRight)
        {
            if (leftHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axisValue))
            {
                if (leftHand.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out bool value))
                {
                    if (value)
                    {
                        //RIGHT
                        if (axisValue.x > 0)
                        {
                            StartWallRun();
                        }
                    }
                }
            }
        }
        if (isWallLeft)
        {   
            if (leftHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axisValue1))
            {
                if(leftHand.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out bool value1))

                    if (value1)
                    {
                        //LEFT
                        if (axisValue1.x < 0)
                        {
                            StartWallRun();
                        }
                    }
            }
        }
    }

    void StartWallRun()
    {
        rigidbody.useGravity = false;
        isWallRunning = true;
        continuousMoveProvider.useGravity = false;

        if (rigidbody.velocity.magnitude <= maxWallSpeed)
        {
            rigidbody.AddForce(cameraTransform.forward * wallRunForce * Time.deltaTime); 
        }
        if (isWallRight)
        {
            rigidbody.AddForce(cameraTransform.right * wallRunForce / 5 * Time.deltaTime);
        }
        else
        {
            rigidbody.AddForce(-cameraTransform.right * wallRunForce / 5 * Time.deltaTime);
        }
    }

    void StopWallRun()
    {

        //rigidbody.useGravity = true;

        isWallRunning = false;
        continuousMoveProvider.useGravity = true;
    }

    void CheckWall()
    {
        isWallRight = Physics.Raycast(new Vector3(transform.position.x + characterCenter.x, transform.position.y + characterCenter.y, transform.position.z + characterCenter.z)
            , cameraTransform.right, 1f, whatIsWall);

        //Debug.DrawRay(new Vector3(transform.position.x + characterCenter.x, transform.position.y + characterCenter.y, transform.position.z + characterCenter.z), cameraTransform.right, Color.green, 1f);

        isWallLeft = Physics.Raycast(new Vector3(transform.position.x + characterCenter.x, transform.position.y + characterCenter.y, transform.position.z + characterCenter.z)
            , -cameraTransform.right, 1f, whatIsWall);

        //Debug.DrawRay(new Vector3(transform.position.x + characterCenter.x, transform.position.y + characterCenter.y, transform.position.z + characterCenter.z), -cameraTransform.right, Color.green, 1f);

        if (!isWallLeft && !isWallRight) StopWallRun();

    }

    /// <summary>
    /// Update the player's position according to the physical body movement
    /// </summary>
    void PositionController()
    {
        //Restrict the head height
        float headHeight = Mathf.Clamp(head.transform.localPosition.y, 1, 2);
        characterController.height = headHeight;
        capsuleCollider.height = headHeight;

        Vector3 newCenter = Vector3.zero;
        newCenter.y = characterController.height / 2;
        newCenter.y += characterController.skinWidth;


        newCenter.x = head.transform.localPosition.x;
        newCenter.z = head.transform.localPosition.z;

        characterController.center = newCenter;
        capsuleCollider.center = newCenter;
        characterCenter = newCenter;
    }

    /*
    IEnumerator ResetJump(float cd)
    {
        yield return new WaitForSeconds(cd);
        if (canJump) Debug.LogError("canJump before reset in coroutine");
        canJump = true;
    }
    */

    /// <summary>
    /// Check jump input: Trigger
    /// </summary>
    void CheckJump()
    {
        isGrounded = Physics.Raycast(new Vector3(transform.position.x + characterCenter.x, transform.position.y + 0.1f , transform.position.z + characterCenter.z)
            , Vector3.down,1f);
        //Debug.DrawRay(new Vector3(transform.position.x + characterCenter.x, transform.position.y + 0.1f, transform.position.z + characterCenter.z)
        //    , Vector3.down, Color.red, 1f);

        //IN AIR
        if (!isGrounded && !isWallRunning)
        {
            rigidbody.useGravity = true;
            return;
        }
        //Landed
        else
        {
            if (isGrounded)
            {
                //Right after Landing
                if (!ForceCleared)
                {
                    Debug.Log("Clear Force");
                    ForceCleared = true;
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.angularVelocity = Vector3.zero;
                }
                rigidbody.useGravity = false;
            }  
        }

        
        //Normal Jump
        if (!isWallRunning)
        {
            bool triggerValue;
            //tigger is being pressed
            if (leftHand.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue)
            {
                if (!jumpPressed)
                {
                    jumpPressed = true;
                    //canJump = false;
                    //TODO: NORMAL VECTOR
                    rigidbody.AddForce(Vector3.up * jumpForce);
                }
            }
            //trigger is not being pressed, but jump has not been reset
            else if (jumpPressed)
            {
                jumpPressed = false;
            }
        }

        
        //Wall Jump
        else
        {
            bool triggerValue;
            //tigger is being pressed
            if (leftHand.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue)
            {
                if (!jumpPressed)
                {
                    jumpPressed = true;
                    ForceCleared = false;
                    rigidbody.AddForce(Vector3.up * jumpForce);
                    rigidbody.AddForce(cameraTransform.forward * jumpForce);
                    if (isWallLeft)
                    {
                        Debug.Log("Left Wall Jump");
                        rigidbody.AddForce(cameraTransform.right * jumpForce);
                    }
                    else if (isWallRight)
                    {
                        Debug.Log("Right Wall Jump");
                        rigidbody.AddForce(-cameraTransform.right * jumpForce);
                    }
                }
            }
            //trigger is not being pressed, but jump has not been reset
            else if (jumpPressed)
            {
                jumpPressed = false;
            }
        }
    }

    void CheckGrapple()
    {
        bool grappleValue;
        //tigger is being pressed
        if (leftHand.TryGetFeatureValue(CommonUsages.gripButton, out grappleValue) && grappleValue)
        {
            if (!grapplePressed)
            {
                if (!grapplling)
                {
                    Debug.Log("Grapple");
                    grapplling = true;
                    grapplePressed = true;
                    characterController.enabled = false;
                    grappleController.StartGrapple();
                    
                }
                else
                {
                    Debug.Log("Grapple Released");
                    grapplling = false;
                    characterController.enabled = true;
                    grappleController.StopGrapple();
                }
            }
        }
        //trigger is not being pressed, but jump has not been reset
        else if (grapplePressed)
        {
            grapplePressed = false;
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


}
