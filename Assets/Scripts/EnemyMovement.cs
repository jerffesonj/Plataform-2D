using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int direction;
    [SerializeField] private SpriteRenderer sprite;

    [SerializeField] private Transform leftRaycast;
    [SerializeField] private Transform rightRaycast;
    [SerializeField] private Transform frontRaycast;
    [SerializeField] private Transform backRaycast;
    [SerializeField] private float rayDistance;
    [SerializeField] private float rayWallDistance;

    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    EnemyHp enemyHp;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHp = GetComponent<EnemyHp>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.x != 0)
        {
            if (direction < 0)
            {
                sprite.flipX = false;
            }
            else if (direction > 0)
            {
                sprite.flipX = true;
            }
        }

        CheckFloorFall();
        CheckWall();
    }

    private void CheckWall()
    {
        RaycastHit2D hit;
        int value = 0;
        if (direction < 0)
        {
            hit = Physics2D.Raycast(frontRaycast.position, Vector2.left, rayWallDistance, groundLayer);
            Debug.DrawRay(leftRaycast.position, Vector2.left * rayWallDistance, Color.red);

            if (hit.collider != null)
            {
                value += 1;
            }
        }
        else if (direction > 0)
        {
            hit = Physics2D.Raycast(backRaycast.position, Vector2.right, rayWallDistance, groundLayer);
            Debug.DrawRay(rightRaycast.position, Vector2.right * rayWallDistance, Color.red);

            if (hit.collider != null)
            {
                value += 1;

            }
        }
        if (value != 0)
        {
            ChangeDirection();
            StartCoroutine(Protect());
        }
    }

    private void FixedUpdate()
    {
        Move(speed * direction);
    }
    public Animator animator;

    IEnumerator Protect()
    {
        if(enemyHp.IsProtected)
        {
            yield break;
        }
        speed = 0;
        animator.SetBool("Protected", true);
        enemyHp.IsProtected = true;
        yield return new WaitForSeconds(3);
        enemyHp.IsProtected = false;
        animator.SetBool("Protected", false);
        speed = 3;

    }

    private void CheckFloorFall()
    {
        RaycastHit2D hit;
        int value = 0;
        if (direction < 0)
        {
            hit = Physics2D.Raycast(leftRaycast.position, Vector2.down, rayDistance, groundLayer);
            Debug.DrawRay(leftRaycast.position, Vector2.down * rayDistance, Color.red);

            if (hit.collider == null)
            {
                value += 1;
            }
        }
        else if (direction > 0)
        {
            hit = Physics2D.Raycast(rightRaycast.position, Vector2.down, rayDistance, groundLayer);
            Debug.DrawRay(rightRaycast.position, Vector2.down * rayDistance, Color.red);

            if (hit.collider == null)
            {
                value += 1;

            }
        }
        if (value != 0)
        {
            ChangeDirection();
            StartCoroutine(Protect());
        }
    }

    void ChangeDirection()
    {
        direction *= -1;
    }

    private void Move(float dir)
    {
        float xValue = dir * speed * Time.deltaTime * 10;
        Vector2 targetVelocity = new Vector2(xValue, rb.velocity.y);
        rb.velocity = targetVelocity;
    }
}
