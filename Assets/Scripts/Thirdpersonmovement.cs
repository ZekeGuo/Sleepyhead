using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thirdpersonmovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Animator anim;

    public float speed = 8f;
    public float rollingSpeed = 4f;
    public float crouchSpeed = 4f;
    public float sprintspeed = 16f;

    public float turnSmoothTime = 0.1f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    float turnSmoothVelocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private int rollingFrame = 0;

    Vector3 velocity;
    bool isGrounded;

    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {         
            velocity.y = -2f;
        }

        if (!isGrounded)
        {
            anim.SetBool("Falling", true);
        }
        else
        {
            anim.SetBool("Falling", false);
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Walking
        if ((direction.magnitude >= 0.1f) && (!Input.GetButton("Fire3")))
        {
            anim.SetBool("Walking", true);
            anim.SetBool("Standard Run", false);
            anim.SetBool("Jump", false);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Decide the walk speed based on standing/crouch
            if (anim.GetBool("Crouch") == true)
            {
                controller.Move(moveDir.normalized * crouchSpeed * Time.deltaTime);
            } else
            {
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }

        }

        // Idle
        if ((direction.magnitude <= 0.1f))
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Standard Run", false);
            anim.SetBool("Jump", false);
        }

        // Run
        if ((direction.magnitude >= 0.1f) && (Input.GetButton("Fire3")))
        {
            anim.SetBool("Standard Run", true);
            anim.SetBool("Jump", false);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * sprintspeed * Time.deltaTime);
        }

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            anim.SetBool("Jump", true);
            Invoke("JumpAnimation", 0.25f);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Punch
        if (Input.GetButtonDown("Fire1") && isGrounded)
        {
            anim.SetBool("Punch Combo", true);
            Invoke("Punch", 0.5f);
        }

        // Roll
        if (Input.GetButtonDown("Fire2") && isGrounded)
        {
            rollingFrame = 1800;
            anim.SetBool("Roll", true);
            Invoke("Roll", 3f);
        }

        if (rollingFrame > 0)
        {
            controller.Move(transform.forward * Time.deltaTime * rollingSpeed );
            rollingFrame --;
        }

        // Crouch
        if (Input.GetKeyDown(KeyCode.C) && isGrounded)
        {
            bool isCrounch = anim.GetBool("Crouch");
            anim.SetBool("Crouch", !isCrounch);
        }

    }
    void Punch()
    {
        anim.SetBool("Punch Combo", false);
    }
    void Roll()
    {
        anim.SetBool("Roll", false);
    }
    void JumpAnimation()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}