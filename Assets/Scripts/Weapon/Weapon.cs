using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    
    [SerializeField] float speed = 3f;
    [SerializeField] bool isRotate, isMultiple;
    [SerializeField] float rotationSpeed = 540f;
    [SerializeField] Transform weaponParent;
    [SerializeField] Attack attack;
    [SerializeField] GameObject character;
    private Rigidbody rb;
    public bool isClone = false;
    private Vector3 oldPos,oldScale;
    private Quaternion oldRot;
    private bool isReturning;
    public float attackRange;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        oldPos = transform.localPosition;
        oldRot = transform.localRotation;
        oldScale = transform.localScale;


        isReturning = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        weaponParent = transform.parent;
        if (gameObject.GetComponentInParent<Attack>() != null)
        {
            attack = gameObject.GetComponentInParent<Attack>();
            character = attack.gameObject;
        }
        rb.isKinematic = true;
        rb.useGravity = false;
        attackRange = attack.Range();
    }

    public void WeaponAttack(Vector3 shootDir)
    {
        rb.transform.parent = null;
        StartCoroutine(MoveWeapon(shootDir));
    }

    private IEnumerator MoveWeapon(Vector3 targetPosition)
    {
        Vector3 tempPos = transform.position;
        Vector3 target = new Vector3(targetPosition.x, targetPosition.y + 0.4f, targetPosition.z);
        Vector3 direction = (target - tempPos).normalized;

        rb.isKinematic = false;
        //rb.velocity = Vector3.zero;
        //rb.AddForce(direction * speed, ForceMode.Impulse);
        bool hasSpwanClone = false;
        while (Vector3.Distance(transform.position, tempPos) < attackRange && !isReturning)
        {
            transform.position += direction * speed * Time.deltaTime;
            if (isRotate)
            {
                transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
            }
            if(isMultiple && !hasSpwanClone)
            {
                Vector3 leftDirection = Quaternion.Euler(0, -35, 0) * direction;
                Vector3 rightDirection = Quaternion.Euler(0, 35, 0) * direction;

                MultipleWeapon(leftDirection);
                MultipleWeapon(rightDirection);

                hasSpwanClone = true;
            }
            yield return null;
        }
        isReturning = true;
        StartCoroutine(ResetWeapon());
    }

    

    private void MultipleWeapon(Vector3 direction)
    {
        GameObject clone = Instantiate(gameObject, transform.position, transform.rotation);
        Weapon cloneWeapon = clone.GetComponent<Weapon>();
        cloneWeapon.isMultiple = false;
        cloneWeapon.isClone = false;
        cloneWeapon.WeaponAttack(transform.position + direction * attackRange);
        Destroy(clone, attackRange / speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attackable") && other.gameObject != character)
        {
            if(other.GetComponent<StateController>() != null)
            {
                other.enabled = false;
                other.GetComponent<StateController>().Death();
                if (other.GetComponent<PlayerController>() != null)
                {
                    other.GetComponent<PlayerController>().SetDeath(true);
                }
                attack.Kill();
                attackRange = attack.Range();
                gameObject.transform.localScale += oldScale * attack.growPercent / 100;
            }
            if (!isClone)
            {
                if (!isReturning)
                {
                    isReturning = true;
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator ResetWeapon()
    {
        rb.isKinematic = true;
        rb.transform.parent = weaponParent;
        transform.localPosition = oldPos;
        transform.localRotation = oldRot;
        yield return new WaitForSeconds(0.45f);
        isReturning = false;
    }
}
