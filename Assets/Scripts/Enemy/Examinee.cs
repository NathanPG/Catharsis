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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") && !isDead)
        {
            isDead = true;
            GameManager.Instance.TimeStopEffect();

            examineeAnimator.Play("Death0");

            //GetComponent<AudioSource>().PlayOneShot(hitList[Random.Range(0, hitList.Count)]);
        }
    }

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
        StartCoroutine(SetDest(destPosition, Random.Range(3,10)));
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
                //Debug.Log("Touched Lava");
                isDead = true;
                Death();
            }
        }
    }

    private void Respawn()
    {
        //GetComponent<CapsuleCollider>().enabled = true;
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
        if (!isDead)
        {
            isDead = true;
            //GetComponent<CapsuleCollider>().enabled = false;
            Invoke("Respawn", Random.Range(1,10));
        }
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
