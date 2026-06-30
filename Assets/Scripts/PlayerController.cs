using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float speed = 3;
    private float runSpeed = 5;
    public float force = 5;
    private Rigidbody rb;
    private Animator anim;
    private Transform cameraTransform;
    public Vector3 camForward = Vector3.forward;
    public Vector3 camRight = Vector3.right;
    private bool isRunning = false;
    private bool isGrounded = true;
    private int hp = 100;
    private Slider hpSlider;
    private Slider staminaSlider;
    private int stamina = 100;
    private bool canRun = true;
    private GameObject gameManager;
    void Start()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        hpSlider = GameObject.FindGameObjectWithTag("HPSlider").GetComponent<Slider>();
        staminaSlider = GameObject.FindGameObjectWithTag("StaminaSlider").GetComponent<Slider>();
        rb = gameObject.GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        /*GameObject[] hpObj = GameObject.FindGameObjectsWithTag("HeartHP");
        for (int i = 0; i < hpObj.Length; i++)
        {
            hpImagesOn.Add(hpObj[i].GetComponent<Image>());
        }*/
        gameManager = GameObject.Find("GameManager");
    }

    void Update()
    {
        Move();
        Jump();
        Attack();
        Stamina();
        HP();
    }
    private void FixedUpdate()
    {
        Vector3 move = (camRight * horizontal + camForward * vertical).normalized;
        rb.AddForce(move * speed, ForceMode.VelocityChange);
        Vector3 velocity = move * (isRunning ? runSpeed : speed);
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
    }

    public void Move()
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

        if ((horizontal != 0 || vertical != 0))
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
        if (vertical > 0 && Input.GetKey(KeyCode.LeftShift) && canRun)
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
    }
    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            anim.SetTrigger("Jump");
            isGrounded = false;
            Vector3 v = rb.linearVelocity;
            v.y = force;
            rb.linearVelocity = v;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetFloat("JumpVersion", 1);
            }
            else
            {
                anim.SetFloat("JumpVersion", 0);
            }
        }
    }
    public void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
            if (vertical > 0 && Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetFloat("AttackVersion", 1);
            }
            else
            {
                anim.SetFloat("AttackVersion", 0);
            }
        }
    }
    public void Stamina()
    {
        staminaSlider.value = stamina;
        if (stamina > 0)
        {
            canRun = true;
        }
        else
        {
            canRun = false;
        }

        if (vertical > 0 && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(StaminaLowing());
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StartCoroutine(StaminaReCharge());
        }
    }
    public void HP()
    {
        if(hp <= 0)
        {
            hp = 0;
            anim.SetTrigger("Dead");
            gameManager.GetComponent<GameManager>().Restart();
        }
        hpSlider.value = hp;
    }
    IEnumerator StaminaReCharge()
    {
        while(stamina < 100)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                break;
            }
            stamina++;
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator StaminaLowing()
    {
        while (stamina > 0)
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                break;
            }
            stamina--;
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void TakeDamage()
    {
        if(hp > 10)
        {
            anim.SetTrigger("Hurt");
        }
        hp -= 10;
    }

    private void OnTriggerEnter(Collider other)
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
