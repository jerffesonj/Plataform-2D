using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    [SerializeField] private int points;

    public int Points { get => points; }

    // Start is called before the first frame update
    public void AddPoints()
    {
        points += 1;
    }
}
