using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Deserter : EnemyBase
{
    public bool shouldAttack = false, shouldRespawn = true;
    public GameObject ragdollObject;
    public State state;
    public float pushBackForce;

    private Animator deserterAnimator;

    private void Awake()
    {
        DisableRagdoll();
    }
    private void Start()
    {
        
        deserterAnimator = GetComponent<Animator>();
        startingPosition = transform.position;
        state = State.Idle;
        player = Camera.main.transform;

    }
    
    void Update()
    {
        if (isDead) return;
        
        canAttackPalyer = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


        if (!shouldAttack)
        {
            //STAY
            state = State.Idle;
        }

        //Aggressive
        else if (canAttackPalyer)
        {
            
            state = State.Attacking;
        }
        else
        {
            
            state = State.Idle;
        }

        switch (state)
        {
            case State.Attacking:
                deserterAnimator.SetBool("Attacking", true);
                Vector3 lookVector = player.position - transform.position;
                lookVector.y = transform.position.y;
                Quaternion rot = Quaternion.LookRotation(lookVector);
                rot.x = 0;
                rot.z = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1);
                break;
            case State.Idle:
                deserterAnimator.SetBool("Attacking", false);
                break;
        }
        
    }

    /*
    private void FixedUpdate()
    {
        if (isDead) return;

        if (!groundDetector.isGrounded) Debug.Log("NOT GROUNDED");

        if (!groundDetector.isGrounded && !lostTrack)
        {
            Debug.Log("Lost Track!");
            lostTrack = true;
            agent.enabled = false;
            deserterAnimator.enabled = false;
            //GetComponent<Rigidbody>().isKinematic = false;
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
    */



    private void OnDrawGizmosSelected()
    {
        //Gizmos.DrawLine(transform.position, transform.position - Vector3.up * 2f);
    }

    private void Respawn()
    {
        transform.position = startingPosition;
        deserterAnimator.enabled = true;

        //GetComponent<Rigidbody>().isKinematic = true;

        isDead = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") && !isDead)
        {
                isDead = true;
                GameManager.Instance.TimeStopEffect();
                //TODO: RANDOM DEATH ANIM
                deserterAnimator.Play("Death0");

            //GetComponent<AudioSource>().PlayOneShot(hitList[Random.Range(0, hitList.Count)]);
        }
    }

    private void DisableAnimator()
    {
        deserterAnimator.enabled = false;
        EnableRagdoll();
    }

    private void DisableRagdoll()
    {
        var colsChildren = ragdollObject.GetComponentsInChildren<Collider>();
        var rigsChildren = ragdollObject.GetComponentsInChildren<Rigidbody>();
        foreach (Collider c in colsChildren)
        {
            c.enabled = false;
        }
        foreach (Rigidbody r in rigsChildren)
        {
            r.isKinematic = true;
        }
    }

    private void EnableRagdoll()
    {
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

    private void CheckPlayer()
    {
        Vector3 playerCenter = GameManager.Instance.playerObject.GetComponent<MovementControl>().worldCharacterCenter;
        float dist = Vector3.Distance(transform.position, playerCenter);
        Debug.Log(dist);
        if(dist < 1.4f)
        {
            //Push Player Back
            Vector3 direction = playerCenter - transform.position;
            GameManager.Instance.playerRigidbody.AddForce(direction.normalized * pushBackForce, ForceMode.Impulse);
        }
    }
}
