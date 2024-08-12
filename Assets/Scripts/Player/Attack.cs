using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    private SphereCollider attackRange;
    private GameObject weapon;
    public bool canAttack;

    private void Awake()
    {
        attackRange = gameObject.GetComponentInChildren<SphereCollider>();
        Weapon weaponScript = gameObject.GetComponentInChildren<Weapon>();
        weapon = weaponScript.gameObject;
        canAttack = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Attackable"))
        {
            if(gameObject.GetComponent<PlayerController>() != null && !gameObject.GetComponent<PlayerController>().CheckMovement())
            {
                
                Vector3 tempPosition = other.transform.position;
                transform.LookAt(tempPosition);
                if (canAttack)
                {
                    GameObject weaponObj = Instantiate(weapon, transform.position, Quaternion.Euler(0, 0, 0));
                    weapon.GetComponent<Weapon>().WeaponAttack(tempPosition);
                    canAttack = false;
                }

            }
            Debug.Log("Enemy!");
            
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("Player!");
        }
    }

    IEnumerator AttackAction()
    {
        yield return new WaitForSeconds(0.2f);
    }
}
