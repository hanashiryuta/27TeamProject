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
    public float SpawnSetTime;
    [SerializeField]
    public float SpawnTime;
    [SerializeField]
    public GameObject SpawnEnemy;
    [SerializeField]
    public int SpawnLimit;
    [SerializeField]
    public int SpawnCount;
    
    public GameObject waveManager;

	// Use this for initialization
	public virtual void Start () {
        SpawnTime = SpawnSetTime;
        waveManager = GameObject.FindGameObjectWithTag("WaveManager");
	}
	
	// Update is called once per frame
	public virtual void Update () {
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
