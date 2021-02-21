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
    private bool isNearStick;
    private bool canJump;
    private bool canClimb;
    private bool canBreakStick;
    private bool isFacingLeft = true;
    private bool powered = true;

    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float climbingSpeed = 10.0f;
    public float groundCheckRadius;
    public float pipeCheckRadius;

    public float stickCheckRadius;

    public Transform groundCheck;
    public Transform pipeCheck;
    public Transform stickCheck;

    public LayerMask whatIsGround;
    public LayerMask whatIsPipe;
    public LayerMask whatIsStick;

    public GameObject stick;
    public GameObject brokenStick1;
    public GameObject brokenStick2;
    public GameObject fallingLog;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        CheckInput();
        CheckIfCanJump();
        CheckIfCanClimb();
        CheckIfCanBreakStick();
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
        if(Input.GetButtonDown("Fire1"))
        {
            BreakStick();
            TurnOffGenerator();
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

    private void CheckIfCanBreakStick()
    {
        if(isNearStick)
        {
            canBreakStick = true;
        }
        else
        {
            canBreakStick = false;
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
        isNearStick = Physics2D.OverlapCircle(stickCheck.position, stickCheckRadius, whatIsStick);
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

    private void BreakStick()
    {
        if(canBreakStick)
        {
            Destroy(stick);
            ReplaceStick();
            FallLog();
        }
    }

    private void ReplaceStick()
    {
        brokenStick1.GetComponent<SpriteRenderer>().enabled = true;
        brokenStick1.GetComponent<Rigidbody2D>().simulated = true;

        brokenStick2.GetComponent<SpriteRenderer>().enabled = true;
        brokenStick2.GetComponent<Rigidbody2D>().simulated = true;
    }

    private void FallLog()
    {
        fallingLog.GetComponent<Rigidbody2D>().simulated = true;
    }

    private void TurnOffGenerator()
    {
        RaycastHit2D hit;
        if(!isFacingLeft)
        {
            hit = Physics2D.Raycast(rigidBody.position, Vector2.right, 1.0f, LayerMask.GetMask("Generator"));
        }
        else
        {
            hit = Physics2D.Raycast(rigidBody.position, Vector2.left, 1.0f, LayerMask.GetMask("Generator"));
        }

        if(hit.collider != null)
        {
            ElectricityController.instance.SetIsPowered(!powered);
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
        Gizmos.DrawWireSphere(stickCheck.position, stickCheckRadius);
    }
}
