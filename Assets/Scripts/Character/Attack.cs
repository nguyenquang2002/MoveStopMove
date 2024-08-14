using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    private SphereCollider attackRange;
    private Animator anim;

    public float growPercent = 10f;
    [SerializeField] GameObject weapon;
    public bool canAttack;
    private Quaternion oldRos;
    private Vector3 oldScale;
    public int numberOfRays = 36;
    public int killCount = 0;

    private void Awake()
    {
        attackRange = gameObject.GetComponentInChildren<SphereCollider>();
        anim = gameObject.GetComponent<Animator>();
        canAttack = true;
        oldRos = transform.localRotation;
        oldScale = transform.localScale;
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Range());
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemy();
    }

    

    
    public void Kill()
    {
        killCount++;
        gameObject.transform.localScale += oldScale * growPercent / 100;

        if (attackRange != null)
        {
            attackRange.enabled = false;
            attackRange.enabled = true;
        }
        Physics.SyncTransforms();

        CheckEnemy();
    }

    public float Range()
    {
        return attackRange.radius * attackRange.transform.localScale.x * gameObject.transform.localScale.x;
    }

    private void CheckEnemy()
    {
        //Raycast error real hit distance not scale like range
        //for (int i = 0; i < numberOfRays; i++)
        //{
        //    float angle = i * 360f / numberOfRays;
        //    Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            
        //    RaycastHit hit;
        //    float range = Range();
        //    Debug.DrawRay(transform.position, direction * range, Color.red);
            
        //    if (Physics.Raycast(transform.position, direction, out hit, range))
        //    {
                
        //        if (gameObject.GetComponent<PlayerController>() != null)
        //        {
        //            if (!gameObject.GetComponent<PlayerController>().CheckMovement())
        //            {
        //                if (hit.collider.CompareTag("Attackable"))
        //                {
        //                    Debug.Log(hit.distance);
        //                    Vector3 tempPosition = hit.collider.transform.position + new Vector3(0, 1f, 0);
        //                    transform.LookAt(tempPosition);
        //                    attackRange.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        //                    if (canAttack && weapon.GetComponentInChildren<Weapon>() != null)
        //                    {
        //                        StartCoroutine(AttackAction(tempPosition));
        //                        Debug.Log(range);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                attackRange.gameObject.transform.localRotation = oldRos;
        //                canAttack = true;
        //            }
        //        }
        //    }
        //}

        float range = Range();
        Collider[] hits = Physics.OverlapSphere(transform.position, range);

        foreach (var hit in hits)
        {
            if (gameObject.GetComponent<PlayerController>() != null)
            {
                if (!gameObject.GetComponent<PlayerController>().CheckMovement())
                {
                    if (hit.CompareTag("Attackable"))
                    {
                        Vector3 tempPosition = hit.transform.position + new Vector3(0, 1f, 0);
                        transform.LookAt(tempPosition);
                        attackRange.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

                        if (canAttack && weapon.GetComponentInChildren<Weapon>() != null)
                        {
                            StartCoroutine(AttackAction(tempPosition));
                            Debug.Log(range);
                        }
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
