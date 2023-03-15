using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PassThroughPlataform : MonoBehaviour
{
    [SerializeField] private float timePlataformRotation = 0.5f;
    [SerializeField] private float timeLayer = 0.5f;

    private PlatformEffector2D plataformEffector;
    private BoxCollider2D boxCollider;
    private Transform player;
    private bool changeOffset;
    private bool isOnPlataform = false;

    void Start()
    {
        plataformEffector = GetComponent<PlatformEffector2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.S) && isOnPlataform)
        {
            StartCoroutine(ChangePlataformRotationOffset());
        }
    }

    IEnumerator ChangePlataformRotationOffset()
    {
        if (changeOffset)
            yield break;
        changeOffset = true;
        this.gameObject.layer = 2;

        boxCollider.enabled = false;
        yield return new WaitForSeconds(timePlataformRotation);
        boxCollider.enabled = true;
        changeOffset = false;

        plataformEffector.rotationalOffset = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            isOnPlataform = true;
            StartCoroutine(ChangeLayerEnum());
        }
    }

    bool changingLayer;
    IEnumerator ChangeLayerEnum()
    {
        if (changingLayer)
            yield break;
        changingLayer = true;
        yield return new WaitForSeconds(timeLayer);
        this.gameObject.layer = LayerMask.NameToLayer("Ground");
        changingLayer = false;
        
        yield return new WaitForSeconds(timeLayer);


        if (player.position.y < this.transform.position.y)
            this.gameObject.layer = 2;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject == player.gameObject)
        {
            isOnPlataform = false;
            StartCoroutine(ChangePlataformRotationOffset());
        }
    }
}
