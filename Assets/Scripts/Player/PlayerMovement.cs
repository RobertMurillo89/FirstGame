using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public PlayerSounds PlayerSounds;
    public CharacterController Controller;

    public float Speed;
    public float SprintMod;
    public float Gravity;
    public float JumpHeight;

    public Transform GroundCheck;
    public float GroundDistance;
    public LayerMask GroundMask;

    Vector3 Velocity;
    bool isGrounded;
    bool isSprinting;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Sprint();

    }

    void Movement()
    {
        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        if (isGrounded && Velocity.y < 0)
        {
            Velocity.y = -2f; // not set to 0 because -2 works better
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * x + transform.forward * z;

        Controller.Move(moveDirection * Speed * Time.deltaTime);

        //HandleFootSteps

        if (moveDirection.magnitude > 0f && isGrounded)
        {
            PlayerSounds.PlayFootstep(Velocity);
        }

        //Handle Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            PlayerSounds.JumpEmote();
        }

        //apply gravity
        Velocity.y += Gravity * Time.deltaTime;

        Controller.Move(Velocity * Time.deltaTime);

        
    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            Speed *= SprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            Speed /= SprintMod;
        }
    }
}
