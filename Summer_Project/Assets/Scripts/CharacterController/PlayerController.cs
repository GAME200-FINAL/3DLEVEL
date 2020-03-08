/*
 * UCSC Level Design Toolkit
 * 
 * Aidan Kennell
 * akennell94@gmail.com
 * 
 * This script handles all of the input to the player character (Jump, attack, sneak, run, walk, turn)
 * 
 * Released under MIT Open Source License
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]

public class PlayerController : ControllerBase
{
    Transform mainCamera;
    public Rigidbody rigidBody;
    public string footStepEvent;
    public string jumpEvent;
    public string attackEvent;
    public bool isAttacking;
    protected bool isSneaking;
    protected float capsuleRadius;
    float tGravity = -10;
    public float moveSpeed;
    public float rotateSpeed;
    public bool isGrounded;
    public bool falling = false;
    public bool onRamp;
    RaycastHit rhit;
    public GameObject Direction;
    void Start()
    {
        Physics.gravity = new Vector3(0, tGravity, 0);
        isAttacking = false;
        isSneaking = false;
        attackTimer = 0.0f;
        distanceToGrounded = .35f;
        input = new ControllerInput();
        animator = transform.GetComponent<Animator>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        hasDoubleJumped = false;
        capsuleRadius = transform.GetComponent<CapsuleCollider>().radius;
    }

    void Update()
    {
        
        updateStates();
        input.read();

        handleJump();

        if (!isSneaking)
        {
            handleAttack();
        }

        if (!isAttacking&&input.isGrounded)
        {
           handleMove();
        }

    }

    private void FixedUpdate()
    {
       // rigidBody.AddForce(0, -9.8f * gravity, 0);
    }

    /*
     * Requires: The character's origin to be set at its feet, and the layer mask needs to be set to any
     *           layers that are considered the ground. The "ground" is defined as anything the player can 
     *           walk on
     * Modifies: input.isGrounded(bool), hasDoubleJumped(bool) and distanceToGrounded(float)
     * Returns: Nothing
     */
    public override void updateStates()
    {
        base.updateStates();
        
       
        if (Physics.Raycast(transform.position + transform.up*0.1f - transform.forward * capsuleRadius, -transform.up,out rhit, 1, groundLayer) || 
            Physics.Raycast(transform.position + transform.up*0.1f + transform.forward * capsuleRadius, -transform.up,out rhit, 1, groundLayer) || Physics.Raycast(transform.position, -transform.up,out rhit, 1, groundLayer))
        {
            if (falling)
            {
                //transform.rotation = Quaternion.LookRotation(Vector3.Cross(rhit.normal, Vector3.Cross(transform.forward, rhit.normal)),
                //           rhit.normal);
                Direction.transform.rotation= transform.rotation;
                //Camera.main.GetComponent<CamContrl>().Reset();
                falling = false;
            }
            input.isGrounded = true;
            hasDoubleJumped = false;
        }
        else
        {
            input.isGrounded = false;
        }

        isGrounded = input.isGrounded;
        if (!isGrounded) falling = true;
        animator.SetBool("IsGrounded", input.isGrounded);
    }

    /*
     * Requires: That there be a rigid body attached to the game object
     * Modifies: Applies a force, in the positive y, to a rigidbody component attached to the game object that this script is a component of
     * Returns: Nothing
     */
    public override void handleJump()
    {
        base.handleJump();

        if (input.jump)
        {
            float speedMod = input.throttle * input.throttle * Time.deltaTime * maxSpeed;
            Vector3 jumpForce = new Vector3(0, jumpSpeed * FORCE_MOD, 0);
            jumpForce += transform.forward * speedMod * forwardJumpForce * FORCE_MOD;
           // rigidBody.AddForce(jumpForce);
           
        }

        //if (input.doubleJump && !hasDoubleJumped && canDoubleJump)
        //{
        //    float jumpForce = doubleJumpSpeed * FORCE_MOD;

        //    //We zero out the velocity in the Y to make sure that even if the player is falling they will go the full height of the double jump
        //    rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
        //    rigidBody.AddForce(0, jumpForce, 0);
        //    hasDoubleJumped = true;
        //}


    }

    /* 
     * Requires: That there is a camera in the scene tagged as main camera
     * Modifies: Rotatates the game object this script is attached to and translates the same game object on the x,z plane
     * Returns: Nothing
     */   
    public override void handleMove()
    {
        // base.handleMove();
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 f = Direction.transform.forward.normalized;
        //f.y = 0;
        Vector3 r = Direction.transform.right.normalized;
        //r.y = 0;
        Vector3 targetSpeed = v * f + r * h;
        GetComponent<Rigidbody>().velocity = transform.forward * Input.GetAxis("Vertical")* moveSpeed;
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime, 0);
        // Vector3 targetMove = targetSpeed * 
        //animator.SetFloat("Speed", Mathf.Sqrt(v * v + h * h));
        //onRamp = isOnRamp();
        //if (onRamp)
        //{
        //    transform.rotation = Quaternion.LookRotation(Vector3.Cross(rhit.normal, Vector3.Cross(transform.forward, rhit.normal)),
        //                   rhit.normal);
        //}
        /* {
         * Turn the character using Quaternions to avoid gimbal lock and 0/360 wraparound issues that come from using Eular angles
         * The heading is multiplied by the camera's rotation in order to get the player character to move relative to the camera's 
         * rotation. Zeroing out the x and z components ensures that the character is only moved in relation to the camera's rotaion
         * around the y axis.
         */
        //if (input.throttle > input.deadZone)
        //{
        //    Quaternion cameraRotation = mainCamera.rotation;
        //    cameraRotation.x = 0;
        //    cameraRotation.z = 0;

        //    Quaternion targetRotation = Quaternion.AngleAxis(input.heading, new Vector3(0, 1, 0)) * cameraRotation;

        //    float rotationGap = Mathf.Abs(targetRotation.eulerAngles.y - transform.rotation.eulerAngles.y);

        //    //The less than 190 is to ensure that we are not doing the instant turn on any 0 to 360 wrap around
        //    if (rotationGap > instantTurnAngle && rotationGap < 190)
        //    {
        //        transform.rotation = targetRotation;
        //    }
        //    else
        //    {
        //        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //    }
        //}


        //Move the player character, and set the speed parameter in the animator to make sure the right blend state is played
        //float speedMod;

        ////if the character is sneaking reduce the max speed to 3/5 it's origional amount
        //if (isSneaking)
        //{
        //    speedMod = input.throttle * input.throttle * Time.deltaTime * (maxSpeed * 3.0f/5.0f);
        //}
        //else
        //{
        //    speedMod = input.throttle * input.throttle * Time.deltaTime * maxSpeed;
        //}


        //if (input.isGrounded)
        //{
        //    transform.Translate(0, 0, speedMod);
        //    animator.SetFloat("Speed", input.throttle);
        //}
        //else
        //{
        //    transform.Translate(0, 0, speedMod * airControl);
        //}
    }

    /*
     * Requires: nothing
     * Modifies: isAttacking(bool), attackTimer(float), and the player character's position
     * Returns: nothing
     */
    public override void handleAttack()
    {
        base.handleAttack();

        if (input.attack && !isAttacking)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
           if( Physics.Raycast(ray, out hit))
            {
                Physics.gravity = -hit.normal * 10;
            }
            Physics.gravity = Camera.main.transform.forward*10;
            Quaternion targetRotation = Quaternion.LookRotation(Camera.main.transform.forward);
            transform.rotation = targetRotation * Quaternion.Euler(-90, 0, 0);
            isAttacking = true;

            animator.Play("Attack");
            animator.Play("AttackStep");
            
            attackTimer = 0;
        }

        if (isAttacking)
        {
            //This translates the player forward
           // transform.position += transform.forward * attackTranslationMultiplier * Time.deltaTime;
            attackTimer += Time.deltaTime;

            if (attackTimer >= timeBetweenAttacks)
            {
                isAttacking = false;
            }
        }

        //animator.SetBool("Attacking", isAttacking);
    }

    /*
     * Requires: A trigger volume component the size of the attack area
     * Modifies: Nothing, but will call a funtion on any enemies that are inside the trigger volume to kill/damage them
     * Returns: Nothing
     */
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy" && isAttacking)
        {
            //kill enemy
            Destroy(other.gameObject);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    foreach (ContactPoint contact in collision.contacts)
    //    {
    //        if (contact.point.y > transform.position.y && contact.point.y - transform.position.y < stepUpHeight)
    //        {
    //            transform.position = new Vector3(transform.position.x, contact.point.y, transform.position.z);
    //        }
    //    }
    //}

    public override void freezeMovement()
    {
        base.freezeMovement();

        animator.SetFloat("Speed", 0.0f);
    }

    public override void unFreezeMovement()
    {
        base.unFreezeMovement();
    }

    public void toggleSneak()
    {
        isSneaking = !isSneaking;
        animator.SetBool("Sneaking", isSneaking);
    }

    public bool getSneaking() { return isSneaking; }


    void PlayFootstep()
    {
        AudioManager.PlaySound(footStepEvent, gameObject);
    }

    void PlayJump()
    {
        AudioManager.PlaySound(jumpEvent, gameObject);
    }

    void PlayAttack()
    {
        AudioManager.PlaySound(attackEvent, gameObject);
    }
    public bool isOnRamp()
    {
        return (Physics.Raycast(transform.position + transform.up*0.1f, -transform.up, out rhit, 2f, 1 << 9));
    }
}

public class ControllerInput : VirtualInput
{
    public ControllerInput() : base()
    {
    }

    /*
     * Requires: Nothing
     * Modifies: throttle(float), heading(float), and jump(bool)
     * Returns: Nothing
     * 
     * Reads input and stores any changes, so that the player controller can access them later
     */
    public override void read()
    {
        base.read();

        if (!freezeMovement)
        {
            //Gets the joystick and WASD inputs
            Vector3 move = Vector3.zero;
            move.x = Input.GetAxisRaw("Horizontal");
            move.z = Input.GetAxisRaw("Vertical");

            //Ensure that the joy stick is pushed outside of the deadzone
            if (move.magnitude > deadZone)
            {
                throttle = Mathf.Min(move.magnitude, 1.0f);
                heading = Mathf.Rad2Deg * Mathf.Atan2(move.x, move.z);
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                jump = true;
            }

            if (Input.GetButtonDown("Jump") && !isGrounded)
            {
                doubleJump = true;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                attack = true;
            }
        }
    }
  
}
