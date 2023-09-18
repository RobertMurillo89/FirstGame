using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    public PlayerSounds PlayerSounds;

    [Header("----- Player Movement -----")]
    [SerializeField] float MoveSpeed;
    [SerializeField] float SprintMod;
    [SerializeField] int JumpMax;
    [SerializeField] int JumpHeight;
    [SerializeField] int GravityValue;
    private bool groundedPlayer;
    private Vector3 moveDirection;
    private Vector3 playerVelocity;
    private int jumpCount;
    private bool isSprinting;
    private bool isJumping;
    private float lastJumpTime = 0f;
    private float jumpCooldown = 1f;

    //isJumping = true;
    ////if (groundedPlayer && isJumping)
    ////{
    ////    PlayerSounds.LandEmote();
    ////}
    //isJumping = false;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        groundedPlayer = controller.isGrounded;

        //Handle Grounded State
        if (groundedPlayer)
        {
            playerVelocity.y = 0f; // Ensures the player does not accumulate downward velocity when grounded.
            jumpCount = 0;

        }

        //Handle Input
        moveDirection = (Input.GetAxis("Horizontal") * transform.right) +
               (Input.GetAxis("Vertical") * transform.forward);
        moveDirection.Normalize();
        controller.Move(moveDirection * Time.deltaTime * MoveSpeed);
        
        //Handle Jumping
        if (Input.GetButtonDown("Jump") && jumpCount < JumpMax && Time.time - lastJumpTime > jumpCooldown)
        {
            lastJumpTime = Time.time;
            playerVelocity.y += JumpHeight;
            PlayerSounds.JumpEmote();
            jumpCount++;
        }
        
        //Apply Gravity
        playerVelocity.y += GravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        playerVelocity = moveDirection * MoveSpeed;
        if (moveDirection.magnitude > 0f && groundedPlayer)
        {
            PlayerSounds.PlayFootstep(playerVelocity);            
        }
        Sprint();

    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            MoveSpeed *= SprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            MoveSpeed /= SprintMod;
        }
    }
}
