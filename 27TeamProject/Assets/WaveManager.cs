using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public bool Flag = false;
    public GameObject WaveBlock;
    
    float x, ix, y;
    float time = 0.5f;
    int count = 0;
    int max = 5;    

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
        x = transform.position.x + 10;
        ix = transform.position.x - 10;
        if (Flag == true)
        {
            y = 10;
            time -= Time.deltaTime;
            if (time <= 0)
            {
                Vector3 CreatePoint = new Vector3(x, y, -2);
                if (count == max)
                {
                    Instantiate(WaveBlock, CreatePoint, Quaternion.identity);
                }
                Vector3 CreatePoint2 = new Vector3(x, y, 2);
                if (count == max)
                {
                    Instantiate(WaveBlock, CreatePoint2, Quaternion.identity);
                }
                Vector3 CreatePoint3 = new Vector3(ix, y, -2);
                if (count == max)
                {
                    Instantiate(WaveBlock, CreatePoint3, Quaternion.identity);
                }
                Vector3 CreatePoint4 = new Vector3(ix, y, 2);
                if (count == max)
                {
                    Instantiate(WaveBlock, CreatePoint4, Quaternion.identity);
                }
                count++;
                time = 0.02f;
            }

        }

    }
    void Update()
    {
        BlockInstance();
        //Spark();
    }
}

