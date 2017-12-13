using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAI : BasicUnit
{

    private bool alive = true;

    public GameObject target;

    public enum State
    {
        IDLE,
        CHASE,
        DAMADED
    }

    private Rigidbody rb;

    public Transform spawnBulet;
    public GameObject bulet;

    private bool cooldownAttack = false;

    public State state;

    // Use this for initialization
    void Awake() {
        //anim.SetTrigger("Attack");
        StartCoroutine(FSM());
    }
	
    IEnumerator FSM()
    {
        while (alive)
        {
            switch (state)
            {
                case State.IDLE:
                    //Idle();
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
    public void Damaded()
    {
        StartCoroutine(DamadedAnim());
    }

    private void Chase()
    {
        if (target != null)
        {
            Turning();
            if (!cooldownAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        GameObject clone;
        cooldownAttack = true;
        anim.SetTrigger("Attack");
        clone = Instantiate(bulet, spawnBulet.position, Quaternion.identity) as GameObject;
        clone.GetComponent<Rigidbody>().AddForce((target.transform.position - transform.position) * 100);
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(2);
        cooldownAttack = false;
    }
    IEnumerator DamadedAnim()
    {
        StopCoroutine(Attack());
        cooldownAttack = true;
        anim.SetTrigger("Damaded");
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        cooldownAttack = false;
    }

    void Turning()
    {
        Vector3 unitToTarget = target.transform.position - transform.position;
        unitToTarget.y = 0f;
        Quaternion newRotation = Quaternion.LookRotation(unitToTarget);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(unitToTarget), 3000 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && target == null)
        {
            state = StayAI.State.CHASE;
            target = other.gameObject;
        }
    }
}
