using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    public PlayerSounds PlayerSounds;

    [Header("----- Player Attack -----")]
    [SerializeField] float shootRate;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    private bool isShooting;

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


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        Sprint();
        if(Input.GetButton("Shoot") && !isShooting)
        {
            StartCoroutine(shoot());
        }
    }

    void MovePlayer()
    {
        groundedPlayer = controller.isGrounded;

        //Handle Grounded State
        if (groundedPlayer)
        {
            
            if(isJumping)
            {
                PlayerSounds.LandEmote();
            }

            playerVelocity.y = 0f;
            playerVelocity.x = 0f;
            playerVelocity.z = 0f;// Ensures the player does not accumulate downward velocity when grounded.
            jumpCount = 0;
            isJumping = false;

            //Handle FootStepSFX
            if (moveDirection.normalized.magnitude > 0f && !isJumping)
            {
                playerVelocity = moveDirection * MoveSpeed;
                PlayerSounds.PlayFootstep(playerVelocity);
            }
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
            playerVelocity.y = Mathf.Sqrt(JumpHeight * -2f * GravityValue);
            PlayerSounds.JumpEmote();
            jumpCount++;
            isJumping = true;
        }

        {
            //Apply Gravity
            playerVelocity.y += GravityValue * Time.deltaTime;
        }
        controller.Move(playerVelocity * Time.deltaTime);        

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

    IEnumerator shoot()
    {
        isShooting = true;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            IDamage damageable = hit.collider.GetComponent<IDamage>();

            if(damageable != null)
            {
                damageable.TakeDamage(shootDamage);
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
