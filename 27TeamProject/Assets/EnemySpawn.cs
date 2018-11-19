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
    
    GameObject waveManager;

	// Use this for initialization
	void Start () {
        SpawnTime = SpawnSetTime;
        waveManager = GameObject.FindGameObjectWithTag("WaveManager");
	}
	
	// Update is called once per frame
	void Update () {
        if (waveManager.GetComponent<WaveManager>().isWave)
        {
            SpawnTime -= Time.deltaTime;
            if(SpawnTime < 0)
            {
                if(SpawnCount < SpawnLimit)
                {
                    GameObject enemy = Instantiate(SpawnEnemy, transform.position, Quaternion.identity);
                    enemy.GetComponent<Enemy>().waveManager = waveManager.GetComponent<WaveManager>();
                    SpawnTime = SpawnSetTime;
                    SpawnCount++;
                }
            }
        }
        else
        {
            SpawnCount = 0;
        }
	}
}
