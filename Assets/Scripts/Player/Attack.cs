using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    private SphereCollider attackRange;

    private void Awake()
    {
        attackRange = gameObject.GetComponentInChildren<SphereCollider>();
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
            Debug.Log("Enemy!");
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("Player!");
        }
    }
}
