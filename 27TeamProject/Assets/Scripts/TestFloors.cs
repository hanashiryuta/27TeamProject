using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFloors : MonoBehaviour
{

    public float blockSize;
    public GameObject block;
    public float XBlockNum;
    public float ZBlockNum;
    int _x, _y = 100;
    

    // Use this for initialization
    void Start()
    {
        transform.position -= new Vector3(20, 0,0);
        block.transform.localScale = new Vector3(blockSize, blockSize, blockSize);
        for (int j = 0; j < ZBlockNum; j++)
        {
            for (int i = 0; i < XBlockNum; i++)
            {
                // b = Instantiate(block, new Vector3(transform.position.x + blockSize * i, transform.position.y, transform.position.z + blockSize * j), Quaternion.identity, transform);
                GameObject b = Instantiate(block, new Vector3(transform.position.x + blockSize * i, transform.position.y, transform.position.z + blockSize * j), Quaternion.identity,transform) as GameObject;
                Vector2 offset = new Vector2(_x + 0.1f * i, _y + 0.1f * j);
                b.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(0.1f, 0.1f));
                b.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", offset);
                b.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
