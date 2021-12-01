using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Examinee : EnemyBase
{
    public bool shouldMove = true, shouldRespawn = true;
    public GroundDetector groundDetector = null;
    public Transform destBox;
    public GameObject ragdollObject;
    public State state;

    private Animator examineeAnimator;
    private Vector3 destPosition;
    private bool lostTrack = false;

    private void Awake()
    {
        DisableRagdoll();
    }

    private void Start()
    {
        DisableRagdoll();
        examineeAnimator = GetComponent<Animator>();
        startingPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        state = State.Idle;
        player = Camera.main.transform;
        canMove = true;
        destPosition = destBox.position;
        StartCoroutine(SetDest(destPosition, 3f));
    }
    // Update is called once per frame
    void Update()
    {
        /*
        if (dead) return;
        
        canAttackPalyer = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!shouldMove)
        {
            //STAY
            state = State.Idle;
        }
        else if (canAttackPalyer)
        {
            //meleeAnimator.SetBool("Attacking", true);
            state = State.Attacking;
        }
        else
        {
            //meleeAnimator.SetBool("Attacking", false);
            state = State.Approaching;
            //CONTINUE TO MOVE
        }

        switch (state)
        {
            case State.Approaching:
                //agent.isStopped = false;
                //meleeAnimator.SetBool("Idle", false);
                //meleeAnimator.Play("Forward");
                //Approach();
                break;
            case State.Attacking:
                //agent.isStopped = true;
                //Attack();
                break;
            case State.Idle:
                //meleeAnimator.SetBool("Idle", true);
                //agent.isStopped = true;
                break;
        }
        */
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        if (!groundDetector.isGrounded && !lostTrack)
        {
            lostTrack = true;
            agent.enabled = false;
            examineeAnimator.enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;
            EnableRagdoll();
        }

        RaycastHit r;
        if (Physics.Raycast(transform.position, Vector3.down, out r, 2f))
        {
            
            if (r.transform.tag == "Lava" && !isDead)
            {
                Debug.Log("Touched Lava");
                isDead = true;
                Death();
            }
        }
    }

    private void Respawn()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        DisableRagdoll();
        agent.enabled = true;
        agent.Warp(startingPosition);
        examineeAnimator.enabled = true;
        lostTrack = false;
        
        StartCoroutine(SetDest(destPosition, 3f));
        isDead = false;
    }

    public void Death()
    {
        //Touch Lava
        isDead = true;
        //Respawn
        Invoke("Respawn", 1f);
    }

    private void DisableRagdoll()
    {
        var colsChildren = ragdollObject.GetComponentsInChildren<Collider>();
        var rigsChildren = ragdollObject.GetComponentsInChildren<Rigidbody>();
        foreach(Collider c in colsChildren)
        {
            c.enabled = false;
        }
        foreach (Rigidbody r in rigsChildren)
        {
            r.isKinematic = true;
        }
        GetComponent<CapsuleCollider>().isTrigger = false;
    }

    private void EnableRagdoll()
    {
        GetComponent<CapsuleCollider>().isTrigger = true;
        var colsChildren = ragdollObject.GetComponentsInChildren<Collider>();
        var rigsChildren = ragdollObject.GetComponentsInChildren<Rigidbody>();
        foreach (Collider c in colsChildren)
        {
            c.enabled = true;
        }
        foreach (Rigidbody r in rigsChildren)
        {
            r.isKinematic = false;
        }
    }
}
