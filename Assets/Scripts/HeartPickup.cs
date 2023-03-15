using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] private int healValue;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Hp hp = collision.gameObject.GetComponent<Hp>();
            hp.AddHP(healValue);
            this.gameObject.SetActive(false);
        }
    }
}
