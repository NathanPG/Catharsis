using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using Valve.VR;

public class IdleDog : MonoBehaviour
{
    public NavMeshAgent agent;


    public AudioSource audioSource;
    public bool shouldBark = true;
    public AudioClip[] audioClips;

    public Transform player;
    //public Messages msgs;

    public LayerMask whatIsGround, whatIsPlayer;

    public float endurance = 100f;
    public bool stunned = false;
    public float stuntime = 20f;


    private Animator animator;
    public Vector3 walkPoint;
    private Vector3 originalPoint;

    public float maxChaseRange;
    bool walkPointSet;
    public float walkPointRange;



    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSight, canAttackPalyer;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        originalPoint = transform.position;
        //msgs = FindObjectOfType<Messages>();
        //print(GameObject.FindGameObjectWithTag("Player").name);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Claws"))
        {
            return;
        }
        print("CLAWS!!");
        //SteamVR_Behaviour_Pose pose = other.gameObject.GetComponent<SteamVR_Behaviour_Pose>();
        if (pose.GetVelocity().y <= -1f)
        {
            BeAttacked();
        }
    }
    */

    public void BeAttacked()
    {
        endurance -= 35f;
        if (endurance <= 0f)
        {
            BeStunned();
            endurance = 0f;
        }
        else
        {
            animator.Play("Hit");
            Invoke(nameof(Resume), 1);
            Time.timeScale = 0.5f;
        }
    }

    public void BeStunned()
    {
        audioSource.volume = 1f;
        audioSource.PlayOneShot(audioClips[2]);


        animator.SetBool("stun", true);
        if (!stunned)
        {
            stunned = true;
            animator.Play("Stun");
            Invoke(nameof(ResetStun), stuntime);
            Invoke(nameof(Resume), 1);
            Time.timeScale = 0.5f;
        }
    }

    private void Resume()
    {
        Time.timeScale = 1;
    }

    private void ResetStun()
    {
        animator.SetBool("stun", false);
        stunned = false;
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
    }

    private void Attack()
    {
        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            //ATTACK
            transform.LookAt(player);
            animator.Play("Attack");
            if (canAttackPalyer)
            {
                audioSource.volume = 1f;
                audioSource.PlayOneShot(audioClips[1]);
                //msgs.DealDmgToPlayer(15);
            }
            
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    // Update is called once per frame
    void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        canAttackPalyer = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!stunned)
        {
            if (!playerInSight && !canAttackPalyer)
            {
                shouldBark = true;
                animator.SetBool("agression", false);
                animator.SetBool("chasing", false);


                agent.SetDestination(originalPoint);
                if ((transform.position - originalPoint).magnitude >= 1f)
                {
                    if (animator.GetBool("walk") == false || animator.GetCurrentAnimatorStateInfo(0).IsName("Aggression"))
                    {
                        animator.Play("Walk");
                    }
                    animator.SetBool("walk", true);
                    agent.speed = 0.5f;

                }
                else
                {
                    animator.SetBool("walk", false);
                    animator.SetBool("idle", true);
                    if (animator.GetBool("idle") == false || animator.GetCurrentAnimatorStateInfo(0).IsName("Aggression"))
                    {
                        animator.Play("Idle");
                    }
                }
            }
            if (playerInSight && !canAttackPalyer)
            {
                if (shouldBark)
                {
                    audioSource.volume = 1f;
                    audioSource.PlayOneShot(audioClips[0]);
                    shouldBark = false;
                }
                if ((transform.position - originalPoint).magnitude >= maxChaseRange)
                {
                    if (!walkPointSet)
                    {
                        agent.speed = 0.5f;
                        agent.SetDestination(originalPoint);
                        walkPointSet = true;
                    }
                }
                agent.speed = 3f;
                if (animator.GetBool("chasing") == false)
                {
                    animator.Play("Chase");
                }
                animator.SetBool("chasing", true);
                animator.SetBool("idle", false);
                animator.SetBool("agression", false);
                Chase();
            }
            if (playerInSight && canAttackPalyer)
            {
                agent.speed = 0f;
                agent.SetDestination(transform.position);
                animator.SetBool("idle", false);
                animator.SetBool("chasing", false);
                animator.SetBool("agression", true);
                Attack();
            }
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);


        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxChaseRange);
    }
}
