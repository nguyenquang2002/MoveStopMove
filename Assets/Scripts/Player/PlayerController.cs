using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 3.0f;
    [SerializeField] Camera cam;
    [SerializeField] float rotationSpeed = 30.0f;

    private JoystickManager joystickManager;
    private Rigidbody rb;
    private SphereCollider attackRange;
    private Animator animator;
    private float inputX, inputZ;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        joystickManager = GameObject.Find("Joystick").GetComponent<JoystickManager>();
        attackRange = gameObject.GetComponentInChildren<SphereCollider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Movement();
    }

    void GetInput()
    {
        inputX = joystickManager.InputHorizontal();
        inputZ = joystickManager.InputVertical();
        
    }

    void Movement()
    {
        Vector3 targetVector = new Vector3 (inputX, 0 ,inputZ);
        Vector3 movementVector = MoveTowardTarget(targetVector);
        RotateTowardMovementVector(movementVector);
        animator.SetBool("IsIdle", inputX == 0 && inputZ == 0);
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
