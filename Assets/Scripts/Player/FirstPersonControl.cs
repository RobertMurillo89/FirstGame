using UnityEngine;

public class FirstPersonControl : MonoBehaviour {

    public CharacterController CC;
	public float Speed = 5;
	public float SpeedRotate = 180;

	private Transform _cam;

    private Vector3 _velocity;
    public float JumpHeight = 1;
    private float _jumpForce = 1;

    void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		_cam = Camera.main.transform;

        _jumpForce = Mathf.Sqrt(2 * Physics.gravity.magnitude * JumpHeight);
    }

	void Update ()
	{
        RotateCharacterAndCamera();
        MovementCharacter();
	}

    void MovementCharacter()
    {
        if (CC.isGrounded)
        {
            _velocity.y = -CC.stepOffset * 10;//So there is pressure towards the grounds, makes sure the next frame the character is still grounded

            if ( Input.GetButtonDown("Jump"))
            {
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

        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        dir = this.transform.TransformDirection(dir);
        _velocity = dir * Speed;

        _velocity.y = y;
    }

    void RotateCharacterAndCamera()
    {
        this.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * SpeedRotate * Time.deltaTime);

        _cam.Rotate(Vector3.right, Input.GetAxis("Mouse Y") * -SpeedRotate * Time.deltaTime);

        Vector3 r = _cam.eulerAngles;
        r.x = ClampAngle(r.x, -20, 20);

        _cam.eulerAngles = r;
    }

    private void ApplyGravity()
    {
        _velocity.y += Physics.gravity.y * Time.deltaTime;
    }

    private void ApplyJump()
    {
        _velocity += -Physics.gravity.normalized * Mathf.Sqrt(2 * Physics.gravity.magnitude * _jumpForce);
    }

    float ClampAngle(float angle, float from, float to)
	{
		if(angle < 0f)
		{	
			angle = 360 + angle;
		}
		else if(angle > 180f)
		{
			return Mathf.Max(angle, 360+from);
		}
		return Mathf.Min(angle, to);
	}
}
