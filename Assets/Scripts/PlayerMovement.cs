using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    bool isAlive = true;

    [SerializeField] float runSpeed;
    [SerializeField] float climbSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] Vector2 death;
    [SerializeField] ParticleSystem deathVFX;

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
        if(!isAlive) { return; }
        Run();
        FlipPlayer();
        ClimbingLadder();
        Die();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) || !isAlive) { return; }

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

    void Die()
    {
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            isAlive = false;
            animator.SetTrigger("Death");
            rb.velocity = death;
            deathVFX.Play();
        }

    }
}
