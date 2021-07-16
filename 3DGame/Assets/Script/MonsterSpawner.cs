using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    List<GameObject> enemyPrefabs = new List<GameObject>();

    public Transform[] spawPoints;
    int monsterMaxCount;
    private void Start()
    {
        StartCoroutine(Spawn());
    }
    IEnumerator Spawn() 
    {
        while (true) 
        {
            for (int i = 0; i < Random.Range(0, 4); i++) 
            {
                Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)],spawPoints[Random.Range(0, spawPoints.Length)].position,Quaternion.identity);
                monsterMaxCount++;
            }

            yield return new WaitForSeconds(5);
        }
    }
}
