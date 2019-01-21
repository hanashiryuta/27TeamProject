using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : EnemySpawn {
	
	// Update is called once per frame
	public override void Update () {
        if (waveManager.GetComponent<WaveManager>().isWave && enemySpawnManager.isSpawn && spawnList.Count > 0)
        {
            if (enemy == null )
                SpawnTime -= Time.deltaTime;

            if (SpawnTime < 2 && spawn_Particle == null)
            {
                spawn_Particle = Instantiate(origin_Spawn_Particle, transform.position, Quaternion.identity, transform);
            }
            if (SpawnTime < 0)
            {
                if (SpawnCount < SpawnLimit)
                {
                    int rand = Random.Range(0, spawnList.Count);
                    enemy = Instantiate(spawnList[rand], transform.position+new Vector3(0,0.5f,0), Quaternion.identity);
                    enemy.GetComponent<BossControl>().waveManager = waveManager;
                    SpawnCount++;
                    SpawnTime = SpawnSetTime;
                    enemySpawnManager.enemyCount++;
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
