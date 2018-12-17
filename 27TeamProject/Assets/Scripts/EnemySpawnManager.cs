//
//12/10
//エネミースポーン管理クラス
//葉梨竜太
//
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour {

    public List<GameObject> enemySpawnPoints;
    public List<GameObject> enemyList;
    List<List<float>> pointRate;
    [HideInInspector]
    public bool isSpawn;
    public float enemyCount;
    public int enemyRange;

    public WaveManager waveManager;

    // Use this for initialization
    void Awake () {
        RateSet();
    }

    public void RateSet()
    {
        pointRate = new List<List<float>>();

        TextAsset file;
        file = Resources.Load("enemyRate/" + GameObject.Find("Nametransprot").GetComponent<Name>().stagename + "-" + waveManager.waveCount) as TextAsset;
        StringReader reader = new StringReader(file.text);

        while (reader.Peek() > -1)
        {
            string[] line = reader.ReadLine().Split(',');
            List<float> lineList = new List<float>();
            for (int i = 0; i < line.Length; i++)
            {
                lineList.Add(float.Parse(line[i]));
            }
            pointRate.Add(lineList);
        }

        for (int i = 0; i < enemySpawnPoints.Count; i++)
        {
            EnemySpawn enemySpawn = enemySpawnPoints[i].GetComponent<EnemySpawn>();
            enemySpawn.spawnRate = pointRate[i];
            enemySpawn.enemyList = enemyList;
            enemySpawn.enemySpawnManager = this;
            enemySpawn.RateSet(waveManager);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        isSpawn = enemyCount < enemyRange ? true : false;
        Debug.Log(isSpawn);
	}
}
