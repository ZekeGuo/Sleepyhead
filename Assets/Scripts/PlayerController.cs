using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Animator anim;
    private string currentState;

    public float speed = 8f;
    public float rollingSpeed = 4f;
    public float crouchSpeed = 4f;
    public float sprintSpeed = 16f;

    public float turnSmoothTime = 0.1f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    float turnSmoothVelocity;

    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public float waterDistance = 0.1f;
    public float activeClosetDistance = 2.0f;
    public LayerMask groundMask;
    public LayerMask waterMask;

    Vector3 velocity;
    bool isGrounded;
    bool isInWater;
    bool isCrouching;
    bool isJumping;

    // Animation States
    const string STAND_IDLE = "Idle";
    const string CROUCH_IDLE = "Crouch idle";
    const string STAND_TO_CROUCH = "Stand to crouch";
    const string SWIM_IDLE = "Floating in water";
    const string FALL_IDLE = "Falling Idle";
    const string WALK = "Walking";
    const string CROUCH_WALK = "Crouch walking";
    const string SWIM = "Swim";
    const string RUN = "Standard Run";
    const string ROLL = "Stand To Roll";
    const string JUMP = "Jump";

    // The money you have
    public int coinNum;


    void Start()
    {
        anim = GetComponent<Animator>();
        isJumping = false;
        coinNum = 0;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Coin")
        {
            Debug.Log("Coin");
            coinNum ++;
            GameObject.Find("coinNumber").GetComponent<Text>().text = coinNum.ToString();
            Destroy(hit.gameObject);
        }
    }

    void Update()
    {
        // check the collision continiously
        isGrounded = controller.isGrounded;
        isInWater = Physics.CheckSphere(groundCheck.position, waterDistance, waterMask);


        // read the inputs 
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;


        // Standing Idle
        if ((direction.magnitude <= 0.1f) && isGrounded && !isCrouching && !isJumping)
        {
            // play stand idle animation
            ChangeAnimationState(STAND_IDLE);
        }


        // Falling
        if (!isGrounded && !isJumping && !isInWater && direction.magnitude >= 0.1f)
        {
            // you can move during falling
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * crouchSpeed * Time.deltaTime);
        }


        // Walking 
        if (direction.magnitude >= 0.1f && !isInWater && (!Input.GetButton("Fire3")) && isGrounded && !isJumping)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // walk slowly if player is crouching
            if (isCrouching)
            {
                controller.Move(moveDir.normalized * crouchSpeed * Time.deltaTime);
            } 
            else
            {
                // play walk animation
                ChangeAnimationState(WALK);
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
        }


        // Running
        if ((direction.magnitude >= 0.1f) && !isInWater && Input.GetButton("Fire3") && isGrounded && !isJumping)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // don't run when player is crouching
            if (isCrouching)
            {
                controller.Move(moveDir.normalized * crouchSpeed * Time.deltaTime);
            }
            else if (!isGrounded)
            {
                controller.Move(moveDir.normalized * crouchSpeed * Time.deltaTime);
            }
            else
            {
                // play run animation
                ChangeAnimationState(RUN);
                controller.Move(moveDir.normalized * sprintSpeed * Time.deltaTime);
            }
        }


        // When press C, invert the value of isCrouching.
        if (Input.GetKeyDown(KeyCode.C) && isGrounded && (direction.magnitude <= 0.1f))
        {
            isCrouching = !isCrouching;
            anim.SetBool("Crouch", isCrouching);
        }

        if (isCrouching && isGrounded)
        {
            if (direction.magnitude > 0.1f)
                anim.SetBool("Walking", true);
            else
                anim.SetBool("Walking", false);
        } 

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            anim.SetBool("Falling", false);
            Invoke("JumpAnimation", 0.45f);
            Invoke("StopJump", 1.0f);
            isJumping = true;
            ChangeAnimationState(JUMP);
        }

        if (direction.magnitude >= 0.1f && isJumping)
        {
            // you can move during jumping
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * crouchSpeed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime * 1.5f;
        controller.Move(velocity * Time.deltaTime);


        // Floating in water && swim
        if (isInWater)
        {
            anim.SetBool("InWater", true);
            if (direction.magnitude >= 0.1f)
            {
                anim.SetBool("Walking", true);
                ChangeAnimationState(SWIM);
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * crouchSpeed * Time.deltaTime);
            }
            else
            {
                anim.SetBool("Walking", false);
            }
        } 
        else
        {
            anim.SetBool("InWater", false);
        }

        // activate the changing costume functionality when approaching the closet
        float distance = Vector3.Distance(GameObject.Find("Closet").transform.position, transform.position);
        if (distance < 2.5)
        {
            GameObject.Find("Parent_Player").GetComponent<CostumeSwitch>().isChanging = true;
        }
        else
        {
            GameObject.Find("Parent_Player").GetComponent<CostumeSwitch>().isChanging = false;
        }



    }

    void ChangeAnimationState(string newState)
    {
        // stop the same animation from interrupting itself
        if (currentState == newState)
        {
            return;
        }

        // play the animation
        anim.Play(newState);

        // resign the new state
        currentState = newState;

    }

    // add jumping speed
    void JumpAnimation()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    void StopJump()
    {
        isJumping = false;
        anim.SetBool("Falling", true);
    }

}