using UnityEngine;

public class ThirdPersonCameraControl : MonoBehaviour {

	public Transform Target;
	public Vector3 Offset = new Vector3(0,1.5f,0);

	public Vector2 LimitY;

	public float SpeedRotate = 180;

	private Transform _Y;

	public CharacterControl CC;

	void Start ()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		_Y = this.transform.GetChild(0);
	}
	
	void Update ()
	{
        this.transform.position = Target.position + Offset;//Follows the player

        RotateCamera();

        ApplyLookAt();
    }

    void RotateCamera()
    {
        this.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * SpeedRotate * Time.deltaTime);

        _Y.Rotate(Vector3.right, Input.GetAxis("Mouse Y") * -SpeedRotate * Time.deltaTime, Space.Self);

        _Y.localEulerAngles = ClampRotation(_Y.localEulerAngles);
    }

    void ApplyLookAt()
    {
        //Makes the character look in that direction
        if ( Input.GetButton("Vertical"))
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                CC.TargetRotation(this.transform.rotation);
            }
            else
            {
                CC.TargetRotation(this.transform.rotation * Quaternion.Euler(0, -180, 0));
            }
        }
        else if (Input.GetButton("Horizontal"))//Look left or right
        {
            CC.TargetRotation(this.transform.rotation * Quaternion.Euler(0, 90 * Mathf.Sign(Input.GetAxis("Horizontal")), 0));
        }
    }

    Vector3 ClampRotation( Vector3 eulerangles)
    {
        if (eulerangles.x > 180)
        {
            eulerangles.x = LimitY.x;
        }

        eulerangles.x = Mathf.Clamp(eulerangles.x, LimitY.x, LimitY.y);
        eulerangles.y = 0;
        eulerangles.z = 0;
        return eulerangles;
    }
}
