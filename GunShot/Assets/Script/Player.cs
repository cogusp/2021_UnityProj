using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float rate;
    public int maxBull;
    public int currentBull;
    float hAxis;
    float vAxis;
    float fireDelay;
    bool jDown;
    bool fDown;
    bool rDown;
    bool isJump;
    bool isFire = true;
    bool isReload;

    public GameObject bullet;
    public Transform bulletPos;

    Vector3 moveVec;

    Rigidbody rig;
    Animator ani;
    
    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        ani = GetComponentInChildren<Animator>();
    }
    
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Fire();
        Reload();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        jDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButtonDown("Fire1");
        rDown = Input.GetButtonDown("Reload");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isReload || !isFire)
            moveVec = Vector3.zero;

        transform.position += moveVec * speed * Time.deltaTime;

        ani.SetBool("isRun", moveVec != Vector3.zero);
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        if (jDown && !isJump)
        {
            rig.AddForce(Vector3.up * 7, ForceMode.Impulse);
            ani.SetBool("isJump",true);
            isJump = true;
        }
    }

    void Fire()
    {
        fireDelay += Time.deltaTime;
        isFire = rate < fireDelay;

        if (fDown && currentBull > 0)
        {
            currentBull--;
            GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
            Rigidbody bulletRig = intantBullet.GetComponent<Rigidbody>();
            bulletRig.velocity = bulletPos.forward * 50;
            
            ani.SetTrigger("doShot");
            fireDelay = 0;
            isFire = true;
        }
    }

    void Reload()
    {
        if (rDown && !isJump && isFire)
        {
            ani.SetTrigger("doReload");
            isReload = true;

            Invoke("ReloadOut", 3.0f);
        }
    }

    void ReloadOut()
    {
        currentBull = maxBull;
        isReload = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Floor")
        {
            ani.SetBool("isJump", false);
            isJump = false;
        }
    }
}
