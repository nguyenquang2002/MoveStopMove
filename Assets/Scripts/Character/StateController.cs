using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{

    public enum State
    {
        Idle,
        Run,
        Attack,
        Ulti,
        Dance,
        Dead
    }
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    public void Death()
    {
        StartCoroutine(DeathAnimate());
    }

    public void Idle(bool isIdle)
    {
        animator.SetBool("IsIdle", isIdle);
    }

    IEnumerator DeathAnimate()
    {
        animator.SetBool("IsDead", true);
        animator.SetBool("IsIdle", false);
        if(gameObject.GetComponent<Rigidbody>() != null )
        {
            gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
        if (gameObject.GetComponent<EnemyController>() != null)
        {
            gameObject.GetComponent<EnemyController>().CancelPatrol();
        }
        yield return new WaitForSeconds(1.0f);
        if(gameObject.GetComponent<EnemyController>() != null )
        {
            gameObject.GetComponent<EnemyController>().enemyPool.Release(gameObject.GetComponent<EnemyController>());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

}
