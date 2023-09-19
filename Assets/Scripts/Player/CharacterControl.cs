using UnityEngine;

public class CharacterControl : MonoBehaviour {

    //public Animator AC;
    public CharacterController CC;
    public float Speed = 5;
	public float SpeedRotation = 10;

    private Vector3 _velocity;
    public float JumpHeight = 1;
    private float _jumpForce = 1;

    void Start()
    {
        _jumpForce = Mathf.Sqrt(2 * Physics.gravity.magnitude * JumpHeight);
        //if (AC == null) AC = gameObject.GetComponent<Animator>();
        _velocity.y = -CC.stepOffset * 10;
    }

    void FixedUpdate ()
	{
        MovementCharacter();
	}

    void MovementCharacter()
    {       
        if (CC.isGrounded)
        {
           // AC.SetBool("IsJumping", false);
            _velocity.y = -CC.stepOffset * 10;//So there is pressure towards the grounds, makes sure the next frame the character is still grounded

            if (Input.GetButtonDown("Jump"))
            {
               // AC.SetBool("IsJumping", true);
                ApplyJump();
            }
        }
        else
        {
            ApplyGravity();
        }

        ApplyMovement();

        CC.Move(_velocity * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        float y = _velocity.y;//backup

        float max = Mathf.Max(Mathf.Abs(Input.GetAxis("Vertical")), Mathf.Abs(Input.GetAxis("Horizontal")));
        _velocity = this.transform.forward * max * Speed;
        _velocity.y = y;
       // AC.SetBool("IsRunning", true);
        
        //if(max <= 0.1f) AC.SetBool("IsRunning", false); //idle
    }

    private void ApplyGravity()
    {
        _velocity.y += Physics.gravity.y * Time.deltaTime;
    }

    private void ApplyJump()
    {
        
        _velocity += -Physics.gravity.normalized * Mathf.Sqrt(2 * Physics.gravity.magnitude * _jumpForce);
    }

    public void TargetRotation( Quaternion rot)
	{
		this.transform.rotation = Quaternion.Slerp( this.transform.rotation, rot, SpeedRotation * Time.deltaTime);
	}
}
