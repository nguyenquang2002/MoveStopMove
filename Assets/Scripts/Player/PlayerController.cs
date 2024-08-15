using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 3.0f;
    [SerializeField] Camera cam;
    [SerializeField] float rotationSpeed = 30.0f;

    private JoystickManager joystickManager;
    private StateController stateController;
    private Rigidbody rb;
    private SphereCollider attackRange;
    private float inputX, inputZ;
    private bool isDeath = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        joystickManager = GameObject.Find("Joystick").GetComponent<JoystickManager>();
        attackRange = gameObject.GetComponentInChildren<SphereCollider>();
        stateController = GetComponent<StateController>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isDeath) return;
        GetInput();
        Movement();
    }

    void GetInput()
    {
        inputX = joystickManager.InputHorizontal();
        inputZ = joystickManager.InputVertical();
        
    }
    public bool CheckMovement()
    {
        return inputX != 0 || inputZ != 0;
    }
    void Movement()
    {
        Vector3 targetVector = new Vector3 (inputX, 0 ,inputZ);
        Vector3 movementVector = MoveTowardTarget(targetVector);
        RotateTowardMovementVector(movementVector);
        stateController.Idle(!CheckMovement());
    }

    public void SetDeath(bool death)
    {
        isDeath = death;
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        float speedMove = speed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, cam.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;
        Vector3 targetPosition = transform.position + targetVector * speedMove;
        transform.position = targetPosition;
        return targetVector;
    }
    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if (movementDirection.magnitude == 0)
        {
            return;
        }
        var rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);
    }
}
