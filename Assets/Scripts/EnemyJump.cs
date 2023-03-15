using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float jumpForce;

    [SerializeField] private float totalTimeAlive;
    private float timeAlive;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.y > 0)
        {
            spriteRenderer.flipY = false;
        }
        else
        {
            spriteRenderer.flipY = true;
        }

        timeAlive += Time.deltaTime;
        if(timeAlive > totalTimeAlive)
        {
            timeAlive = 0;
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        if (!rb)
            return;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
