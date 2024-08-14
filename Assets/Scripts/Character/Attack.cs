using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Attack : MonoBehaviour
{

    private SphereCollider attackRange;
    private Animator anim;

    public float growPercent = 10f;
    [SerializeField] GameObject weapon;
    [SerializeField] TextMeshProUGUI killCountText;
    public bool canAttack;
    private Quaternion oldRos;
    private Vector3 oldScale;
    public int numberOfRays = 36;
    public int killCount = 0;
    private CameraFollow cam;

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
        cam = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemy();
    }

    private void DisplayKillCount()
    {
        killCountText.text = killCount.ToString();
    }

    
    public void Kill()
    {
        killCount++;
        DisplayKillCount();
        gameObject.transform.localScale += oldScale * growPercent / 100;
        if (gameObject.GetComponent<PlayerController>() != null )
        {
            cam.IncreaseOffset(gameObject.transform.localScale.y);
        }

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
                    if (hit.CompareTag("Attackable") && hit.gameObject != gameObject)
                    {
                        Vector3 tempPosition = new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z);
                        transform.LookAt(tempPosition);
                        attackRange.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

                        if (canAttack && weapon.GetComponentInChildren<Weapon>() != null)
                        {
                            StartCoroutine(AttackAction(tempPosition));
                            
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

    public void EnemyCheckAttackable()
    {
        float range = Range();
        Collider[] hits = Physics.OverlapSphere(transform.position, range);
        foreach (var hit in hits)
        {
            if(gameObject.GetComponent<EnemyController>() != null)
            {
                if (hit.CompareTag("Attackable") && hit.gameObject != gameObject)
                {
                    Vector3 tempPosition = new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z);
                    transform.LookAt(tempPosition);
                    attackRange.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

                    bool enemyAttack = Random.value > 0.25f;
                    if (canAttack)
                    {
                        StartCoroutine(AttackAction(tempPosition));
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
        if(weapon.GetComponentInChildren<Weapon>() != null)
        {
            Debug.Log(weapon.GetComponentInChildren<Weapon>());
            weapon.GetComponentInChildren<Weapon>().WeaponAttack(tempPosition);
        }
        canAttack = false;
        yield return new WaitForSeconds(0.75f);
        anim.SetBool("IsAttack", false);
    }
}
