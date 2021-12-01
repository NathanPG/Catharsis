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
        examineeAnimator = GetComponent<Animator>();
        DisableRagdoll();
    }

    private void Start()
    {
        startingPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        player = Camera.main.transform;
        canMove = true;
        destPosition = destBox.position;
        StartCoroutine(SetDest(destPosition, 3f));
    }

    private void Update()
    {
        float dist = Vector3.Distance(transform.position, destPosition);

        if(dist < 2f)
        {
            state = State.Idle;
            examineeAnimator.SetBool("isApproaching", false);
        }
        else
        {
            state = State.Approaching;
            examineeAnimator.SetBool("isApproaching", true);
        }
    }

    public void ResetDestOrDie()
    {
        state = State.Approaching;

        if (agent.isOnNavMesh)
        {
            StartCoroutine(SetDest(destPosition, 0.5f));
        }
        else
        {
            DisableRagdoll();
            Invoke("Respawn", 2f);
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        if (!groundDetector.isGrounded && !lostTrack)
        {
            lostTrack = true;
            agent.enabled = false;
            
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

    public void DisableRagdoll()
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
        examineeAnimator.enabled = true;
    }

    public void EnableRagdoll()
    {
        examineeAnimator.enabled = false;
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
