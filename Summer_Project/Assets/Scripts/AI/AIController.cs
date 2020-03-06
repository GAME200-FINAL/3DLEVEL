using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour {

   
    public GameObject idleNotifier;
    public GameObject alertNotifier;
    public GameObject attackNotifier;


    public GameObject startArea;
    public float moveSpeed = 6;
    [Space(10)]
    public bool returnToStartAreaAfterDisengage = true;
    public bool canHangAround = false;
    public GameObject hangAroundPoint;
    public float hangAroundRadius;

    [Space(10)]
    public bool canPatrol = false;
    public List<GameObject> patrolWaypoints = new List<GameObject>();
    public float waitTime = 0;

    [Space(10)]
    public bool canAttackPlayer = true;
    public bool canChase = true;
    [Range(0,360)]
    public float fieldOfView ;
    public float frontDetectRange ;
    public float backDetectRange ;
    public float chaseRange ;
    public float chaseSpeed ;
    public float attackRange;

    private GameObject chasingTarget;
    private GameObject nearTarget;
    private NavMeshAgent navMeshAgent;
    private AIState currentState = AIState.Idle;
    private AIState lastState = AIState.Idle;
    private int currentWaypoint = 0;
    private float waitTimer;
    private float hangTimer = 0;
    private float hangTime = 1;
    private float attackTimer = 0;

    

    private enum AIState
    {
        Idle,
        Alert,
        Attacking
    }


    void Start()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

        SphereCollider detectRangeCollider = gameObject.GetComponent<SphereCollider>();
        detectRangeCollider.radius = frontDetectRange;
        detectRangeCollider.isTrigger = true;
    }

    private void FixedUpdate()
    {
        setAlertModifier(currentState);

        switch (currentState)
        {
            case AIState.Idle:
                if (canPatrol && patrolWaypoints.Count > 0)
                {
                    handlePatrolling();
                }
                else if (returnToStartAreaAfterDisengage && distanceToMe(startArea) > 0.2 && hangTimer == 0)
                {
                    walkTowardsTarget(startArea);
                }
                else if (canHangAround)
                {
                    handleHangAround(null);
                }
                break;
            case AIState.Alert:
                if (chasingTarget != null && isTargetStillInChaseRange())
                {
                    runTowardsTarget(chasingTarget);
                   
                    if (canAttackPlayer && (navMeshAgent.remainingDistance < attackRange))
                    {
                        currentState = AIState.Attacking;
                        break;
                    }
                   
                }
                else if (chasingTarget != null && !isTargetStillInChaseRange())
                {
                    chasingTarget = null;
                    currentState = AIState.Idle;
                }else if(chasingTarget == null)
                {
                    currentState = AIState.Idle;
                }
                break;
            case AIState.Attacking:
                if(attackTimer == 0)
                {
                    attackTimer = 2f;
                    //TODO start attack Animation
                }else if(attackTimer > 0)
                {
                    attackTimer -= Time.deltaTime;
                }else if (attackTimer < 0)
                {
                    currentState = AIState.Alert;
                }
                break;
        }
       
    }

    private bool isTargetStillInChaseRange()
    {
        float distance = distanceToMe(chasingTarget);
        return distance < chaseRange;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;

        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        navMeshAgent.SetDestination(hit.point);
        //    }
        //}
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.DrawRay(transform.position, (col.gameObject.transform.position - transform.position), Color.green);
        GameObject target = col.gameObject;
        if (target.tag.Equals("Player") && canChase && !isPlayerStealth(target))
        {
            Vector3 targetDir = target.transform.position - transform.position;
            float angle = Vector3.Angle(targetDir, transform.forward);
            print(angle + " " + distanceToMe(target));
            if ((angle > getFOV()) && (nearTarget == null))
            {
                if (distanceToMe(target) < backDetectRange)
                {
                    makeAIChaseTarget(target);
                    return;
                }
                nearTarget = target;
                return;
            }
            makeAIChaseTarget(target);          
        }
    }

    private void OnTriggerStay(Collider col)
    {
        Debug.DrawRay(transform.position, (col.gameObject.transform.position - transform.position), Color.red);
       if(nearTarget != null)
        {
            Vector3 targetDir = col.gameObject.transform.position - transform.position;
            float angle = Vector3.Angle(targetDir, transform.forward);
           // print(angle);
            if (angle < getFOV() || (angle > getFOV() && distanceToMe(col.gameObject) < backDetectRange) )
            {
                nearTarget = null;
                makeAIChaseTarget(col.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(nearTarget != null)
        {
            nearTarget = null;
        }
    }

    private void runTowardsTarget(GameObject chasingTarget)
    {
        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.SetDestination(chasingTarget.transform.position);  
    }

    private void walkTowardsTarget(Vector3 target)
    {
        navMeshAgent.speed = moveSpeed;
        navMeshAgent.SetDestination(target);
    }

    private void walkTowardsTarget(GameObject chasingTarget)
    {
        walkTowardsTarget(chasingTarget.transform.position);  
    }

    private void setAlertModifier(AIState state)
    {
        switch (currentState)
        {
            case AIState.Idle:
                idleNotifier.SetActive(true);
                attackNotifier.SetActive(false);
                alertNotifier.SetActive(false);
                break;
            case AIState.Alert:
                idleNotifier.SetActive(false);
                attackNotifier.SetActive(false);
                alertNotifier.SetActive(true);
                break;
            case AIState.Attacking:
                idleNotifier.SetActive(false);
                attackNotifier.SetActive(true);
                alertNotifier.SetActive(false);
                break;
        }
    }

    private void handlePatrolling()
    {
        if (!navMeshAgent.destination.Equals(patrolWaypoints[currentWaypoint].transform.position)) {
            navMeshAgent.SetDestination(patrolWaypoints[currentWaypoint].transform.position);
        }
        if(navMeshAgent.remainingDistance < 0.1f)
        {
            if(waitTime > 0 && waitTimer == 0)
            {
                waitTimer = waitTime;
            }
            if(waitTimer > 0)
            {
                waitTimer -= Time.deltaTime;
                if (canHangAround)
                {
                    handleHangAround(patrolWaypoints[currentWaypoint]);
                }
            }
            else
            {
                waitTimer = 0;
                walkTowardsTarget(getNextWaypoint());
            }
           
        }
    }

    private void handleHangAround(GameObject hangOrigin)
    {
        if(hangOrigin == null)
        {
            hangOrigin = startArea;
        }
        if(hangTime > 0 && hangTimer == 0)
        {
            hangTimer = hangTime;
        }
        if(hangTimer > 0)
        {
            //TODO play idle animation
            hangTimer -= Time.deltaTime;
        }
        else
        {
            hangTimer = hangTime;
            walkTowardsTarget(getNextHangPoint(hangOrigin));
        }
    }

    private Vector3 getNextHangPoint(GameObject origin)
    {
        Vector3 nextPoint = origin.transform.position;
        nextPoint += UnityEngine.Random.insideUnitSphere * hangAroundRadius;
        nextPoint.y = gameObject.transform.position.y;
        return nextPoint;
    }

    private Vector3 getNextWaypoint()
    {
        currentWaypoint = (currentWaypoint + 1) % patrolWaypoints.Count;
        return patrolWaypoints[currentWaypoint].transform.position;
    }

    private float getFOV()
    {
        return fieldOfView / 2f;
    }

    private void makeAIChaseTarget(GameObject target)
    {
        chasingTarget = target;
        currentState = AIState.Alert;
        runTowardsTarget(chasingTarget);
    }

    private float distanceToMe(GameObject target)
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }

    private bool checkTargetStealth(GameObject target)
    {
        if(target.GetComponent<PlayerController>() != null)
        {
           //Check for stealth on
        }
        return false;
    }

    private bool isPlayerStealth(GameObject target)
    {
        if (target.GetComponent<StatManager>() != null)
        {
            return target.GetComponent<StatManager>().getCurrentState().Equals(PlayerStates.STATE.SNEAKING);
        }
        else return false;
    }
}
