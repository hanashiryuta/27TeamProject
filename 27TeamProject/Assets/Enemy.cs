//
//10月3日
//作成者：安部崇寛
//エネミー
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    float distance;//移動距離
    [SerializeField]
    float speed;//移動スピード

    float turndistance;
    float turnSpeed;
    Vector3 StartPosition;
    Vector3 velosity;

    bool isTurn;

	// Use this for initialization
	void Start () {
        turndistance = distance;
        StartPosition = transform.position;
        isTurn = false;
	}
	
	// Update is called once per frame
	void Update () {
        //初期値と前後の移動距離との差で前後移動
        if(StartPosition.x + turndistance >= transform.position.x && !isTurn)
        {
            Turn();   
        }
        else if (StartPosition.x - turndistance <= transform.position.x && isTurn)
        {
            Turn2();
        }
        
    }

    void Turn()
    {
        //前移動
        transform.position += new Vector3(speed, 0);

        if (StartPosition.x + turndistance <= transform.position.x)
        {
            isTurn = true;
        }
    }

    //後ろ移動
    void Turn2()
    {
        transform.position -= new Vector3(speed, 0);

        if (StartPosition.x - turndistance >= transform.position.x && isTurn)
        {
            isTurn = false;
        }
    }
}
