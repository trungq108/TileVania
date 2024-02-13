using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rb;
    Animator animator;
    CapsuleCollider2D playerCollider;
    BoxCollider2D feetCollider;

    float gravityScale;

    [SerializeField] float runSpeed;
    [SerializeField] float climbSpeed;
    [SerializeField] float jumpHeight;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        feetCollider= GetComponent<BoxCollider2D>();    
        gravityScale = rb.gravityScale;
    }

    void Update()
    {
        Run();
        FlipPlayer();
        ClimbingLadder();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (value.isPressed)
        {
            rb.velocity += new Vector2(0f, jumpHeight);
        }
    }

    void Run()
    {
        Vector2 runVelocity = new Vector2(moveInput.x * runSpeed, rb.velocity.y) ;
        rb.velocity = runVelocity;
        bool isMoving = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", isMoving);  
    }

    void FlipPlayer()
    {
        bool isMoving = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        if (isMoving)
        {
            transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void ClimbingLadder()
    {
        if(!playerCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) 
        {
            animator.SetBool("isClimbing", false);
            rb.gravityScale = gravityScale;
            return; 
        }

        rb.gravityScale = 0f;
        Vector2 climbVelocity = new Vector2(rb.velocity.x, moveInput.y * climbSpeed);
        rb.velocity = climbVelocity;
        bool isClimbing = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", isClimbing);
    }
}
