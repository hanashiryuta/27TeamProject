//
//12月3日
//作成者：安部崇寛
//ポールエネミースポーンクラス
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaulEnemySpawn : EnemySpawn {

    BoxCollider box;
    float x, z;

    public override void Start()
    {
        base.Start();
        box = GetComponent<BoxCollider>();
        box.size = new Vector3(0, 1, 0);
        x = 0.3f; x = z;
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
                    box.size = new Vector3(box.size.x + 0.5f, box.size.y, box.size.z + 0.5f);
                    GameObject enemy = Instantiate(SpawnEnemy, transform.position, Quaternion.identity);
                    foreach(var pl in enemy.GetComponent<PaulLaserScript>().PaulList)
                    {
                        pl.GetComponent<Enemy>().waveManager = waveManager.GetComponent<WaveManager>();
                    }
                    SpawnTime = SpawnSetTime;
                    SpawnCount++;
                }
            }

            if (SpawnCount < SpawnLimit && SpawnTime < 2.0f - x && SpawnTime > 0)
            {
                x += 0.1f;
                z = x;
                box.size = new Vector3(x, box.size.y, z);
            }
        }
        else
        {
            SpawnCount = 0;
        }
    }
}
