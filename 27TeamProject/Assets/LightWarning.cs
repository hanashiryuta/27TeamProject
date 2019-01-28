//
//12月17日
//ライト点滅クラス
//田中　悠斗
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightWarning : MonoBehaviour
{
    public Light lt;
    [HideInInspector]
    public bool isUp = true;
    int count;
    public EnemySpawnManager enemySpawnManager;

    void Start()
    {
        lt = GetComponent<Light>();
        lt.color = new Color(1, 1, 1, 1);
        enemySpawnManager.isBossSpawn = false;
        count = 0;

    }

    public void Update()
    {
        if (count < 240)
        {
            if (enemySpawnManager.isBossSpawn == true)
            {
                if (lt.color.b > 1)
                {
                    isUp = true;
                }
                else if (lt.color.b < 0)
                {
                    isUp = false;
                }
                if (isUp == true)
                {
                    //lt.color -= Color.white / 1.0f * Time.deltaTime;
                    lt.color -= Color.green / 1.0f * Time.deltaTime;
                    lt.color -= Color.blue / 1.0f * Time.deltaTime;
                }
                else if (isUp == false)
                {

                    lt.color += Color.green / 1.0f * Time.deltaTime;
                    lt.color += Color.blue / 1.0f * Time.deltaTime;
                }
                count++;
            }
            
        }
        else
        {
            lt.color = new Color(1, 1, 1, 1);
        }
    }
}