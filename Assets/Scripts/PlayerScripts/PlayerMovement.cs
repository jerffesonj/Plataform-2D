using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Values")]
    [SerializeField] private float movementAcceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float groundLinearDrag;
    [SerializeField] private float airLinearDrag = 2.5f;

    [Header("Jump Values")]
    [SerializeField] private float jumpForce;
    [SerializeField] private int extraJumps = 1;
    [SerializeField] private int extraJumpsCounter = 1;
    [SerializeField] private float hangTime = 0.1f;
    [SerializeField] private float jumpBuffer = 0.1f;
    
    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector3 groundCheckSize;
    [SerializeField] private Transform groundCheckCollision;

    [Header("Ground Damage")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform groundDamage;
    [SerializeField] private Vector3 groundDamageSize;

    [Header("Gravity Values")]
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private float gravity = 1;

    [Header("Corner Correction Variables")]
    [SerializeField] private LayerMask _cornerCorrectLayer;
    [SerializeField] private float _topRaycastLength;
    [SerializeField] private Vector3 _edgeRaycastOffset;
    [SerializeField] private Vector3 _innerRaycastOffset;
    [SerializeField] private bool _canCornerCorrect;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private Hp hp;
    private float horizontalValue = 0;
    private bool isGrounded = false;

    private float hangTimeCounter = 0.1f;
    private float jumpBufferCounter = 0.1f;

    private bool canJump => jumpBufferCounter > 0 && (hangTimeCounter > 0 || extraJumpsCounter > 0);
    public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
    public float MovementAcceleration { get => movementAcceleration; set => movementAcceleration = value; }
    public float FallMultiplier { get => fallMultiplier; set => fallMultiplier = value; }
    public float JumpMultiplier { get => jumpMultiplier; set => jumpMultiplier = value; }
    public float Gravity { get => gravity; set => gravity = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hp = GetComponent<Hp>();
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalValue = GetInput().x;
        
        anim.SetBool("Walking", horizontalValue != 0);

        if (rb.velocity.x != 0)
        {
            if (horizontalValue < 0)
            {
                sprite.flipX = false;
            }
            else if (horizontalValue > 0)
            {
                sprite.flipX = true;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBuffer;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        if (canJump)
        {
            StartCoroutine(JumpCounter());
            Jump();
        }

    }
    IEnumerator JumpCounter()
    {
        yield return new WaitForSeconds(0.1f);
        extraJumpsCounter--;
    }


    private void FixedUpdate()
    {
        Move();

        GroundCheck();
        GroundDamage();
        if (isGrounded)
        {
            hangTimeCounter = hangTime;
            extraJumpsCounter = extraJumps;
            ApplyGroundLinearDrag();
        }
        else
        {
            hangTimeCounter -= Time.deltaTime;
            ApplyAirLinearDrag();
            ApplyFallMultiplier();
        }
        if (_canCornerCorrect) CornerCorrect(rb.velocity.y);

        _canCornerCorrect = Physics2D.Raycast(transform.position + _edgeRaycastOffset, Vector2.up, _topRaycastLength, _cornerCorrectLayer) &&
                        !Physics2D.Raycast(transform.position + _innerRaycastOffset, Vector2.up, _topRaycastLength, _cornerCorrectLayer) ||
                        Physics2D.Raycast(transform.position - _edgeRaycastOffset, Vector2.up, _topRaycastLength, _cornerCorrectLayer) &&
                        !Physics2D.Raycast(transform.position - _innerRaycastOffset, Vector2.up, _topRaycastLength, _cornerCorrectLayer);

    }
    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
    private void Move()
    {
        rb.AddForce(new Vector2(horizontalValue * movementAcceleration, 0));
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
    }

    void ApplyGroundLinearDrag()
    {
        if (MathF.Abs(horizontalValue) < 0.4f)
        {
            rb.drag = groundLinearDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
    void ApplyAirLinearDrag()
    {
        rb.drag = airLinearDrag;
    }

    void ApplyFallMultiplier()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallMultiplier;
        }
        else if (rb.velocity.y > 0 && !Input.GetKeyDown(KeyCode.Space))
        {
            rb.gravityScale = jumpMultiplier;
        }
        else
        {
            rb.gravityScale = gravity;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpBufferCounter = 0;
        hangTimeCounter = 0;
    }

    private void GroundDamage()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(groundDamage.transform.position, groundDamageSize, enemyLayer);

        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.CompareTag("Enemy"))
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                    EnemyHp enemyHp = colliders[i].GetComponent<EnemyHp>();
                    if (enemyHp.AlwaysInvunerable)
                    {
                        hp.RemoveHP(enemyHp.Damage);
                    }
                    else
                    {
                        colliders[i].gameObject.GetComponent<EnemyHp>().RemoveHP(hp.Damage);
                    }
                    
                }
            }
        }
    }
    void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(groundCheckCollision.transform.position, groundCheckSize, groundLayer);

        List<Collider2D> collidersList = colliders.ToList();

        for (int i = 0; i < collidersList.Count; i++)
        {
            if (colliders[i].gameObject.layer != groundLayer)
            {
                collidersList.Remove(colliders[i]);
            }
        }

        if (collidersList.Count > 0)
        {
            isGrounded = true;
        }
    }
    void CornerCorrect(float Yvelocity)
    {
        //Push player to the right
        RaycastHit2D _hit = Physics2D.Raycast(transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength, Vector3.left, _topRaycastLength);
        if (_hit.collider != null)
        {
            float _newPos = Vector3.Distance(new Vector3(_hit.point.x, transform.position.y, 0f) + Vector3.up * _topRaycastLength,
                transform.position - _edgeRaycastOffset + Vector3.up * _topRaycastLength);
            transform.position = new Vector3(transform.position.x + _newPos, transform.position.y, transform.position.z);
            rb.velocity = new Vector2(rb.velocity.x, Yvelocity);
            return;
        }

        //Push player to the left
        _hit = Physics2D.Raycast(transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength, Vector3.right, _topRaycastLength);
        if (_hit.collider != null)
        {
            float _newPos = Vector3.Distance(new Vector3(_hit.point.x, transform.position.y, 0f) + Vector3.up * _topRaycastLength,
                transform.position + _edgeRaycastOffset + Vector3.up * _topRaycastLength);
            transform.position = new Vector3(transform.position.x - _newPos, transform.position.y, transform.position.z);
            rb.velocity = new Vector2(rb.velocity.x, Yvelocity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(groundCheckCollision.transform.position, groundCheckSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundDamage.transform.position, groundDamageSize);

        //Corner Check
        Gizmos.DrawLine(transform.position + _edgeRaycastOffset, transform.position + _edgeRaycastOffset + Vector3.up * _topRaycastLength);
        Gizmos.DrawLine(transform.position - _edgeRaycastOffset, transform.position - _edgeRaycastOffset + Vector3.up * _topRaycastLength);
        Gizmos.DrawLine(transform.position + _innerRaycastOffset, transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength);
        Gizmos.DrawLine(transform.position - _innerRaycastOffset, transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength);
        //Corner Distance Check
        Gizmos.DrawLine(transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength,
                        transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength + Vector3.left * _topRaycastLength);
        Gizmos.DrawLine(transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength,
                        transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength + Vector3.right * _topRaycastLength);
    }
}
