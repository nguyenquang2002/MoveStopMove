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
    private Attack attack;
    private Rigidbody rb;
    private PlayerController playerController;
    private EnemyController enemyController;
    private GameManager uiCanvas;
    private Spawner spawner;
    [SerializeField] Material deathMaterial;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        attack = GetComponent<Attack>();
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
        enemyController = GetComponent<EnemyController>();
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        uiCanvas = GameObject.Find("Canvas").GetComponent<GameManager>();
    }

    private void Update()
    {
        CheckWinCondition();
    }

    public void Death()
    {
        StartCoroutine(DeathAnimate());
    }

    public void CheckWinCondition()
    {
        if(spawner.GetAlive() == 1)
        {
            if (playerController != null && !playerController.IsDeath())
            {
                uiCanvas.SetVictory();
            }
        }
    }

    public void Idle(bool isIdle)
    {
        animator.SetBool("IsIdle", isIdle);
    }

    IEnumerator DeathAnimate()
    {
        animator.SetBool("IsDead", true);
        animator.SetBool("IsIdle", false);
        if(rb != null )
        {
            rb.useGravity = false;
        }
        if (enemyController != null)
        {
            enemyController.CancelPatrol();
        }
        if (playerController != null && playerController.GetKillBy() != null)
        {
            uiCanvas.SetKillerText(playerController.GetKillBy());
        }
        if (spawner != null)
        {
            if (playerController != null)
            {
                uiCanvas.SetRanking(spawner.GetAlive());
            }
            spawner.ReduceAlive();
        }
        if(attack != null)
        {
            attack.ChangeMaterial(deathMaterial, deathMaterial);
        }
        yield return new WaitForSeconds(1.0f);
        if (playerController != null)
        {
            uiCanvas.SetGameOver();
        }
        if(enemyController != null )
        {
            enemyController.enemyPool.Release(gameObject.GetComponent<EnemyController>());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

}
