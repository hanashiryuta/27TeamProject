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
    float waveSpeed = -51;
    //float waveSpeedMax = 20;
    int count = 0;
    int max = 5;



    [HideInInspector]
    public int waveCount = 1;
    [HideInInspector]
    // public int waveCountWarning = 1;
    bool isSceneChange;
    [HideInInspector]
    public bool isWave = false;
    [HideInInspector]
    public bool isWarning = false;
    float waveInterval = 5;

    [HideInInspector]
    public int enemyDeathNum;
   
    //public GameObject canvas;

    public Text enemyDeathCountText;
    public Text waveCountText;
    public GameObject WaveWarningCount;

    public EnemySpawnManager enemySpawnManager;

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
            Instantiate(WaveBlock, CreatePoint2, Quaternion.Euler(0, -90, 0));
        }
        Vector3 CreatePoint3 = new Vector3(ix, y, transform.position.z + 8);
        //if (count == max)
        {
            Instantiate(WaveBlock, CreatePoint3, Quaternion.Euler(0, -180, 0));
        }
        Vector3 CreatePoint4 = new Vector3(ix, y, transform.position.z - 10);
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
        //Instantiate(WaveBlock, new Vector3(1.0f, 2.0f, 0.0f), Quaternion.identity);
        WaveWarningCount.transform.position = new Vector3(1400, 0, 0);
        
    }

    void Update()
    {


        if (isWarning == false)
        {
            WaveWarningCount.transform.position += new Vector3(waveSpeed, 0, 0);
            waveSpeed = waveSpeed + 0.94f;
            if (waveInterval < 4)
            {
                //  waveSpeed = 0;
                if (waveInterval < 2)
                {
                    isWarning = true;
                }
            }
        }
        Debug.Log(isWarning + "判定");
        if (isWarning == true)
        {

            WaveWarningCount.transform.position -= new Vector3(waveSpeed, 0, 0);
            waveSpeed = waveSpeed + 1.3f;
        }
        if (waveCount > 0)
        {
            waveCountText.text = "wave:" + waveCount.ToString();
           // WaveWarningCount.text = "WAVE" + waveCount.ToString();

        Debug.Log(waveSpeed);
        }
    
        else
            waveCountText.text = "";
            //warningWaveCountText.text = "";
        enemyDeathCountText.text = enemyDeathNum.ToString() + "/" + (20 + 10 * waveCount).ToString();
        if (!isWave)
        {

            waveInterval -= Time.deltaTime;
            if (waveInterval < 0)
            {
                waveInterval = 5;

                isWave = true;
            }
        }
        else
        {
            if (enemyDeathNum >= 20 + 10 * waveCount)
            {
                waveCount++;
                //waveCountWarning++;
                enemyDeathNum = 0;
                isWave = false;                
                enemySpawnManager.RateSet();
                Instantiate(WaveWarningCount);
                WaveWarningCount.transform.position = new Vector3(1400, 0, 0);
            }
        }


        if (waveCount > 3)
        {
            isSceneChange = true;
            SceneManager.LoadScene("GameClear");
        }
    }
}

