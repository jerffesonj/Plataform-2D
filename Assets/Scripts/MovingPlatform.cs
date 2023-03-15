using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform[] points;
    private int pointIndex = 0;
    
    void Update()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, points[pointIndex].transform.position, speed*Time.deltaTime);

        if (Vector2.Distance(this.transform.position, points[pointIndex].position) <= 0.01f)
        {
            pointIndex += 1;
            if (pointIndex >= points.Length)
            {
                pointIndex = 0;
            }
        }
    }
}
