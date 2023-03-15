using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp : MonoBehaviour
{
    [SerializeField] protected int currentHp;
    [SerializeField] protected int maxHp;

    [SerializeField] protected int damage;

    [SerializeField] protected List<Image> hearts;
    [SerializeField] protected Sprite heartFull;
    [SerializeField] protected Sprite heartEmpty;
    [SerializeField] protected Sprite heartHalf;
    [SerializeField] protected GameObject canvas;

    [SerializeField] protected bool invulnerable;
    [SerializeField] protected bool alwaysInvunerable;

    [SerializeField] protected SpriteRenderer spriteRenderer;
    public int Damage { get => damage; }
    public bool AlwaysInvunerable { get => alwaysInvunerable; set => alwaysInvunerable = value; }

    // Start is called before the first frame update
    protected void Start()
    {
        currentHp = maxHp;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public virtual void RemoveHP(int damage)
    {
        if (invulnerable)
            return;
        
        currentHp -= damage;
        if (currentHp <= 0)
            currentHp = 0;
        StartCoroutine(DamageAnim(spriteRenderer));
        if (!alwaysInvunerable)
            StartCoroutine(InvulnerableRoutine());

        AttHeart();
    }

    public virtual void AddHP(int value)
    {
        currentHp += value;
        if(currentHp >= maxHp)
        {
            currentHp = maxHp;
        }
        AttHeart();
    }

    protected IEnumerator DamageAnim(SpriteRenderer spriteRenderer)
    {
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator InvulnerableRoutine()
    {
        invulnerable = true;
        yield return new WaitForSeconds(0.2f);
        invulnerable = false;
    }

    protected void AttHeart()
    {
        StartCoroutine(CanvasShowUp());
        for (int i = hearts.Count-1; i >= 0; i--)
        {
            int currentHpCheck = currentHp - (i * 2);
            if (currentHpCheck <= 0)
                currentHpCheck = 0;
            if (currentHpCheck >= 2)
                currentHpCheck = 2;

            switch (currentHpCheck)
            {
                case 0:
                    hearts[i].sprite = heartEmpty;
                    break;
                case 1:
                    hearts[i].sprite = heartHalf;
                    break;
                case 2:
                    hearts[i].sprite = heartFull;
                    break;
            }
        }
    }

    IEnumerator CanvasShowUp()
    {
        canvas.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        canvas.SetActive(false);

    }
}
