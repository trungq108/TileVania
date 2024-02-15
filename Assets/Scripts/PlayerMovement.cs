using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D playerRb;
    Animator animator;
    CapsuleCollider2D playerCollider;
    BoxCollider2D feetCollider;

    float gravityScale;
    bool isAlive = true;

    [SerializeField] float runSpeed;
    [SerializeField] float climbSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] Transform gunPosition;
    [SerializeField] GameObject bullet;
    [SerializeField] ParticleSystem deathVFX;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        feetCollider= GetComponent<BoxCollider2D>();
        gravityScale = playerRb.gravityScale;
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
        playerRb.velocity += new Vector2(0f, jumpHeight);       
    }

    void Run()
    {
        Vector2 runVelocity = new Vector2(moveInput.x * runSpeed, playerRb.velocity.y) ;
        playerRb.velocity = runVelocity;
        bool isMoving = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", isMoving);  
    }

    void FlipPlayer()
    {
        bool isMoving = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;
        if (isMoving)
        {
            transform.localScale = new Vector3(Mathf.Sign(playerRb.velocity.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void ClimbingLadder()
    {
        if(!playerCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) 
        {
            animator.SetBool("isClimbing", false);
            playerRb.gravityScale = gravityScale;
            return; 
        }

        playerRb.gravityScale = 0f;
        Vector2 climbVelocity = new Vector2(playerRb.velocity.x, moveInput.y * climbSpeed);
        playerRb.velocity = climbVelocity;
        bool isClimbing = Mathf.Abs(playerRb.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", isClimbing);
    }

    void Die()
    {
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            animator.SetTrigger("Death");
            playerRb.velocity = new Vector2(-playerRb.velocity.x, 10f);
            deathVFX.Play();
            FindObjectOfType<GameSession>().PlayerDeathSequence();
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        Instantiate(bullet, gunPosition.position, transform.rotation);                  
    }
}
