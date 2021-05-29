using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance;
    public static PlayerController Instance { get { return _instance; } }
    
    public CharacterController characterController;
    public float speed = 3;
    
    [SerializeField]
    private LayerMask groundMask ;
    [SerializeField]
    private LayerMask wallMask ;
    Vector3 wallPoint;
    Vector3 wallNormal;
    private BoxCollider collider;

    private float gravity = 9.87f;
    private float verticalSpeed = 0;
    public float ClimbSpeed = 0.1f;
    private GroundDetector[] feet;
    private Vector3 _moveDirection;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        collider = GetComponent<BoxCollider>();
        feet = GetComponentsInChildren<GroundDetector>();
    }
    void FixedUpdate()
    {
        Rotate();
        if (NearWall() && FacingWall())
        {
            ClimbWall();
        }
        else
        {
            Move();
        }
        if(Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }

    private void Rotate()
    {
        Vector3 v = Camera.main.transform.forward;
        v.y = 0f;
        v.Normalize();
        Quaternion rotation = Quaternion.LookRotation(v);
        float smoothing = 5f;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * smoothing);
    }


    public void Move()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");
        if (characterController.isGrounded) verticalSpeed = 0;
        else verticalSpeed -= gravity * Time.deltaTime;
        Vector3 gravityMove = new Vector3(0, verticalSpeed, 0);
        Vector3 move = transform.forward * verticalMove + transform.right * horizontalMove;
        characterController.Move(speed * Time.deltaTime * move + gravityMove * Time.deltaTime);
    }
    bool NearWall()
    {
        return Physics.CheckSphere(transform.position, 1f, wallMask);
    }
    bool FacingWall()
    {
        RaycastHit hit;
        var facingWall = Physics.Raycast(transform.position, transform.forward, out hit, collider.size.x , wallMask);
        wallPoint = hit.point;
        wallNormal = hit.normal;
        return facingWall;
    }
    void ClimbWall()
    {
        GrabWall();
        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");
        var move = transform.up * v + transform.right * h;
        ApplyMove(move, ClimbSpeed);
    }
    
    void GrabWall()
    {
        var newPosition = wallPoint + wallNormal * (collider.size.x - 0.1f);
        transform.position = Vector3.Lerp(transform.position, newPosition, ClimbSpeed * Time.deltaTime);
    }
    void ApplyMove(Vector3 move, float speed)
    {
        characterController.Move(speed * Time.deltaTime * move);
    }
}