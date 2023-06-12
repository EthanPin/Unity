using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public GameObject cam;      //automatically shown in the script section of the Inspector panel. this time set as the main camera
	float Xsensitivity = 8.0f;	//looking sensitivity
	float Ysensitivity = 8.0f;
	float MinimumX = -90;
	float MaximumX = 90;
	
	float speedMove = 0.5f;
	Rigidbody rb;
	CapsuleCollider capsule;
	
	Quaternion cameraRot;
	Quaternion characterRot;

	bool cursorIsLocked = true;	// for making the cursor disappear
	bool lockCursor = true;
	
	// Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();	//assigns the rigidbody component we added to the variable, similar to a pointer to the rigidbody
		capsule = this.GetComponent<CapsuleCollider>();
		
		cameraRot = cam.transform.localRotation;
		characterRot = this.transform.localRotation;
	}

    // Update is called once per frame
    void Update()
    {
		
		if(Input.GetKeyDown(KeyCode.Space) && IsGrounded())
			rb.AddForce(0,300,0);
		
    }
	
	//called for inputs like an interupt?
	void FixedUpdate()	
	{
		float yRot = Input.GetAxis("Mouse X") * Ysensitivity;				//setup mouse changes to a variable
		float xRot = Input.GetAxis("Mouse Y") * Xsensitivity;
		
		cameraRot *= Quaternion.Euler(-xRot, 0, 0);			//change the variables by how we move the mouse
		characterRot *= Quaternion.Euler(0, yRot, 0);
		
		cameraRot = ClampRotationAroundXAxis(cameraRot);
		
		this.transform.localRotation = characterRot;		//set the new values into our object
		cam.transform.localRotation = cameraRot;
		

		
		float x = Input.GetAxis("Horizontal") * speedMove;	//assigns the string used for keybaord inputs to the float
		float z = Input.GetAxis("Vertical") * speedMove;	//assigns the string used for keybaord inputs to the float
		//float y = Input.GetAxis("Jump");		//assigns the string used for keybaord inputs to the float ////this is not how you jump
		
		transform.position += cam.transform.forward * z + cam.transform.right * x;	//new Vector3(x * speedMove, 0, z * speedMove);
		UpdateCursorLock();	//checks if we should lock the mouse to the cursor
	}
	
	Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;
		
		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);	//converts the x value in Quaternion into an euler x value (rotation around the x-axis)
		angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);
		q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);		//returns to a quaternion value
		return q;
	}
	
	bool IsGrounded()
	{
		RaycastHit hitInfo;
		if(Physics.SphereCast(transform.position, capsule.radius, Vector3.down, out hitInfo,
			(capsule.height/2f) - capsule.radius + 0.1f))
			{
				return true;
			}
			return false;
	}

	public void SetCursorLock(bool value)		//method no longer needed?
    {
		lockCursor = value;
		if(!lockCursor)
        {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
        }
    }

	public void UpdateCursorLock()
    {
		if (lockCursor)
			InternalLockUpdate();
    }
	
	public void InternalLockUpdate()
    {
		if (Input.GetKeyUp(KeyCode.Escape))
			cursorIsLocked = false;
		else if (Input.GetMouseButtonUp(0))
			cursorIsLocked = true;

		if(cursorIsLocked)
        {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
        }
		else if (!cursorIsLocked)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}
}
