using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEaterController : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    
    void Start()
    {
        StartCoroutine(Spawn());   
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(Random.Range(1, 5));
             
        while (true)
        {
            foreach (GameObject enemy in enemies)
            {
                if (!enemy.gameObject.activeSelf)
                {
                    enemy.SetActive(true);
                    yield return new WaitForSeconds(0.5f);
                }
            }

            yield return new WaitForSeconds(5);
        }
    }
}
