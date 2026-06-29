using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class EnemyScript : MonoBehaviour
{
    private float hp = 100;
    protected float speed = 5f;
    protected float force = 2f;
    protected float seeRange = 20f;
    private float jumpRange = 0.5f;
    private float attackRange = 1.2f;
    protected GameObject player;
    protected bool isSeeing = false;
    protected Animator anim;
    protected Rigidbody rb;
    public bool canWalk = false;
    private bool isHitted;
    private bool isNear;
    public bool isDead = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        isItSaw();
        if (isSeeing)
        {
            Chasing();
        }
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        isHitted = Physics.Raycast(ray, out hit, jumpRange);
        if (isHitted)
        {
            Debug.DrawRay(transform.position, transform.forward * jumpRange, Color.yellow);
            if (!hit.collider.CompareTag("Player"))
            {
                Jump();
            }
        }
        if(hp <= 0)
        {
            anim.SetTrigger("Dead");
        }
        if(isDead)
        {
            Destroy(gameObject);
        }
    }

    public  void isItSaw()
    {
        if (Physics.OverlapSphere(transform.position, seeRange).Contains(player.GetComponent<BoxCollider>()))
        {
            isSeeing = true;
            anim.SetBool("SawPlayer",true);
        }
        else
        {
            isSeeing = false;
            anim.SetBool("SawPlayer",false);
        }
    }

    public void Jump()
    {
        rb.AddForce(transform.up * force, ForceMode.Impulse);
    }
    public virtual void Hitting()
    {
        anim.SetTrigger("Attack");
        anim.SetFloat("AttackVersion", 0);
    }

    public virtual void Chasing()
    {
        transform.LookAt(player.transform.position);
        if (canWalk && !isNear)
        {
            gameObject.transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z), speed * Time.deltaTime);

        }
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, attackRange))
        {
            Debug.DrawRay(transform.position, transform.forward * attackRange, Color.red);
            if(hit.collider.name == "EnemyStopRadius")
            {
                Hitting();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyStopRadius"))
        {
            isNear = true;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage();
        }
        if (other.gameObject.CompareTag("Weapon"))
        {
            TakeDamage();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyStopRadius"))
        {
            isNear = false;
        }
    }
    public void TakeDamage()
    {
        hp -= 20;
        anim.SetTrigger("Hurt");
    }
}
