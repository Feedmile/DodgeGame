using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private bool wait = false;
    private int count;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float swarmerInterval = 3.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!wait)StartCoroutine(SpawnEnemy(enemyPrefab));
    }

    private IEnumerator  SpawnEnemy(GameObject enemy)
    {
        wait = true;
        
        GameObject spawnedEnemy = Instantiate(enemy, new Vector2(Random.Range(-5f, 5f), 0f), Quaternion.identity);
        
        yield return new WaitForSecondsRealtime(4);
        wait = false;

    }
    
}
