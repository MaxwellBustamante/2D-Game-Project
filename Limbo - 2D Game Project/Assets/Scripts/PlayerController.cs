using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private float movementInputDirection;
    private float verticalInputDirection;

    private bool isGrounded;
    private bool isTouchingPipe;
    private bool canJump;
    private bool canClimb;
    private bool isFacingLeft = true;

    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float climbingSpeed = 10.0f;
    public float groundCheckRadius;
    public float pipeCheckRadius;

    public Transform groundCheck;
    public Transform pipeCheck;

    public LayerMask whatIsGround;
    public LayerMask whatIsPipe;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        CheckInput();
        CheckIfCanJump();
        CheckIfCanClimb();
        CheckMovementDirection();
    }

    private void FixedUpdate()
    {
       ApplyMovement(); 
       CheckSurroundings();
    }
    private void CheckMovementDirection()
    {
        if(isFacingLeft && movementInputDirection > 0)
        {
            Flip();
        }
        else if(!isFacingLeft && movementInputDirection < 0)
        {
            Flip();
        }
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
        verticalInputDirection = Input.GetAxisRaw("Vertical");

        if(Input.GetButtonDown("Jump"))
        {
            if(!canClimb)
            {
                Jump();
            }       
        }
        if(Input.GetButtonDown("Vertical"))
        {
            Climb();
        }
    }

    private void CheckIfCanJump()
    {
        if(isGrounded)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }

    private void CheckIfCanClimb()
    {
        if(isTouchingPipe)
        {
            canClimb = true;
            rigidBody.bodyType = RigidbodyType2D.Kinematic;
        }
        else
        {
            canClimb = false;
            rigidBody.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void ApplyMovement()
    {
        rigidBody.velocity = new Vector2(movementSpeed * movementInputDirection, rigidBody.velocity.y);
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isTouchingPipe = Physics2D.OverlapCircle(pipeCheck.position, pipeCheckRadius, whatIsPipe);
    }

    private void Jump()
    {   
        if(canJump)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        }  
    }

    private void Climb()
    {
        if(canClimb)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, climbingSpeed * verticalInputDirection);     
        }
        
    }

    private void Flip()
    {
        isFacingLeft = !isFacingLeft;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(pipeCheck.position, pipeCheckRadius);
    }
}
