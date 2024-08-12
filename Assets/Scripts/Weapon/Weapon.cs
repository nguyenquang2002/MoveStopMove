using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField] float force;
    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        Vector3.MoveTowards(transform.position, shootDir, 2);

    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
