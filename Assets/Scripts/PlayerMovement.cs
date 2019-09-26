using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public float jumpForce;

    private float baseGravity = 3;

    Rigidbody2D rb;

    #region Jumping
    bool isGrounded;
    CircleCollider2D playerCircleCollider;
    public LayerMask groundLayer;
    public int extraJumps = 0;

    private int jumpsAvalible;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCircleCollider = GetComponent<CircleCollider2D>();

        rb.gravityScale = baseGravity;
    }

    // Update is called once per frame
    void Update()
    {
        if(isGrounded)
        {
            jumpsAvalible = (1 + extraJumps);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(jumpsAvalible > 1 || (jumpsAvalible == 1 && isGrounded))
            {
                rb.velocity = Vector2.up * jumpForce;
                isGrounded = false;
                jumpsAvalible--;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground") isGrounded = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Kill") Debug.Log("YOU DIE!");

        if (col.gameObject.tag == "Gem")
        {
            Player.instance.GiveGems(col.gameObject.GetComponent<Gem>().gemValue);
            Destroy(col.transform.parent.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ladder")
        {
            rb.gravityScale = 0;
            Debug.Log("Inside ladder");
            if (Input.GetKey(KeyCode.W))
            {
                rb.velocity = new Vector2(rb.velocity.x, speed);
            }
            else if(Input.GetKey(KeyCode.S))
            {
                rb.velocity = new Vector2(rb.velocity.x, -speed);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ladder")
            rb.gravityScale = baseGravity;
    }

    private void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(speed * move, rb.velocity.y);

        FlipHandling(move);
    }

    private void FlipHandling(float move)
    {
        if (move < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (move > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);

        }
    }
}
