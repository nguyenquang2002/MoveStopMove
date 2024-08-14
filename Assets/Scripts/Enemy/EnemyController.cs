using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    private float randAction;
    [SerializeField] float range = 8f;
    [SerializeField] TextMeshProUGUI nameText;
    private Transform centrePoint;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private bool isMoving = false;
    private bool isCoroutineReady = true;
    private StateController stateController;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        stateController = GetComponent<StateController>();
        centrePoint = transform;
        nameText.text = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = agent.velocity.magnitude > 0.5f;
        stateController.Idle(!isMoving);
        if(isCoroutineReady)
        {
            StartCoroutine(RandomAction());
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) 
        { 
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void Patrol()
    {
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point))
            {
                agent.SetDestination(point);
                //Debug.Log(gameObject.name + " Patrol!");
            }
        }
    }
    IEnumerator RandomAction()
    {
        isCoroutineReady = false;
        randAction = Random.Range(0f, 5f);
        if (randAction > 2f)
        {
            Patrol();
            if (gameObject.GetComponent<Attack>() != null)
            {
                gameObject.GetComponent<Attack>().canAttack = true;
            }
        }
        else
        {
            if(gameObject.GetComponent<Attack>() != null)
            {
                gameObject.GetComponent<Attack>().EnemyCheckAttackable();
            }
            

        }
        yield return new WaitForSeconds(2f);
        isCoroutineReady = true;
    }
}
