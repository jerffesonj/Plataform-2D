using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private CanvasScript canvas;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(EndEnum(collision));
        }
    }

    IEnumerator EndEnum(Collider2D collision)
    {
        PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();  
        Animator animator = collision.gameObject.transform.GetComponentInChildren<Animator>();
        SpriteRenderer sprite = collision.gameObject.transform.GetComponentInChildren<SpriteRenderer>();  

        playerMovement.enabled = false;
        animator.SetBool("Walking", true);
        rb.velocity = new Vector2(5,0);
        sprite.flipX = true;
        yield return new WaitForSeconds(1f);
        rb.velocity = new Vector2(0, 0);
        animator.SetBool("Walking", false);
        canvas.ShowEnd();

    }
}
