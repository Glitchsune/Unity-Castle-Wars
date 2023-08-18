using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed;
    public float jumpForce;

    private bool isFacingRight = true;
    private float horizontalMove = 0;
    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode jump;
    public KeyCode roll;

    private Rigidbody2D theRB;
    private Animator animator;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = 0f;
        if (Input.GetKey(moveLeft))
        {
            horizontalMove += -1f;
        }
        if (Input.GetKey(moveRight))
        {
            horizontalMove += 1f;
        }
        Flip();
    }

    private void FixedUpdate()
    {
        theRB.velocity = new Vector2(horizontalMove * moveSpeed, theRB.velocity.y);
    }

    private void Flip()
    {
        if(isFacingRight && horizontalMove < 0f || !isFacingRight && horizontalMove > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
