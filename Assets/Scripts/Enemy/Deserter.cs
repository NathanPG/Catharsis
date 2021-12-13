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
        deserterAnimator = GetComponent<Animator>();
        DisableRagdoll();
    }

    private void Start()
    {
        
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
            deserterAnimator.SetBool("Attacking", false);
            state = State.Idle;
        }

        //Aggressive
        else if (canAttackPalyer)
        {
            deserterAnimator.SetBool("Attacking", true);
            state = State.Attacking;
        }
        else
        {
            deserterAnimator.SetBool("Attacking", false);
            state = State.Idle;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") && !isDead)
        {
                isDead = true;
                GameManager.Instance.TimeStopEffect();

                deserterAnimator.Play("Death0");

                //GetComponent<AudioSource>().PlayOneShot(hitList[Random.Range(0, hitList.Count)]);
        }
    }

    private void DisableAnimator()
    {
        deserterAnimator.enabled = false;
        EnableRagdoll();
    }

    private void CheckPlayer()
    {
        Vector3 playerCenter = GameManager.Instance.playerObject.GetComponent<MovementControl>().worldCharacterCenter;
        float dist = Vector3.Distance(transform.position, playerCenter);
        //Debug.Log(dist);
        if(dist < 1.4f)
        {
            //Push Player Back
            Vector3 direction = playerCenter - transform.position;
            GameManager.Instance.playerRigidbody.AddForce(direction.normalized * pushBackForce, ForceMode.Impulse);
        }
    }
    public void DisableRagdoll()
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
        GetComponent<CapsuleCollider>().isTrigger = false;
        deserterAnimator.enabled = true;
    }

    public void EnableRagdoll()
    {
        deserterAnimator.enabled = false;
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
