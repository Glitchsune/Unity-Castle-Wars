using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed;
    public float jumpPower;

    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode jump;
    public KeyCode roll;

    private Rigidbody2D theRB;
    private Animator animator;

    private float horizontalMove = 0f;

    private bool canRoll = true;
    private bool isRolling;
    private float rollPower = 8f;
    private float rollTime = 0.3f;
    private float rollCoolDown = 1f;

    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRolling)
        {
            return;
        }

        horizontalMove = 0f;
        if (Input.GetKey(moveLeft))
        {
            horizontalMove += -1f;
        }
        if (Input.GetKey(moveRight))
        {
            horizontalMove += 1f;
        }

        if (Input.GetKeyDown(jump) && IsGrounded())
        {
            theRB.velocity = new Vector2(theRB.velocity.x, jumpPower);
        }
        if (Input.GetKeyUp(jump) && theRB.velocity.y > 0f)
        {
            theRB.velocity = new Vector2(theRB.velocity.x, theRB.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown(roll) && canRoll)
        {
            StartCoroutine(Roll());
        }

        UpdateAnimationUpdate();

        Flip();
    }

    private void FixedUpdate()
    {
        if (isRolling)
        {
            return;
        }
        theRB.velocity = new Vector2(horizontalMove * moveSpeed, theRB.velocity.y);
    }


    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private IEnumerator Roll()
    {
        canRoll = false;
        isRolling = true;
        float originalGravity = theRB.gravityScale;
        theRB.gravityScale = 0f;
        theRB.velocity = new Vector2(transform.localScale.x * rollPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(rollTime);
        tr.emitting = false;
        theRB.gravityScale = originalGravity;
        isRolling = false;
        yield return new WaitForSeconds(rollCoolDown);
        canRoll = true;
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

    private void UpdateAnimationUpdate()
    {
        if (horizontalMove != 0)
        {
            animator.SetBool("running", true);
        }
        else
        {
            animator.SetBool("running", false);
        }

        if (isRolling)
        {
            animator.SetBool("rolling", true);
        }
        else
        {
            animator.SetBool("rolling", false);
        }
    }
}
