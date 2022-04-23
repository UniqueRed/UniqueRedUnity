using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public Transform target;
    public Transform groundCheck;

    public float speed = 4f;
    public float maxDist = 1f;
    public float jumpForce = 50f;
    public float groundDistance = 0.4f;
    public float chaseRadius = 10f;

    public int attackRandomizer;
    
    public bool isGrounded;

    public Rigidbody rb;

    public LayerMask obstacle;
    public LayerMask player;
    public LayerMask ground;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerCheck();
        GroundCheck();
    }

    public void Movement()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
        rb.MovePosition(pos);

        Vector3 targetPostition = new Vector3(target.position.x, this.transform.position.y, target.position.z);
        this.transform.LookAt(targetPostition);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDist, obstacle))
        {
            rb.AddForce(transform.up * jumpForce);
        } 
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, ground);
    }
    
    void PlayerCheck()
    {
        if(Physics.CheckSphere(transform.position, chaseRadius, player))
        {
            Movement();
        }
    }
}
