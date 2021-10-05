using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MovementControl : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform rigTransform;
    private PlayerController playerController;

    private Rigidbody rigidbody = null;
    private CapsuleCollider capsuleCollider = null;
    private CharacterController characterController = null;
    private GameObject head = null;

    //Movement
    //private float moveSpeed;
    public float maxMoveSpeed;
    private bool snapPressed = false;

    //Jump
    public bool isGrounded = true;
    public float jumpForce;
    public float jumpCD;
    private bool jumpPressed = false;
    //private Vector3 characterCenter;
    private Vector3 worldCharacterCenter;

    //Grapple
    public Grapple grappleController;
    private bool grapplePressed = false;
    private bool grapplling = false;

    //Wall Running
    public LayerMask whatIsWall;
    public float wallRunForce, maxWallSpeed;
    [HideInInspector] public bool isWallRight, isWallLeft;
    public bool isWallRunning;

    //TESTING
    //public float testHeight;
    //public Vector3 testForward;

    //Sliding
    private bool dashPressed = false;
    public float dashForce;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        head = GetComponent<XRRig>().cameraGameObject;
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Start()
    {
        isWallRunning = false;
        
        PositionController();
    }

    // Update is called once per frame
    void Update()
    {
        //testHeight = head.transform.localPosition.y;
        PositionController();
        CheckWall();
        if (playerController.canMove && !isWallRunning )
        {
            CheckMovement();
        }
        CheckSnapTurn();
        CheckJump();
        CheckDash();
        CheckWallRun();
        CheckGrapple();
    }

    IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(0.3f);
        playerController.canJump = true;
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

        worldCharacterCenter.x = head.transform.position.x;
        worldCharacterCenter.z = head.transform.position.z;
        worldCharacterCenter.y = transform.position.y + newCenter.y;
    }

    void CheckWall()
    {
        isWallRight = Physics.Raycast(new Vector3(worldCharacterCenter.x, worldCharacterCenter.y, worldCharacterCenter.z)
            , cameraTransform.right, 1f, whatIsWall);

        Debug.DrawRay(new Vector3(worldCharacterCenter.x, worldCharacterCenter.y, worldCharacterCenter.z), cameraTransform.right, Color.green, 1f);

        isWallLeft = Physics.Raycast(new Vector3(worldCharacterCenter.x, worldCharacterCenter.y, worldCharacterCenter.z)
            , -cameraTransform.right, 1f, whatIsWall);

        Debug.DrawRay(new Vector3(worldCharacterCenter.x, worldCharacterCenter.y, worldCharacterCenter.z), -cameraTransform.right, Color.green, 1f);
    }

    void CheckWallRun()
    {
        //START WALL RUN
        if (isWallRight)
        {
            if (playerController.leftHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axisValue) &&
                playerController.leftHand.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out bool value))
            {
                if (value && axisValue.x > 0)
                {
                    //RIGHT
                    StartWallRun();
                }
            }
        }
        else if (isWallLeft)
        {
            if (playerController.leftHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axisValue1) &&
                playerController.leftHand.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out bool value1))
            {
                if (value1 && axisValue1.x < 0)
                {
                    //LEFT
                    StartWallRun();
                }
            }
        }

        //STOP WALL RUN
        if (!isWallLeft && !isWallRight && isWallRunning)
        {
            StopWallRun();
        }

        else if (playerController.leftHand.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out bool value2))
        {
            if (isWallRunning && value2 == false)
            {
                StopWallRun();
            }
        }
    }

    void StartWallRun()
    {
        rigidbody.useGravity = false;
        isWallRunning = true;
        playerController.canMove = false;
        
        if (rigidbody.velocity.magnitude <= maxWallSpeed)
        {
            rigidbody.AddForce(cameraTransform.transform.TransformDirection(Vector3.forward) * wallRunForce * Time.deltaTime);
        }
        if (isWallRight)
        {
            rigidbody.AddForce(cameraTransform.right * wallRunForce / 5 * Time.deltaTime);
        }
        else if (isWallLeft)
        {
            rigidbody.AddForce(-cameraTransform.right * wallRunForce / 5 * Time.deltaTime);
        }
        else
        {
            Debug.LogError("什么猫猫，WALL不在左也不在右，你在搞锤子？");
        }
    }

    void StopWallRun()
    {
        rigidbody.useGravity = true;
        playerController.canMove = true;
        isWallRunning = false;
    }

    void CheckMovement()
    {
        Vector2 primary2dValue;
        
        InputFeatureUsage<Vector2> primary2DVector = CommonUsages.primary2DAxis;
        if (playerController.leftHand.TryGetFeatureValue(primary2DVector, out primary2dValue) && primary2dValue != Vector2.zero)
        {
            var xAxis = primary2dValue.x * maxMoveSpeed * Time.deltaTime;
            var zAxis = primary2dValue.y * maxMoveSpeed * Time.deltaTime;
            //Check everything
            bool wallRight = Physics.Raycast(new Vector3(worldCharacterCenter.x, worldCharacterCenter.y, worldCharacterCenter.z)
            , cameraTransform.right, 1f);
            bool wallLeft = Physics.Raycast(new Vector3(worldCharacterCenter.x, worldCharacterCenter.y, worldCharacterCenter.z)
            , -cameraTransform.right, 1f);
            //Left/Right movement
            if ((!wallRight && xAxis > 0) || (!wallLeft && xAxis < 0))
            {
                transform.position += cameraTransform.transform.TransformDirection(Vector3.right) * xAxis;
            }
            Vector3 forwardVector = cameraTransform.transform.TransformDirection(Vector3.forward) * zAxis;
            forwardVector = new Vector3(forwardVector.x, 0f, forwardVector.z);
            transform.position += forwardVector;
            
            //transform.position += cameraTransform.transform.forward * zAxis;
        }
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
        isGrounded = Physics.Raycast(new Vector3(worldCharacterCenter.x, transform.position.y + 1f, worldCharacterCenter.z)
            , Vector3.down,1f);
        Debug.DrawRay(new Vector3(worldCharacterCenter.x, transform.position.y + 1f, worldCharacterCenter.z), Vector3.down, Color.white, 1f);

        if (!playerController.canJump) return;

        //Normal Jump
        if (!isWallRunning && isGrounded)
        {
            bool triggerValue;
            //tigger is being pressed
            if (playerController.leftHand.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue)
            {
                if (!jumpPressed)
                {
                    playerController.canJump = false;
                    StartCoroutine(ResetJump());
                    jumpPressed = true;
      
                    //TODO: NORMAL VECTOR
                    rigidbody.AddForce(Vector3.up * jumpForce);

                    //TODO: JUMP TOWARDS SPEED DIRECTION
                    rigidbody.AddForce(cameraTransform.forward * jumpForce /3);
                }
            }
            //trigger is not being pressed, but jump has not been reset
            else if (jumpPressed)
            {
                jumpPressed = false;
            }
        }

        
        //Wall Jump
        else if (isWallRunning)
        {
            bool triggerValue;
            //tigger is being pressed
            if (playerController.leftHand.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue)
            {
                if (!jumpPressed)
                {
                    playerController.canJump = false;
                    StartCoroutine(ResetJump());
                    jumpPressed = true;
                    rigidbody.AddForce(Vector3.up * jumpForce);
                    rigidbody.AddForce(cameraTransform.forward * jumpForce / 10);
                    if (isWallLeft)
                    {
                        //Debug.Log("Left Wall Jump");
                        rigidbody.AddForce(cameraTransform.right * jumpForce/3);
                    }
                    else if (isWallRight)
                    {
                        //Debug.Log("Right Wall Jump");
                        rigidbody.AddForce(-cameraTransform.right * jumpForce/3);
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
        if (playerController.leftHand.TryGetFeatureValue(CommonUsages.gripButton, out grappleValue) && grappleValue)
        {
            if (!grapplePressed)
            {
                grapplePressed = true;

                if (!grapplling)
                {
                    //Debug.Log("Grapple");
                    grapplling = true;
                    characterController.enabled = false;
                    grappleController.StartGrapple();
                    
                }
                else
                {
                    //Debug.Log("Grapple Released");
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

    void CheckDash()
    {
        bool gripValue;
        //tigger is being pressed
        if (playerController.rightHand.TryGetFeatureValue(CommonUsages.gripButton, out gripValue) && gripValue)
        {
            if (!dashPressed)
            {
                dashPressed = true;
                rigidbody.AddForce(cameraTransform.forward * jumpForce);
            }
        }
        //trigger is not being pressed, but jump has not been reset
        else if (dashPressed)
        {
            dashPressed = false;
        }
    }
    
    //INCLUDED WITH XR PACKAGE
    void CheckSnapTurn()
    {
        //Right pad to control snap turns
        bool padBool;
        Vector2 primary2dValue;

        //IF PAD IS CLICKED
        if (playerController.rightHand.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out padBool) && padBool)
        {
            playerController.rightHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out primary2dValue);
            if (!snapPressed)
            {
                snapPressed = true;
                if (primary2dValue.x < 0)
                {
                    rigTransform.RotateAround(head.transform.position, Vector3.up, -45);
                }
                else
                {
                    rigTransform.RotateAround(head.transform.position, Vector3.up, 45);
                }
            }
            /*
            if (playerController.rightHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out primary2dValue) && primary2dValue != Vector2.zero)
            {
                
            }
            */   
        }
        else if (snapPressed)
        {
            snapPressed = false;
        }
    }

    public void Ramp(Vector3 rampDirection)
    {
        if (rigidbody.velocity.magnitude <= maxWallSpeed)
        {
            rigidbody.AddForce(rampDirection * wallRunForce * Time.deltaTime);
        }
    }

    public void ToggleGravity(bool isOn)
    {
        if (isOn)
        {
            rigidbody.useGravity = true;
        }
        else
        {
            rigidbody.useGravity = false;
        }
    }
}
