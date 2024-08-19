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
    private UIController uiCanvas;
    private Spawner spawner;
    private bool isDefeat = false, isVictory = false;
    [SerializeField] Material deathMaterial;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        attack = GetComponent<Attack>();
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
        enemyController = GetComponent<EnemyController>();
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        uiCanvas = GameObject.Find("Canvas").GetComponent<UIController>();
    }

    private void LateUpdate()
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
            if (playerController != null && !playerController.IsDeath() && !isDefeat)
            {
                if (attack != null)
                {
                    uiCanvas.SetKillCount(attack.killCount);
                }
                isVictory = true;
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
        if (playerController != null && !isVictory)
        {
            isDefeat = true;
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
