//
//10月3日
//作成者：安部崇寛
//エネミークラス
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //[SerializeField]
    //float distance;//移動距離
    [SerializeField]
    float speed;//移動スピード
    [SerializeField]
    int inputHp;//HPの初期設定

    //float turndistance;
    float turnSpeed;
    int hp;
    
    //public Vector3 StartPosition;

    bool isTurn;
    
    RaycastHit hit;

    readonly Collider collider  = new Collider();

	// Use this for initialization
	public virtual void Start () {
        //turndistance = distance;
        //StartPosition = transform.position;
        isTurn = false;
        hp = inputHp;
	}
	
	// Update is called once per frame
	public virtual void Update () {
        if(hp < 1)
        {
            Destroy(this.gameObject);
        }



        Vector3 origin = new Vector3(transform.position.x - 0.5f, transform.position.y - 1, transform.position.z);
        //hit = 
            Physics.Raycast(origin, new Vector3(-1, -1, transform.position.z),out hit);
        Debug.DrawRay(transform.position, new Vector2(-0.5f, -1), Color.white, 1);
        Debug.Log(hit.collider);

        if (hit.collider == null && isTurn)
        {
            isTurn = !isTurn;
        }

        origin = new Vector2(transform.position.x + 0.5f, transform.position.y - 1);
        //hit = 
        Physics.Raycast(origin, new Vector3(1, -1, transform.position.z), out hit);
        Debug.DrawRay(transform.position, new Vector2(0.5f, -1), Color.white, 1);
        Debug.Log(hit.collider);
        
        if (hit.collider == null && !isTurn)
        {
            isTurn = !isTurn;
        }

        Debug.Log(isTurn);
    }

    public void Move()
    {
        //初期値と前後の移動距離との差で前後移動
        if(!isTurn)// (StartPosition.x + turndistance >= transform.position.x && !isTurn)
        {
            Turn();
        }
        else if(isTurn)// (StartPosition.x - turndistance <= transform.position.x && isTurn)
        {
            reTurn();
        }
    }

    //前移動
    void Turn()
    {
        transform.position += new Vector3(speed, 0);

        //if (StartPosition.x + turndistance <= transform.position.x)
        //{
        //    isTurn = true;
        //}
    }

    //後ろ移動
    void reTurn()
    {
        transform.position -= new Vector3(speed, 0);

        //if (StartPosition.x - turndistance >= transform.position.x && isTurn)
        //{
        //    isTurn = false;
        //}
    }

    void OnCollisionEnter(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {   
            isTurn = !isTurn;
        }
    }
}
