using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class Aibase: MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; } 
        public ThirdPersonCharacter character { get; private set; }

        public enum State {
            PATROL,
            CHASE
        }


        public State state;
        private bool alive;

        //Patrol
        public GameObject[] waypoints;
        private int waypointInd = 0;
        public float patrolSpeed = 0.5f;

        //Chase
        public float chaseSpeed = 1f;
        public GameObject target;

        void Start()
        {
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

            agent.updatePosition = true;
            agent.updateRotation = false;


            state = Aibase.State.PATROL;

            alive = true;

            StartCoroutine("FSM");
        }

        IEnumerator FSM()
        {
            while (alive)
            {
                switch (state)
                {
                    case State.PATROL:
                        Patrol();
                        break;
                    case State.CHASE:
                        Chase();
                        break;
                }
                yield return null;
            }
        }

        void Patrol()
        {
            agent.speed = patrolSpeed;
            if (Vector3.Distance (this.transform.position, waypoints[waypointInd].transform.position) >= 2)
            {
                agent.SetDestination(waypoints[waypointInd].transform.position);
                character.Move(agent.desiredVelocity, false, false);
            }
            else if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) <=2)
            {
                waypointInd += 1;
                if (waypointInd > waypoints.Length)
                {
                    waypointInd = 0;
                }
            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }
        }

        void Chase()
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(target.transform.position);
            character.Move(agent.desiredVelocity, false, false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                state = Aibase.State.CHASE;
                target = other.gameObject;
            }
        }
        //public float HealPoint;

        //public Transform Player;

        //public float AgrRang;
        //public float speed;
        //public float rangHit;

        //private Animator anim;

        //private Rigidbody rb;


        //void Update()
        //{
        //    if (Vector3.Distance(Player.position, transform.position) <= AgrRang)
        //    {
        //        AggressiveStatus();
        //    }
        //    else
        //    {
        //        CalmStatus();
        //    }


        //    if (HealPoint <= 0)
        //    {
        //        Die();
        //    }
        //}

        //void Die()
        //{
        //    Destroy(gameObject);
        //}

        //void CalmStatus()
        //{

        //}

        //void AggressiveStatus()
        //{
        //    transform.LookAt(Player);
        //    if (Vector3.Distance(Player.position, transform.position) <= rangHit)
        //    {
        //        //anim.CrossFade("Hit");
        //    }
        //    else
        //    {
        //        rb.velocity = transform.InverseTransformDirection(Vector3.forward * speed * Time.deltaTime);
        //        //transform.Translate(new Vector3(0, 0, 1 * Time.deltaTime * speed));
        //    }
        //}

        //void OnTriggerEnter(Collider col)
        //{
        //    if (col.tag == "Sword")
        //    {
        //        Die();
        //    }
        //}
    }
}

