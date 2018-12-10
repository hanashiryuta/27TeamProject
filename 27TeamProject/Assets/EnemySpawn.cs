//
//11月
//作成者：安部崇寛
//Enemyのスポーン
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    [SerializeField]
    float SpawnSetTime;
    [SerializeField]
    float SpawnTime;
    [SerializeField]
    GameObject SpawnEnemy;
    [SerializeField]
    int SpawnLimit;
    [SerializeField]
    int SpawnCount;

    [HideInInspector]
    public List<float> spawnRate;
    [HideInInspector]
    public List<GameObject> enemyList;
    [HideInInspector]
    public EnemySpawnManager enemySpawnManager;

    List<GameObject> spawnList;

    GameObject waveManager;

	// Use this for initialization
	void Start () {
        spawnList = new List<GameObject>();
        SpawnTime = SpawnSetTime;
        waveManager = GameObject.FindGameObjectWithTag("WaveManager");
        for (int r = 0; r < spawnRate.Count; r++)
        {
            for(int i = 0; i <= spawnRate[r]; i++)
            {
                spawnList.Add(enemyList[r]);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (waveManager.GetComponent<WaveManager>().isWave&&enemySpawnManager.isSpawn)
        {
            SpawnTime -= Time.deltaTime;
            if(SpawnTime < 0)
            {
                if(SpawnCount < SpawnLimit)
                {
                    int rand = Random.Range(0, 11);
                    GameObject enemy = Instantiate(spawnList[rand], transform.position, Quaternion.identity);
                    enemy.GetComponent<Enemy>().waveManager = waveManager.GetComponent<WaveManager>();
                    enemy.GetComponent<Enemy>().enemySpawnManager = enemySpawnManager;
                    SpawnTime = SpawnSetTime;
                    SpawnCount++;
                    enemySpawnManager.enemyCount++;
                }
            }
        }
        else
        {
            SpawnCount = 0;
        }
	}
}
