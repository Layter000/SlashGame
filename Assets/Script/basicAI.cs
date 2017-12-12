using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//namespace UnityStandardAssets.Characters.ThirdPerson
//{


//}

public class basicAI : BasicUnit
{
    private UnityEngine.AI.NavMeshAgent agent;
    private Rigidbody rb;

    public enum State
    {
        IDLE,
        CHASE,
        DAMADED
    }

    public enum Skills
    {
        PUNCH,
        JUMPATTACK
    }

    public State state;
    private bool alive;

    //Attack
    private bool isAttack = false;

    //Panch
    public float punchRange = 2f;
    public GameObject punchCollider;

    //JumpAttack
    private bool isCooldownJumpAttack = false;
    private float jumpAttacCooldown = 5;
    public GameObject jampAttackCollider;
    public float jumpAttackRange = 4f;
    public float jumpAttackVelocity = 1000f;

    private AnimationClip thisClip ;
    //Chase
    public float chaseSpeed = 1f;
    public GameObject target;

    private bool isLookPlayer;

    float timeTest = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();


        agent.updatePosition = true;
        agent.updateRotation = false;

        state = basicAI.State.IDLE;

        alive = true;

        StartCoroutine("FSM");

    }

    IEnumerator FSM()
    {
        while (alive)
        {
            switch (state)
            {
                case State.IDLE:
                    Idle();
                    break;
                case State.CHASE:
                    Chase();
                    break;
                case State.DAMADED:
                    break;
            }
            yield return null;
        }
    }
    void Idle()
    {
        
    }

    public override void Damaged()
    {
        state = basicAI.State.DAMADED;
        StartCoroutine(DamadedAnim());
    }

    public override void Die()
    { 
        alive = false;
        base.Die();
    }

    IEnumerator DamadedAnim()
    {
        anim.SetTrigger("Damaded");
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        state = basicAI.State.CHASE;
    }

    private void StopSpeed(bool isStopSpeed)
    {
        if (isStopSpeed)
        {
            agent.speed = 0;
        }
        else
        {
            agent.speed = chaseSpeed;
        }
    }

    IEnumerator Punch()
    {
        isAttack = true;
        StopSpeed(true);

        anim.SetTrigger("Punch");
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        StopSpeed(false);
        isAttack = false;
        state = basicAI.State.CHASE;

    }

    IEnumerator JumpAttack()
    {
        isAttack = true;
        isCooldownJumpAttack = true;
        anim.SetTrigger("JumpAttack");

        //yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        isAttack = false;
        state = basicAI.State.CHASE;

        yield return new WaitForSeconds(jumpAttacCooldown);
        isCooldownJumpAttack = false;
    }

    void Chase()
    {
        if (target != null)
        {
            if (!isAttack)
            {

                float distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);
                float angelToPlayer = Vector3.Angle(target.transform.position - transform.position, transform.forward);

                if (angelToPlayer <= 10f)
                {
                    anim.SetBool("IsRunning", false);
                    if (distanceToPlayer <= jumpAttackRange && !isCooldownJumpAttack)
                    {
                        StartCoroutine(JumpAttack());
                    }
                    else if (distanceToPlayer <= punchRange)
                    {
                        StartCoroutine(Punch());
                    }
                }
                else
                {

                    anim.SetBool("IsRunning", true);
                    agent.speed = chaseSpeed;
                    Turning();
                    agent.SetDestination(target.transform.position);
                }
            }
        }
    }
    void Turning()
    {
            Vector3 unitToTarget = target.transform.position - transform.position;
            unitToTarget.y = 0f;
            Quaternion newRotation = Quaternion.LookRotation(unitToTarget);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(unitToTarget), 700 * Time.deltaTime);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && target == null)
        {
            state = basicAI.State.CHASE;
            target = other.gameObject;
        }
    }

}

