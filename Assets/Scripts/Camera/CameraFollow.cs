using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    [SerializeField] Transform player;
    private Vector3 originOffset;
    // Start is called before the first frame update
    void Start()
    {
        originOffset = offset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x, 0, player.position.z) + offset;
        transform.rotation = Quaternion.Euler(45,0,0);
        //transform.LookAt(player.position);
    }
    public void IncreaseOffset(float playerScale)
    {
        offset.y = originOffset.y * playerScale;
        offset.z = originOffset.z * playerScale;
    }
}
