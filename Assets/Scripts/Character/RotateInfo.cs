using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LateUpdate()
    {
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
