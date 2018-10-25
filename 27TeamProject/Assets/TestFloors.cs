using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFloors : MonoBehaviour
{

    public float blockSize;
    public GameObject block;
    public float XBlockNum;
    public float ZBlockNum;

    // Use this for initialization
    void Start()
    {
        block.transform.localScale = new Vector3(blockSize, blockSize, blockSize);
        for (int j = 0; j < ZBlockNum; j++)
        {
            for (int i = 0; i < XBlockNum; i++)
            {
                GameObject b = Instantiate(block, new Vector3(transform.position.x + blockSize * i, transform.position.y, transform.position.z + blockSize * j), Quaternion.identity, transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
