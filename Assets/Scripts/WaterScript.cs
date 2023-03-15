using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    private float currentPlayerSpeed;
    private float currentPlayerMaxSpeed;
    private float currentPlayerFall;
    private float currentPlayerJump;
    private float speedMultiplier;
    private float jumpMultiplier;
    private float fallMultiplier;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            PlayerMovement pm = rb.GetComponent<PlayerMovement>();

            currentPlayerSpeed = pm.MaxSpeed;
            currentPlayerFall = pm.FallMultiplier;
            currentPlayerJump = pm.JumpMultiplier;
            currentPlayerMaxSpeed = pm.MaxSpeed;

            pm.MovementAcceleration = currentPlayerSpeed * speedMultiplier;
            pm.FallMultiplier = fallMultiplier;
            pm.JumpMultiplier = jumpMultiplier;
            pm.MaxSpeed = currentPlayerMaxSpeed * speedMultiplier;
            rb.velocity = new Vector2(rb.velocity.x /2, 0);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            PlayerMovement pm = rb.GetComponent<PlayerMovement>();

            pm.MovementAcceleration = currentPlayerSpeed * speedMultiplier;
            pm.FallMultiplier = fallMultiplier;
            pm.JumpMultiplier = jumpMultiplier;
            pm.MaxSpeed = currentPlayerMaxSpeed * speedMultiplier;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            PlayerMovement pm = rb.GetComponent<PlayerMovement>();
            pm.MovementAcceleration = currentPlayerSpeed;
            pm.JumpMultiplier = currentPlayerJump;
            pm.FallMultiplier = currentPlayerFall;
            pm.MaxSpeed = currentPlayerMaxSpeed;
            pm.Gravity = 1;
        }
    }
}
