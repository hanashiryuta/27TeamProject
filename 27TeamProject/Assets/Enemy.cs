//
//10月3日
//作成者：安部崇寛
//エネミークラス
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    float distance;//移動距離
    [SerializeField]
    float speed;//移動スピード
    [SerializeField]
    int inputHp;//HPの初期設定

    float turndistance;
    float turnSpeed;
    int hp;
    
    public Vector3 StartPosition;

    bool isTurn;

	// Use this for initialization
	public virtual void Start () {
        turndistance = distance;
        StartPosition = transform.position;
        isTurn = false;
        hp = inputHp;
	}
	
	// Update is called once per frame
	public virtual void Update () {
        if(hp < 1)
        {
            Destroy(this.gameObject);
        }
    }

    public void Move()
    {
        //初期値と前後の移動距離との差で前後移動
        if (StartPosition.x + turndistance >= transform.position.x && !isTurn)
        {
            Turn();
        }
        else if (StartPosition.x - turndistance <= transform.position.x && isTurn)
        {
            reTurn();
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
    void reTurn()
    {
        transform.position -= new Vector3(speed, 0);

        if (StartPosition.x - turndistance >= transform.position.x && isTurn)
        {
            isTurn = false;
        }
    }
}
