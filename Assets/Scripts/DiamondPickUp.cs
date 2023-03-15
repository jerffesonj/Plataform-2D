using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerPoints playerPoints = collision.gameObject.GetComponent<PlayerPoints>();
            playerPoints.AddPoints();
            this.gameObject.SetActive(false);
        }
    }
}
