using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnim : MonoBehaviour
{
    [SerializeField] private float yValue;
    [SerializeField] private float endPos;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMoveY(yValue + transform.position.y, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
}
