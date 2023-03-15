using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : Hp
{
    [SerializeField] private bool isProtected;
    [SerializeField] private GameObject diamond;

    public bool IsProtected { get => isProtected; set => isProtected = value; }

    void Start()
    {
        base.Start();
    }

    public override void RemoveHP(int damage)
    {
        if (invulnerable)
            return;
        if (isProtected)
            return;
        base.RemoveHP(damage);
        if (currentHp <= 0)
        {
            GameObject diamondClone = Instantiate(diamond);
            diamondClone.transform.position = this.transform.position;
            this.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!this.isProtected)
            {
                Hp hp = collision.gameObject.GetComponent<Hp>();
                hp.RemoveHP(damage);
            }
        }
    }
}
