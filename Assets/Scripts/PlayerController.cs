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
    private float speed = 7;
    public float force = 5;
    private Rigidbody rb;
    private Animator anim;
    public Transform cameraTransform;
    public Vector3 camForward = Vector3.forward;
    public Vector3 camRight = Vector3.right;
    private bool isRunning = false;
    private int hp = 3;
    public List<Image> hpImagesOn;
    private List<Image> hpImagesOff;
    public Slider staminaSlider;
    private int stamina = 100;
    private bool canRun = true;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        /*GameObject[] hpObj = GameObject.FindGameObjectsWithTag("HeartHP");
        for (int i = 0; i < hpObj.Length; i++)
        {
            hpImagesOn.Add(hpObj[i].GetComponent<Image>());
        }*/
    }

    // Update is called once per frame
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Jump");

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
        if(hp > hpImagesOn.Count)
        {
            while(hp > hpImagesOn.Count)
            {
                hpImagesOn.Add(hpImagesOff[^1]);
                hpImagesOff.Remove(hpImagesOff[^1]);
                hpImagesOn[^1].color = new Color(hpImagesOn[^1].color.r, hpImagesOn[^1].color.g, hpImagesOn[^1].color.b, 1f);
            }
        }
        if(hp <  hpImagesOn.Count)
        {
            hpImagesOff.Add(hpImagesOn[^1]);
            hpImagesOn.Remove(hpImagesOn[^1]);
            hpImagesOff[^1].color = new Color(hpImagesOff[^1].color.r, hpImagesOff[^1].color.g, hpImagesOff[^1].color.b, 0.5f);
        }
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
        hp -= 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Input.GetMouseButton(0) && other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyScript>().TakeDamage();
        }
    }
}
