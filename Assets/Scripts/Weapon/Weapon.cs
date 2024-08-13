using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed = 720f;
    [SerializeField] Transform weaponParent;
    [SerializeField] Attack attack;
    private Rigidbody rb;
    private Vector3 oldPos;
    private Quaternion oldRot;
    private bool isReturning;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        oldPos = transform.localPosition;
        oldRot = transform.localRotation;
        weaponParent = transform.parent;
        isReturning = false;
        attack = gameObject.GetComponentInParent<Attack>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WeaponAttack(Vector3 shootDir)
    {
        rb.transform.parent = null;
        StartCoroutine(MoveWeapon(shootDir,oldPos));
    }

    private IEnumerator MoveWeapon(Vector3 targetPosition, Vector3 oldPos)
    {
        Vector3 tempPos = transform.position;
        Vector3 direction = (targetPosition - tempPos).normalized;
        
        while (Vector3.Distance(transform.position, tempPos) < 4.25f && !isReturning)
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
            isReturning = true;
            
            
        }
    }

    public void ResetWeapon()
    {
        isReturning = false;
        rb.transform.parent = weaponParent;
        transform.localPosition = oldPos;
        transform.localRotation = oldRot;
    }
}
