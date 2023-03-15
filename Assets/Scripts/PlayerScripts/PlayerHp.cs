using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : Hp
{
    [SerializeField] private float damageForce;
   
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void RemoveHP(int damage)
    {
        base.RemoveHP(damage);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!collision.gameObject.GetComponent<EnemyHp>().IsProtected)
            {
                if (collision.transform.position.x > this.transform.position.x)
                    rb.AddForce(Vector2.left * damageForce);
                else
                    rb.AddForce(Vector2.right * damageForce);
            }
        }
        if (collision.gameObject.CompareTag("Spikes"))
        {
            if (collision.transform.position.x > this.transform.position.x)
                rb.AddForce(Vector2.one * damageForce);
            else
                rb.AddForce(Vector2.one * damageForce);
        }
    }
}
