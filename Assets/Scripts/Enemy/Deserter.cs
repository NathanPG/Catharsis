using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Deserter : EnemyBase
{
    public bool shouldAttack = false, shouldRespawn = true;

    public State state;
    public float pushBackForce;

    private Animator deserterAnimator;

    private void Start()
    {
        deserterAnimator = GetComponent<Animator>();
        startingPosition = transform.position;
        state = State.Idle;
        player = Camera.main.transform;
        deserterAnimator.SetBool("Idle", true);
    }
    /*
    void Update()
    {
        
        if (dead) return;
        
        canAttackPalyer = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


        if (!shouldAttack)
        {
            //STAY
            state = State.Idle;
        }

        //Aggressive
        else if (canAttackPalyer)
        {
            //meleeAnimator.SetBool("Attacking", true);
            state = State.Attacking;
        }
        else
        {
            //meleeAnimator.SetBool("Attacking", false);
            state = State.Idle;
            //CONTINUE TO MOVE
        }

        switch (state)
        {
            case State.Attacking:
                //agent.isStopped = true;
                //Attack();
                break;
            case State.Idle:
                //meleeAnimator.SetBool("Idle", true);
                //agent.isStopped = true;
                break;
        }
        
    }
    */

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

    /*
    public void Death()
    {
        //Touch Lava
        isDead = true;
        //Respawn
        //Invoke("Respawn", 1f);
    }
    */

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
