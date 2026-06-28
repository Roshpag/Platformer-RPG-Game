using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    public float speed = 7;
    public float runSpeed = 11;
    public float force = 5;
    private Rigidbody rb;
    private Animator anim;
    public Transform cameraTransform;
    public Vector3 camForward = Vector3.forward;
    public Vector3 camRight = Vector3.right;
    private bool isRunning = false;
    private bool isGrounded = true;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (cameraTransform != null)
        {
            camForward = cameraTransform.forward;
            camRight = cameraTransform.right;
        }
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, cameraTransform.eulerAngles.y, transform.eulerAngles.z);
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if((horizontal != 0 || vertical != 0))
        {
            anim.SetInteger("Move", 1);
            if (Input.GetKey(KeyCode.W))
            {
                anim.SetFloat("Direction", 2);
            }
            if (Input.GetKey(KeyCode.S))
            {
                anim.SetFloat("Direction", 0);
            }
            if (Input.GetKey(KeyCode.A))
            {
                anim.SetFloat("Direction", 3);
            }
            if (Input.GetKey(KeyCode.D))
            {
                anim.SetFloat("Direction", 1);
            }
            
        }
        if (vertical > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
        if (isRunning)
        {
            anim.SetInteger("Move", 2);
        }
        if (horizontal == 0 && vertical == 0)
        {
            anim.SetInteger("Move", 0);
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            anim.SetTrigger("Jump");

            Vector3 v = rb.linearVelocity;
            v.y = force;
            rb.linearVelocity = v;
            isGrounded = false;

            if(Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetFloat("JumpVersion", 1);
            }
            else
            {
                anim.SetFloat("JumpVersion", 0);
            }
        }
        if(Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
            if(vertical > 0 && Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetFloat("AttackVersion", 1);
            }
            else
            {
                anim.SetFloat("AttackVersion", 0);
            }
        }
    }
    private void FixedUpdate()
    {
        Vector3 move = (camRight * horizontal + camForward * vertical).normalized;
        Vector3 velocity = move * (isRunning ? runSpeed : speed);
        velocity.y = rb.linearVelocity.y; 
        rb.linearVelocity = velocity;
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                break;
            }
        }
    }
}
