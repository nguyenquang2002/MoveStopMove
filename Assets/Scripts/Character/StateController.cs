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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

}
