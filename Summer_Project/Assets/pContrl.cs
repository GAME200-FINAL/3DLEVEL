using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pContrl : MonoBehaviour
{
    public Animator animator;
    public bool isGrounded;
    public bool falling;
    public bool onRamp;
    RaycastHit rhit;
    public float moveSpeed;
    public GameObject p;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, -transform.up, 1, 1<<9))
        {
            if (falling)
            {
                Camera.main.GetComponent<CamContrl>().Reset();
                falling = false;
            }
            isGrounded = true;
            //hasDoubleJumped = false;
        }
        else
        {
            isGrounded = false;
        }

        if (!isGrounded) falling = true;
        animator.SetBool("IsGrounded", isGrounded);
        handleMove();
        handleAttack();
    }
    public void handleMove()
    {
        // base.handleMove();
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 f = Camera.main.transform.forward.normalized;
        //f.y = 0;
        Vector3 r = Camera.main.transform.right.normalized;
        //r.y = 0;
        Vector3 targetSpeed = v * f + r * h;
        // Vector3 targetMove = targetSpeed * 
        if (h != 0 || v != 0)
        {
            p.transform.rotation = Quaternion.LookRotation(targetSpeed);
            transform.position += p.transform.forward * moveSpeed * Time.deltaTime; 
        }
        animator.SetFloat("Speed", Mathf.Sqrt(v * v + h * h));
        onRamp = isOnRamp();
        //if (onRamp)
        //{
        //    p.transform.rotation = Quaternion.LookRotation(Vector3.Cross(rhit.normal, Vector3.Cross(p.transform.forward, rhit.normal)),
        //                   rhit.normal);
        //}
    }
    public bool isOnRamp()
    {
        return (Physics.Raycast(transform.position + transform.up * 0.1f, -transform.up, out rhit, 2f, 1 << 9));
    }
    public void handleAttack()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            Physics.gravity = Camera.main.transform.forward * 10;
            Quaternion targetRotation = Quaternion.LookRotation(Camera.main.transform.forward);
            transform.rotation = targetRotation * Quaternion.Euler(-90, 0, 0);
            p.transform.localRotation = Quaternion.Euler(0, 0, 0);
            animator.Play("Attack");
            animator.Play("AttackStep");
        }


        //animator.SetBool("Attacking", isAttacking);
    }
}
