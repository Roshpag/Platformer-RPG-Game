using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyScript : MonoBehaviour
{
    protected float speed = 5f;
    protected float force = 5f;
    protected float seeRange = 10f;
    protected float jumpRange = 1.6f;
    protected float attackRange = 2f;
    protected Collider player;
    protected bool isSeeing = false;
    protected Animator anim;
    protected Rigidbody rb;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<CapsuleCollider>();
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        isItSaw();

        
    }

    public void isItSaw()
    {
        if (Physics.OverlapSphere(transform.position, seeRange).Contains(player))
        {
            isSeeing = true;
            anim.SetTrigger("SawPlayer");
        }
        else
        {
            isSeeing = false;
            anim.SetTrigger("Hide");
        }
    }

    public void Jump()
    {
        Ray ray = new Ray(transform.position,transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit,0.25f))
        {
            rb.AddForce(transform.up * force,ForceMode.Impulse);
        }
    }
}
