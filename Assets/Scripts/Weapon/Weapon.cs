using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    
    [SerializeField] float speed = 3f;
    [SerializeField] float rotationSpeed = 540f;
    [SerializeField] Transform weaponParent;
    [SerializeField] Attack attack;
    [SerializeField] GameObject character;
    private Rigidbody rb;
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
        attack = gameObject.GetComponentInParent<Attack>();
        character = attack.gameObject;
        rb.isKinematic = true;
        rb.useGravity = false;
        attackRange = attack.Range();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WeaponAttack(Vector3 shootDir)
    {
        rb.transform.parent = null;
        StartCoroutine(MoveWeapon(shootDir));
    }

    private IEnumerator MoveWeapon(Vector3 targetPosition)
    {
        Vector3 tempPos = transform.position;
        Vector3 direction = (targetPosition - tempPos).normalized;

        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        //rb.AddForce(direction * speed, ForceMode.Impulse);
        while (Vector3.Distance(transform.position, tempPos) < attackRange && !isReturning)
        {

            transform.position += direction * speed * Time.deltaTime;
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
            yield return null;
        }
        
        ResetWeapon();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attackable"))
        {
            if(other.GetComponent<StateController>() != null && other.gameObject != character)
            {
                other.GetComponent<StateController>().Death();
                attack.Kill();
                attackRange = attack.Range();
                gameObject.transform.localScale += oldScale * attack.growPercent / 100;
            }

            isReturning = true;
            
        }
    }

    public void ResetWeapon()
    {
        isReturning = false;
        rb.isKinematic = true;
        rb.transform.parent = weaponParent;
        transform.localPosition = oldPos;
        transform.localRotation = oldRot;
    }
}
