using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public KeyCode JumpKey = KeyCode.UpArrow, BreakStickKey = KeyCode.LeftControl, ClimbKey = KeyCode.UpArrow;
    public UnityEvent OnPlayerJump, OnPlayerDeath, OnPlayerWin, OnPlayerClimb;
    public float fallAnimationCheck = 0.1f;

    public LimboyAnimation LimboyAnimator;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        LimboyAnimator = GetComponent<LimboyAnimation>();
    }
    
    void Update()
    {
        CheckInput();
        CheckIfCanJump();
        CheckIfCanClimb();
        CheckIfCanBreakStick();
        CheckMovementDirection();
        CheckWalkAnimation();
    }

    private void CheckWalkAnimation()
    {
        if (isGrounded)
        {
            float xabs = Mathf.Abs(movementInputDirection);
            float yabs = Mathf.Abs(rigidBody.velocity.y);
            bool yabsokay = yabs < fallAnimationCheck;
            if(xabs > 0)
            {
                //LimboyAnimator.SetAnimationSpeed(1)
                if (!isTouchingPipe)
                {
                    LimboyAnimator.SetWalk();
                }
              
            }
            else if (isTouchingPipe)
            {
                LimboyAnimator.SetClimb();
            }
            else
            {
                if(yabsokay)
                {
                    //LimboyAnimator.SetAnimationSpeed(0);
                }
                
            }
        }
        else
        {
            //LimboyAnimator.SetAnimationSpeed(1);
        }

    }
    public void setPlayerDeath()
    {
        OnPlayerDeath.Invoke();
    }
    
    public void setPlayerWin()
    {
        OnPlayerWin.Invoke();
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

        if(Input.GetKeyDown(JumpKey))
        {
            if(!canClimb)
            {
                Jump();
            }       
        }
        if(Input.GetKeyDown(ClimbKey))
        {
            Climb();
        }
        if(Input.GetKeyDown(BreakStickKey))
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
        //Debug.LogError("Youre mom gay");
        if(canJump)
        {
            OnPlayerJump.Invoke();
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
            //Debug.LogError("YouGayHAGOTTEMREKTIDIOT");
        }  
    }

    private void Climb()
    {
        if(canClimb)
        {
            OnPlayerClimb.Invoke();
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
