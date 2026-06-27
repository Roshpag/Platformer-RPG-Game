using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float speed = 7;
    public float force = 5;
    private Rigidbody rb;
    private Animator anim;
    public Transform cameraTransform;
    public Vector3 camForward = Vector3.forward;
    public Vector3 camRight = Vector3.right;
    private bool isRunning = false;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Jump");

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
        rb.AddForce(move * speed, ForceMode.VelocityChange);
    }
}
