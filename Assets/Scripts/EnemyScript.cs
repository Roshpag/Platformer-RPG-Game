using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class EnemyScript : MonoBehaviour
{
    protected float hp = 100;
    protected float speed = 5f;
    protected float force = 2f;
    protected float seeRange = 20f;
    public float jumpRange = 1.6f;
    public float attackRange = 2f;
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
        player = GameObject.Find("Player");
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
        transform.LookAt(player.transform.position);
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
        RaycastHit damageCollider;
        if(Physics.BoxCast(transform.position,new Vector3(1f,0.5f,2.5f),transform.forward * attackRange,out damageCollider))
        {
            if (damageCollider.collider.name == "Player")
            {
                Debug.Log("Damage!");
                //damageCollider.collider.gameObject.GetComponent<PlayerController>().TakeDamage();
            }
        }
    }

    public virtual void Chasing()
    {
        if (canWalk && !isNear)
        {
            gameObject.transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z), speed * Time.deltaTime);

        }
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, attackRange))
        {
            Debug.DrawRay(transform.position, transform.forward * attackRange, Color.red);
            if(hit.collider.name == "Player")
            {
                Hitting();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isNear = false;
        }
    }
    public void TakeDamage()
    {
        hp -= 20;
    }
}
