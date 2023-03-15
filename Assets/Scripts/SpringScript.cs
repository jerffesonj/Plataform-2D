using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringScript : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> spriteList;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ChangeSprite());
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.up * force);
        }
    }

    private IEnumerator ChangeSprite()
    {
        spriteRenderer.sprite = spriteList[1];
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.sprite = spriteList[0];
    }
}
