using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

public class FPS : MonoBehaviour
{
    public float speed = 1.0f;
    public float runSpeed = 10.0f;
    private float moveSpeed = 0f;
    public float mouseSense = 100.0f;
    public float jumpSpeed = 0.04f;
    public float jumpForce = 3f;
    public float jumpCoolDown = 2f;
    public float crouchCoolDown = 1f;
    private float lastJumpTime = 0f;
    private float lastCrouchTime = 0f;
    private Vector3 camPos1 = new(0f, 2f, 0f);
    private Vector3 camPos2 = new(0f, 1.35f, 0f);
    private Vector3 camTarget;

    public Transform playerCamera;
    private Rigidbody rb;
    private Transform player;
    public CapsuleCollider playerColider;
    public Transform playerModel;
    private Animator animator;
    public Transform groundCheck;

    private float mouseX;
    private float mouseY;

    float xrotation = 0f;
    float yrotation = 0f;

    private bool isCrouching = false;
    private bool isGrounded = false;
    private bool isRunning = false;

    public LayerMask notPlayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Transform>();
        animator = GetComponent<Animator>();


        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CameraRotation();

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKeyDown(KeyCode.LeftControl))
            Crouch();       
    }

    private void FixedUpdate()
    {
        Moving();

        if (Physics.CheckSphere(groundCheck.position, 0.1f, notPlayer))
        {
            isGrounded = true;
            animator.SetBool("IsInAir", false);
        } 
        else 
        {
            isGrounded = false;
            animator.SetBool("IsInAir", true);
        }
    }

    void Moving()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");


        if (Input.GetKey(KeyCode.LeftShift) && isGrounded && !isCrouching )
        {
            moveSpeed = runSpeed;
            isRunning = true;
        }
        else if (isGrounded)
        {
            moveSpeed = speed;
            isRunning = false;
        }
        else
        {
            moveSpeed = jumpSpeed;
            isRunning = false;
        }

        Vector3 direction = v * transform.forward + h * transform.right;
        animator.SetFloat("Speed", Vector3.ClampMagnitude(direction, 1).magnitude);
        rb.MovePosition(transform.position + Vector3.ClampMagnitude(direction, 1) * moveSpeed);

        
    }

    void CameraRotation()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSense * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSense * Time.deltaTime;
        xrotation -= mouseY;
        xrotation = Mathf.Clamp(xrotation, -65f, 75f);
        yrotation += mouseX;
        playerCamera.localRotation = Quaternion.Euler(xrotation, 0f, 0f);
        player.localRotation = Quaternion.Euler(0f, yrotation, 0f);
    }

    void Jump()
    {
        if (isGrounded && lastJumpTime <= Time.time - jumpCoolDown && !isCrouching)
        {
            animator.SetTrigger("Jumping");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            lastJumpTime = Time.time;
        } 
        
    }

    void Crouch()
    {
        if (isGrounded && !isRunning && playerColider.height == 1.6f && lastCrouchTime <= Time.time - jumpCoolDown)
        {
            playerColider.height = 1f;
            playerColider.center = new Vector3(0f, -0.3f, 0f);
            camTarget = camPos2;
            isCrouching = true;
            animator.SetBool("Crouching", true);
            lastCrouchTime = Time.time;
            StartCoroutine(MoveCamera(camTarget));
        }
        else if ((playerColider.height == 1f) && (lastCrouchTime <= Time.time - jumpCoolDown) && (!Physics.Raycast(transform.position, Vector3.up , 1.6f, notPlayer)))
        {
            playerColider.height = 1.6f;
            playerColider.center = new Vector3(0f, 0, 0f);
            camTarget = camPos1;
            isCrouching = false;
            animator.SetBool("Crouching", false);
            lastCrouchTime = Time.time;
            StartCoroutine(MoveCamera(camTarget));
        }
       
    }

    IEnumerator MoveCamera(Vector3 camTarget)
    {
        while (Vector3.Distance(playerCamera.localPosition, camTarget) >= 0.04f)
        {
            float ms = 3.5f * Time.deltaTime;
            playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, camTarget, ms);
            yield return null;
        }
    }
}