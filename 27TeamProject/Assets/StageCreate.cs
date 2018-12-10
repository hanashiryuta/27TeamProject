//
//12/10
//ステージ作成クラス
//葉梨竜太
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCreate : MonoBehaviour {
    
    public float blockSize;
    public GameObject block;
    public float XBlockNum;
    public float ZBlockNum;
    int _x, _y = 100;

    // Use this for initialization
    void Start()
    {
        block.transform.localScale = new Vector3(blockSize, blockSize, blockSize);
        for (int j = 0; j < ZBlockNum; j++)
        {
            for (int i = 0; i < XBlockNum; i++)
            {
                GameObject b = Instantiate(block, new Vector3(transform.position.x + blockSize * (i - XBlockNum / 2), transform.position.y, transform.position.z + blockSize * (j - ZBlockNum / 2)), Quaternion.identity, transform) as GameObject;
                Vector2 offset = new Vector2(_x + 0.1f * i, _y + 0.1f * j);
                b.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(0.1f, 0.1f));
                b.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", offset);
                b.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
}
