using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

public class FPS : MonoBehaviour
{
    public float speed = 1.0f;
    public float runspeed = 10.0f;
    private float movespeed = 0f;
    public float mousesense = 100.0f;
    public float jumpspeed = 0.04f;

    public Transform camera;
    private Rigidbody rb;
    private Transform player;
    private CapsuleCollider playerColider;
    public Transform playerModel;
    private Animator animator;

    private float mouseX;
    private float mouseY;

    float xrotation = 0f;
    float yrotation = 0f;

    private bool isCrouch;
    private bool isGrounded;

    RaycastHit hit;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
        playerColider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        
    }

    private void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * mousesense * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mousesense * Time.deltaTime;
        xrotation -= mouseY;
        xrotation = Mathf.Clamp(xrotation, -65f, 75f);
        yrotation += mouseX;
        camera.localRotation = Quaternion.Euler(xrotation, 0f, 0f);
        player.localRotation = Quaternion.Euler(0f, yrotation, 0f);
        Cursor.lockState = CursorLockMode.None;

        if (isGrounded && Input.GetKeyDown(KeyCode.LeftControl) && playerColider.height == 1.75f)        
            playerColider.height = 1f;         
        
        else if (playerColider.height == 1f && Input.GetKeyDown(KeyCode.LeftControl))       
            playerColider.height = 1.75f;                 
    }

    private void FixedUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        

        if (isGrounded && Input.GetKey(KeyCode.LeftShift))
            movespeed = runspeed;
        else if (isGrounded)
            movespeed = speed;
        else
            movespeed = jumpspeed;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 0.9f)) 
            isGrounded = true;        
        else       
            isGrounded= false;    
        
        if (isGrounded && Input.GetKey(KeyCode.Space)) {
            rb.AddForce(new Vector3(0f, 5f, 0f), ForceMode.Impulse);          
        }

        Vector3 direction = v * transform.forward + h * transform.right;
        animator.SetFloat("Speed", Vector3.ClampMagnitude(direction, 1).magnitude);
        rb.MovePosition(transform.position + Vector3.ClampMagnitude(direction, 1) * movespeed);
    }
}