//
//12月3日
//作成者：安部崇寛
//ポールエネミースポーンクラス
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaulEnemySpawn : EnemySpawn {

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (waveManager.GetComponent<WaveManager>().isWave)
        {
            SpawnTime -= Time.deltaTime;
            if (SpawnTime < 0)
            {
                if (SpawnCount < SpawnLimit)
                {
                    GameObject enemy = Instantiate(SpawnEnemy, transform.position, Quaternion.identity);
                    foreach(var pl in enemy.GetComponent<PaulLaserScript>().PaulList)
                    {
                        pl.GetComponent<Enemy>().waveManager = waveManager.GetComponent<WaveManager>();
                    }
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
