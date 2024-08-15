using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{

    private SphereCollider attackRange;
    private Animator anim;
    private SkinnedMeshRenderer rendSkin, rendPant;

    public float growPercent = 10f;
    [SerializeField] GameObject weapon;
    [SerializeField] TextMeshProUGUI killCountText, nameText;
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
        rendSkin = transform.Find("Skins").GetComponent<SkinnedMeshRenderer>();
        rendPant = transform.Find("Pants").GetComponent<SkinnedMeshRenderer>();
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

        //if (attackRange != null)
        //{
        //    attackRange.enabled = false;
        //    attackRange.enabled = true;
        //}
        Physics.SyncTransforms();

        CheckEnemy();
    }

    public void ResetKill()
    {
        gameObject.transform.localScale = oldScale;
        killCount = 0;
        DisplayKillCount();
    }
    public void ChangeNameAndMaterial(string name, Material skin, Material pant)
    {
        gameObject.name = name;
        if(rendSkin != null)
        {
            rendSkin.material = skin;
        }
        if(rendPant != null)
        {
            rendPant.material = pant;
        }
        if (killCountText.transform.parent.GetComponent<Image>() != null)
        {
            killCountText.transform.parent.GetComponent<Image>().color = skin.color;
        }
        nameText.color = skin.color;
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

        Collider nearestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Attackable") && hit.gameObject != gameObject)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    nearestEnemy = hit;
                }
            }
        }

        if (gameObject.GetComponent<PlayerController>() != null && nearestEnemy != null)
        {
            if (!gameObject.GetComponent<PlayerController>().CheckMovement())
            {
                Vector3 tempPosition = new Vector3(nearestEnemy.transform.position.x, transform.position.y, nearestEnemy.transform.position.z);
                transform.LookAt(tempPosition);
                attackRange.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

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

                    bool enemyAttack = Random.value > 0.2f;
                    if (canAttack && enemyAttack)
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
        yield return new WaitForSeconds(0.35f);
        if(weapon.GetComponentInChildren<Weapon>() != null)
        {
            weapon.GetComponentInChildren<Weapon>().WeaponAttack(tempPosition);
        }
        canAttack = false;
        yield return new WaitForSeconds(0.15f);
        anim.SetBool("IsAttack", false);
    }
}
