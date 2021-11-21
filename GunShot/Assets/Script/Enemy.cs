using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    public Transform target;
    public BoxCollider attackArea;
    public bool isChase;
    public bool isAttack;

    Rigidbody rig;
    SkinnedMeshRenderer mat;
    NavMeshAgent nav;
    Animator ani;

    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();

        Invoke("Chase", 2);
    }

    void Chase()
    {
        isChase = true;
        ani.SetBool("isRun", true);
    }

    void Update()
    {
        this.transform.LookAt(target);

        if (nav.enabled)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
    }

    void FixedUpdate()
    {
        Target();
        if (isChase)
        {
            rig.velocity = Vector3.zero;
            rig.angularVelocity = Vector3.zero;
        }
    }

    void Target()
    {
        float radius = 1f;
        float range = 12f;

        RaycastHit[] Hit = Physics.SphereCastAll(transform.position, radius,
                                                transform.forward, range, LayerMask.GetMask("Player"));
    
        if (Hit.Length > 0 && !isAttack)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        ani.SetBool("isAttack", true);

        yield return new WaitForSeconds(0.1f);
        rig.AddForce(transform.forward * 20, ForceMode.Impulse);
        attackArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        rig.velocity = Vector3.zero;
        attackArea.enabled = false;

        yield return new WaitForSeconds(2f);

        isChase = true;
        isAttack = false;
        ani.SetBool("isAttack", false);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Bullet bull = other.GetComponent<Bullet>();
            currentHP -= bull.demage;
            Destroy(other.gameObject);

            StartCoroutine(OnDamage());
        }
    }

    IEnumerator OnDamage()
    {
        mat.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if (currentHP > 0)
        {
            mat.material.color = Color.white;
        }
        else
        {
            isChase = false;
            nav.enabled = false;
            ani.SetTrigger("doDie");
            Destroy(gameObject, 1);
        }
    }
}
