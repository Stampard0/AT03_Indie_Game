using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float gravity = 3.5f; // gravity
    [Range(1f, 10f)]
    public float speed = 2f; // normal player speed
    [Range(0.1f, 1f)]
    public float crouchSpeedMultiplier = 0.5f; //crouch speed
    public float jumpForce = 0.5f; // jump force
    public float sprintSpeedMultiplier = 1.25f; //sprint speed

    private CharacterController controller;
    private Vector3 motion;
    private float currentSpeed = 0;
    private float velocity = 0;
    private bool crouching = false;
    private bool isGrounded = false;
    private bool sprint = false;
    public static bool canMove = true;

    // Awake is called before start method
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = speed;
        canMove = true;
    }

    private void FixedUpdate()
    {
        isGrounded = controller.isGrounded;
        motion = Vector3.zero;

        if (isGrounded == true)
        {
            velocity = -gravity * Time.deltaTime;
        }
        else
        {
            velocity -= gravity * Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isGrounded == true)
        {
            if(crouching == false)
            {
                if(Input.GetAxis("Jump") >= 1)
                {
                    velocity = jumpForce;
                }
                else if(Input.GetKeyDown(KeyCode.C) == true)
                {
                    crouching = true;
                    currentSpeed = speed * crouchSpeedMultiplier;
                    controller.height = 1;
                }
                else if(Input.GetAxis("Fire3") >= 1)
                {
                    sprint = true;
                    currentSpeed = speed * sprintSpeedMultiplier;
                    Debug.Log("Sprinting");
                }
            }
        }
        if (crouching == true)
        {
            if (Input.GetKeyUp(KeyCode.C) == true)
            {
                crouching = false;
                currentSpeed = speed;
                controller.height = 2;
            }
        }
        if (sprint == true)
        {
            if (Input.GetAxis("Fire3") == 0)
            {
                sprint = false;
                currentSpeed = speed;
                Debug.Log("Not Sprinting");
            }
        }
        ApplyMovement();
    }

    void ApplyMovement()
    {
        float inputX = Input.GetAxisRaw("Vertical") * currentSpeed;
        float inputY = Input.GetAxisRaw("Horizontal") * currentSpeed;
        motion += transform.forward * inputX * Time.deltaTime;
        motion += transform.right * inputY * Time.deltaTime;
        motion.y += velocity * Time.deltaTime;
        controller.Move(motion);
    }
}
