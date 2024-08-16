using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class EnemyController : MonoBehaviour
{

    private bool randAction;
    [SerializeField] float range = 8f;
    [SerializeField] TextMeshProUGUI nameText;
    private Transform centrePoint;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private bool isMoving = false;
    private bool isCoroutineReady = true;
    private StateController stateController;
    private Attack attack;
    private Color baseColor;

    public IObjectPool<EnemyController> enemyPool;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        attack = GetComponent<Attack>();
        stateController = GetComponent<StateController>();
        centrePoint = transform;
        nameText.text = gameObject.name;
        if (transform.Find("Skins").GetComponent<SkinnedMeshRenderer>() != null)
        {
            baseColor = transform.Find("Skins").GetComponent<SkinnedMeshRenderer>().material.color;
        }
        NonDisplayAim();
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

    public void SetPool(IObjectPool<EnemyController> pool)
    {
        enemyPool = pool;
    }

    public void ResetOnRelease()
    {
        if (gameObject.GetComponent<Attack>() != null)
        {
            gameObject.GetComponent<Attack>().ResetKill();
        }
    }

    public void WhenOnGet(Vector3 randomPoint, string enemyName, Material skin, Material pant)
    {
        if (gameObject.GetComponent<Attack>() != null)
        {
            gameObject.GetComponent<Attack>().canAttack = true;
            gameObject.GetComponent<Attack>().ChangeNameAndMaterial(enemyName,skin,pant);
            baseColor = skin.color;
        }

        if(rb != null)
        {
            rb.useGravity = true;
        }
        if(gameObject.GetComponent<Collider>() != null)
        {
            gameObject.GetComponent<Collider>().enabled = true;
        }
        transform.position = randomPoint;
    }

    public Color GetColor()
    {
        return baseColor;
    }
    
    public void DisplayAim()
    {
        if(transform.Find("Aim").gameObject != null)
        {
            transform.Find("Aim").gameObject.SetActive(true);
        }
    }

    public void NonDisplayAim()
    {
        if (transform.Find("Aim").gameObject != null)
        {
            transform.Find("Aim").gameObject.SetActive(false);
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
    public void CancelPatrol()
    {
        agent.SetDestination(transform.position);
    }
    IEnumerator RandomAction()
    {
        isCoroutineReady = false;
        randAction = Random.value > 0.5f;
        if (randAction)
        {
            Patrol();
            if (gameObject.GetComponent<Attack>() != null)
            {
                gameObject.GetComponent<Attack>().canAttack = true;
            }
        }
        else
        {
            if(gameObject.GetComponent<Attack>() != null && !isMoving)
            {
                gameObject.GetComponent<Attack>().EnemyCheckAttackable();
            }
        }
        yield return new WaitForSeconds(1f);
        isCoroutineReady = true;
    }
}
