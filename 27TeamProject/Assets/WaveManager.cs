using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public bool Flag = false;
    public GameObject WaveBlock;
    
    float x, ix, y;
    float time = 0.5f;
    int count = 0;
    int max = 5;

    [HideInInspector]
    public int waveCount = 1;
    bool isSceneChange;
    [HideInInspector]
    public bool isWave = false;
    float waveInterval=5;

    [HideInInspector]
    public int enemyDeathNum;

    public Text enemyDeathCountText;
    public Text waveCountText;

    public EnemySpawnManager enemySpawnManager;

    public FadeScript fadeScript;

    //public void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Flag = true;
    //        Debug.Log("当たった");
    //    }
    //}
    //public void Spark()
    //{
        
    //    if (Flag == true)
    //    {
    //        time -= Time.deltaTime;
    //        if (time <= 0)
    //        {

    //            for (int z = -1; z < 2; z++)
    //            {
    //                Vector3 BoltPoint = new Vector3(x, 1.5f, z);
    //                if (count == max)
    //                {
    //                    Instantiate(BoltBlock, BoltPoint, Quaternion.Euler(0, 90, 0));
    //                }
    //                count++;
    //            }

    //            for (int z = -1; z < 2; z++)
    //            {
    //                Vector3 BoltPoint = new Vector3(ix, 1.5f, z);
    //                if (count == max)
    //                {
    //                    Instantiate(BoltBlock, BoltPoint, Quaternion.Euler(0, 90, 0));
    //                }
    //                count++;
    //            }
               
    //            time = 2f;
    //        }
    //    }
    //}
    public void BlockInstance()
    {
        x = transform.position.x + 30;
        ix = transform.position.x - 30;
        //if (Flag == true)
        //{
            y = 10;
        //    time -= Time.deltaTime;
        //    if (time <= 0)
        //    {
        Vector3 CreatePoint = new Vector3(x, y, transform.position.z - 10);
        //if (count == max)
        {
            Instantiate(WaveBlock, CreatePoint, Quaternion.identity);
        }
        Vector3 CreatePoint2 = new Vector3(x, y, transform.position.z + 8);
        //if (count == max)
        {
            Instantiate(WaveBlock, CreatePoint2, Quaternion.Euler(0,-90,0));
        }
        Vector3 CreatePoint3 = new Vector3(ix, y, transform.position.z +8);
        //if (count == max)
        {
            Instantiate(WaveBlock, CreatePoint3, Quaternion.Euler(0, -180, 0));
        }
        Vector3 CreatePoint4 = new Vector3(ix, y, transform.position.z -10);
        //if (count == max)
        {
            Instantiate(WaveBlock, CreatePoint4, Quaternion.Euler(0, -270, 0));
        }
        count++;
        time = 0.02f;
        //}

    //}

}
    void Start()
    {
        BlockInstance();
        //Spark();
    }

    void Update()
    {
        if (waveCount > 0)
            waveCountText.text = "wave:" + waveCount.ToString();
        else
            waveCountText.text = "";
        enemyDeathCountText.text = enemyDeathNum.ToString() + "/" + (10 + 10 * waveCount).ToString();
        if (!isWave)
        {
            waveInterval -= Time.deltaTime;
            if(waveInterval < 0)
            {
                waveInterval = 5;
                isWave = true;
            }
        }
        else
        {
            if (enemyDeathNum >= 10 + 10 * waveCount)
            {
                waveCount++;
                enemyDeathNum = 0;
                isWave = false;
                enemySpawnManager.RateSet();
            }
        }
        
        if(waveCount > 3)
        {
            isSceneChange = true;
            fadeScript.nextScene = "GameClear";
            fadeScript.isSceneEnd = true;
        }
    }
}

