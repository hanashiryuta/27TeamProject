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
    [HideInInspector]
    public int SpawnCount;

    [HideInInspector]
    public List<float> spawnRate;
    [HideInInspector]
    public List<GameObject> enemyList;
    [HideInInspector]
    public EnemySpawnManager enemySpawnManager;

    protected List<GameObject> spawnList;

    [HideInInspector]
    public WaveManager waveManager;

    public GameObject origin_Spawn_Particle;
    protected GameObject spawn_Particle;

    protected GameObject enemy;

    BoxCollider box;
    float x, z;

    // Use this for initialization
    public virtual void Start () {
        box = GetComponent<BoxCollider>();
        box.size = new Vector3(0, 0, 0);
        x = 0;
        z = x;
        box.enabled = false;
    }

    public void RateSet(WaveManager waveManager)
    {
        spawnList = new List<GameObject>();
        SpawnTime = SpawnSetTime;
        this.waveManager = waveManager;
        for (int r = 0; r < spawnRate.Count; r++)
        {
            for (int i = 0; i < spawnRate[r]; i++)
            {
                spawnList.Add(enemyList[r]);
            }
        }
    }
	
	public virtual void Update () {
        if (waveManager.GetComponent<WaveManager>().isWave&&enemySpawnManager.isSpawn&&spawnList.Count > 0)
        {
            if (enemy == null || (enemy != null&&Vector3.Distance(transform.position, enemy.transform.position) > 2))
                SpawnTime -= Time.deltaTime;

            if(SpawnTime < 2 && spawn_Particle == null)
            {
                spawn_Particle = Instantiate(origin_Spawn_Particle, transform.position, Quaternion.identity, transform);
            }

            if(SpawnTime < 2)
            {
                box.enabled = true;
                x += 0.05f;
                z = x;
                box.size = new Vector3(x, 0, z);
            }

            if(SpawnTime < 0)
            {
                if(SpawnCount < SpawnLimit)
                {
                    int rand = Random.Range(0, spawnList.Count);
                    enemy = Instantiate(spawnList[rand], transform.position + new Vector3(0, spawnList[rand].transform.localScale.y / 2,0), Quaternion.identity);
                    enemy.GetComponent<Enemy>().waveManager = waveManager.GetComponent<WaveManager>();
                    enemy.GetComponent<Enemy>().enemySpawnManager = enemySpawnManager;
                    SpawnTime = SpawnSetTime;
                    SpawnCount++;
                    enemySpawnManager.enemyCount++;
                    box.size = new Vector3(0, 0, 0);
                    x = 0;
                    z = x;
                    box.enabled = false;
                    Destroy(spawn_Particle);
                }
            }
        }
        else
        {
            SpawnCount = 0;
        }
	}
}
