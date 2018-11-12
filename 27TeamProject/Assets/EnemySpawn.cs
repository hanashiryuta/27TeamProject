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

	// Use this for initialization
	void Start () {
        SpawnTime = SpawnSetTime;
	}
	
	// Update is called once per frame
	void Update () {
        SpawnTime -= Time.deltaTime;
        if(SpawnTime < 0)
        {
            if(SpawnCount < SpawnLimit)
            {
                Instantiate(SpawnEnemy, transform.position, Quaternion.identity);
                SpawnTime = SpawnSetTime;
                SpawnCount++;
            }
        }
	}
}
