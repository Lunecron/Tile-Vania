using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpHight = 18f;
    [SerializeField] float climbSpeed = 2f;
    float playerGravity;

    Animator playerAnimator;
    Vector2 moveInput;
    Rigidbody2D playerRigidbody;
    CapsuleCollider2D playerCapsuleCollider;
    BoxCollider2D playerBoxCollider;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCapsuleCollider = GetComponent<CapsuleCollider2D>();
        playerBoxCollider = GetComponent<BoxCollider2D>();
        playerGravity = playerRigidbody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    private void FlipSprite()
    {
        float xVelocity = playerRigidbody.velocity.x;
        bool playerHasHorizontalSpeed = Mathf.Abs(xVelocity) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(xVelocity), transform.localScale.y);
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

    }

    void OnJump(InputValue value)
    {
        if (!playerBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
 
            return;
        }

        if (value.isPressed)
        {

            playerRigidbody.velocity += new Vector2(0f, jumpHight);
        }

        
    }

    private void ClimbLadder()
    {
        bool playerHasVerticalSpeed = Mathf.Abs(playerRigidbody.velocity.y) > Mathf.Epsilon;

        Debug.Log("Layer:"+ !playerCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")));
        if (!playerCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            playerRigidbody.gravityScale = playerGravity;
            playerAnimator.SetBool("isClimbing", false);
            return ;
        }

        playerRigidbody.gravityScale = 0;
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, moveInput.y * climbSpeed);

        Debug.Log(playerHasVerticalSpeed);
        playerAnimator.SetBool("isClimbing", playerHasVerticalSpeed);

    }
    private void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, playerRigidbody.velocity.y);
        playerRigidbody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;

        playerAnimator.SetBool("isRunning", playerHasHorizontalSpeed);

    }

    
    

}
