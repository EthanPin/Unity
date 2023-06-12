using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    private float yaw = 0.0f, pitch = 0.0f;
    private Rigidbody rb;

    //[SerializeField]
    public float walkSpeed = 5.0f, sensitivity = 2.0f, dashSpeed = 5.0f, dashTime = 2.0f;
    bool isDashing;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Physics.Raycast(rb.transform.position, Vector3.down, 1 + 0.001f))    //Jump
            rb.velocity = new Vector3(rb.velocity.x, 5.0f, rb.velocity.z);
        if (Input.GetKey(KeyCode.LeftControl))    //Float
            rb.velocity = new Vector3(rb.velocity.x, 20.0f, rb.velocity.z);
        if (Input.GetKey(KeyCode.LeftShift))
            isDashing = true;
            //rb.velocity = new Vector3(-Camera.main.transform.right.z, 0.0f, Camera.main.transform.right.x);
            //rb.AddForce(transform.up * 20.0f, ForceMode.Impulse);
        Look();
    }

    //must be called more than once each frame so movement stays consistant
    private void FixedUpdate()  
    {
        Movement();
        if (isDashing) Dashing();
    }

    void Look()
    {
        pitch -= Input.GetAxisRaw("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, -90.0f, 90.0f);
        yaw += Input.GetAxisRaw("Mouse X") * sensitivity;
        Camera.main.transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
    }

    void Movement()
    {
        Vector2 axis = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * walkSpeed;
        Vector3 forward = new Vector3(-Camera.main.transform.right.z, 0.0f, Camera.main.transform.right.x);
        Vector3 wishDirection = (forward * axis.x + Camera.main.transform.right * axis.y + Vector3.up * rb.velocity.y);
        rb.velocity = wishDirection;
    }

    void Dashing()
    {
        Vector2 axis = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * walkSpeed;
        Vector3 forward = new Vector3(-Camera.main.transform.right.z, 0.0f, Camera.main.transform.right.x);
        //Vector3 wishDirection = (forward * axis.x + Camera.main.transform.right * axis.y + Vector3.up * rb.velocity.y);
        //rb.velocity = wishDirection;
        rb.AddForce(forward * dashSpeed, ForceMode.Impulse);
        isDashing = false;
    }

}
