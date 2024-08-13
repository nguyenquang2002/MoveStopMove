using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    private SphereCollider attackRange;
    private Animator anim;

    [SerializeField] float growPercent = 10f;
    [SerializeField] GameObject weapon;
    public bool canAttack;
    private Quaternion oldRos;
    public int numberOfRays = 36;
    public int killCount = 0;

    private void Awake()
    {
        attackRange = gameObject.GetComponentInChildren<SphereCollider>();
        anim = gameObject.GetComponent<Animator>();
        canAttack = true;
        oldRos = transform.localRotation;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemy();
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Attackable"))
    //    {
    //        if (gameObject.GetComponent<PlayerController>() != null)
    //        {
    //            if (!gameObject.GetComponent<PlayerController>().CheckMovement())
    //            {
    //                Vector3 tempPosition = other.transform.position + new Vector3(0, 1f, 0);
    //                attackRange.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    //                transform.LookAt(tempPosition);

    //                if (canAttack && weapon.GetComponentInChildren<Weapon>() != null)
    //                {

    //                    StartCoroutine(AttackAction(tempPosition));
    //                }
    //            }
    //            else
    //            {
    //                attackRange.gameObject.transform.localRotation = oldRos;
    //                canAttack = true;
    //            }

    //        }


    //    }
    //    else if (other.CompareTag("Player"))
    //    {

    //    }
    //}

    public float Range()
    {
        return attackRange.radius * attackRange.transform.localScale.x;
    }

    public void Kill()
    {
        killCount++;
        gameObject.transform.localScale += gameObject.transform.localScale * growPercent / 100;
    }

    private void CheckEnemy()
    {
        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = i * 360f / numberOfRays;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            //Debug.DrawRay(transform.position, direction * angle, Color.red);

            RaycastHit hit;
            float range = Range() - 1f;

            if (Physics.Raycast(transform.position, direction, out hit, range))
            {
                if (gameObject.GetComponent<PlayerController>() != null)
                {
                    if (!gameObject.GetComponent<PlayerController>().CheckMovement())
                    {
                        Vector3 tempPosition = hit.collider.transform.position + new Vector3(0, 1f, 0);
                        attackRange.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                        transform.LookAt(tempPosition);

                        if (canAttack && weapon.GetComponentInChildren<Weapon>() != null)
                        {
                            StartCoroutine(AttackAction(tempPosition));
                        }
                    }
                    else
                    {
                        attackRange.gameObject.transform.localRotation = oldRos;
                        canAttack = true;
                    }
                }
            }

        }
    }

    IEnumerator AttackAction(Vector3 tempPosition)
    {
        anim.SetBool("IsAttack", true);
        anim.SetBool("IsIdle", false);
        yield return new WaitForSeconds(0.25f);
        if (gameObject.GetComponent<PlayerController>().CheckMovement())
        {
            anim.SetBool("IsAttack", false);
        }
        if(weapon.GetComponentInChildren<Weapon>() != null)
        {
            weapon.GetComponentInChildren<Weapon>().WeaponAttack(tempPosition);
        }
        canAttack = false;
        yield return new WaitForSeconds(0.75f);
        anim.SetBool("IsAttack", false);
    }
}
