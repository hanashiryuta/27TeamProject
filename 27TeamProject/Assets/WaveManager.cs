//
//1月21日
//田中　悠斗
//ウェーブクラス
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [HideInInspector]
    public int enemyDeathNum;
    [HideInInspector]
    public int waveCount = 1;
    float x, ix, y;
    float waveSpeed = -51.5f;
    float waveSpeedMin = 4;
    float waveSpeed_RightEnd = 3;
    float BossCount;
    float BossCountMax;
    float waveInterval = 5;
   
    [HideInInspector]
    public bool isWave = false;
    [HideInInspector]
    public bool isWarning = false;
    [HideInInspector]
    public bool Flag = false;
    [HideInInspector]
    public bool isUp;
    [HideInInspector]
    public bool isSceneChange;

    public bool isBossLight = false;

    public GameObject WaveBlock;
    public GameObject canvas;
    public GameObject WaveWarningCountObject;
    public GameObject BossWarningObject;

    public GameObject BossWarningLightObject;
    public GameObject MainLight;

    GameObject BossWarningLight;
    GameObject BossWarning;

    GameObject WaveWarningCount;

    public Text enemyDeathCountText;
    public Text waveCountText;

    public FadeScript fadeScript;
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
        y = 10;
        Vector3 CreatePoint = new Vector3(x, y, transform.position.z - 10);
        {
            Instantiate(WaveBlock, CreatePoint, Quaternion.identity);
        }
        Vector3 CreatePoint2 = new Vector3(x, y, transform.position.z + 8);
        {
            Instantiate(WaveBlock, CreatePoint2, Quaternion.Euler(0, -90, 0));
        }
        Vector3 CreatePoint3 = new Vector3(ix, y, transform.position.z + 8);
        {
            Instantiate(WaveBlock, CreatePoint3, Quaternion.Euler(0, -180, 0));
        }
        Vector3 CreatePoint4 = new Vector3(ix, y, transform.position.z - 10);
        {
            Instantiate(WaveBlock, CreatePoint4, Quaternion.Euler(0, -270, 0));
        }
        
    }

    private void Start()
    {
        BlockInstance();
        WaveWarningCount = Instantiate(WaveWarningCountObject, canvas.transform);
        WaveWarningCount.transform.localPosition = new Vector3(1400, 0, 0);
      



    }

    private void Update()
    {

        if (isWarning == false)
        {
            WaveWarningCount.transform.position += new Vector3(waveSpeed, 0, 0);
            if (waveSpeed < waveSpeedMin)
            {
                waveSpeed = waveSpeed + 0.96f;
            }
            if (waveSpeed > waveSpeed_RightEnd)
            {
                waveSpeed = 0;
                if (waveInterval < 2)
                {
                    isWarning = true;
                }
            }
        }
       
        if (WaveWarningCount != null)
        {
            if (isWarning == true)
            {
                WaveWarningCount.transform.position -= new Vector3(waveSpeed, 0, 0);
                waveSpeed = waveSpeed + 1.3f;
            }
        }

        if (waveInterval < 0.5f)
        {
            Destroy(WaveWarningCount);
        }

        if (waveCount > 0)
        {
            waveCountText.text = "wave:" + waveCount.ToString();
            if (WaveWarningCount != null)
            {
                WaveWarningCount.GetComponent<Text>().text = "WAVE" + waveCount.ToString();
            }
            enemyDeathCountText.text = enemyDeathNum.ToString() + "/" + (10 + 10 * waveCount).ToString();
        }

        else
        {
            waveCountText.text = "";
            WaveWarningCount.GetComponent<Text>().text = "";
        }

        if (!isWave)
        {
            waveInterval -= Time.deltaTime;
            if (waveInterval < 0)
            {
                waveInterval = 5;
                isWave = true;
            }

            if (enemySpawnManager.GetComponent<EnemySpawnManager>().isBossSpawn == true)
            {
                
                if (waveInterval < 4.5f)
                {
                    isBossLight = true;
                    if (BossCount == BossCountMax)
                    {
                        BossWarning = Instantiate(BossWarningObject, canvas.transform);
                        //BossWarningLight = Instantiate(BossWarningLightObject);
                        //MainLight.SetActive(false);
                    }
                    BossCount++;
                }

                else
                {
                    //MainLight.SetActive(true);
                    Destroy(BossWarning);
                    //Destroy(MainLight);
                    
                }
            }
        }
        else
        {
            if (enemyDeathNum >= 10 + 10 * waveCount)
            {
                WavePlus();
                if (enemySpawnManager.GetComponent<EnemySpawnManager>().isBossSpawn == false)
                {
                    isWarning = false;
                    waveSpeed = -51.5f;
                    WaveWarningCount = Instantiate(WaveWarningCountObject, canvas.transform);
                    WaveWarningCount.transform.localPosition = new Vector3(1400, 0, 0);
                }
            }
        }

        if (waveCount > 3)
        {
            isSceneChange = true;
            fadeScript.nextScene = "GameClear";
            fadeScript.isSceneEnd = true;
        }
    }

    public void WavePlus()
    {
        waveCount++;
        enemyDeathNum = 0;
        isWave = false;
        enemySpawnManager.RateSet();
    }
}

