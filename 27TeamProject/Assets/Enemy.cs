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
    float speed;//移動スピード
    [SerializeField]
    int inputHp;//HPの初期設定

    //float turndistance;
    float turnSpeed;

    [SerializeField]
    int hp;

    bool isTurn;
    RaycastHit[] hitList;
    Vector3 origin;
    Vector3 boxcastScale;

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    GameObject hook;

    [SerializeField]
    float BlowOffSpeed;

    [HideInInspector]
    public bool isHook;

    bool isBlow;
    
    public bool BlowMode;

    [SerializeField]
    int ThrowAtack;

    // Use this for initialization
    public virtual void Start () {
        isTurn = false;
        hp = inputHp;
        isHook = true;
        BlowMode = false;
	}

    // Update is called once per frame
    public virtual void Update()
    {
        if (hp < 1)
        {
            Destroy(this.gameObject);
        }

        if (!isTurn)
        {
            origin = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
            hitList = Physics.BoxCastAll(origin, boxcastScale, -transform.up, Quaternion.identity, 1.0f, layerMask);
            Debug.DrawRay(origin, -transform.up);
            
            int groundcount = 0;
            foreach (var hl in hitList)
            {
                if (hl.transform.tag == "Ground")
                {
                    groundcount++;
                }
            }

            if (groundcount == 0)
            {
                isTurn = !isTurn;
            }
        }
        else if (isTurn)
        {
            origin = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
            hitList = Physics.BoxCastAll(origin, boxcastScale, -transform.up, Quaternion.identity, 1.0f, layerMask);
            Debug.DrawRay(origin, -transform.up);
            
            int groundcount = 0;
            foreach (var hl in hitList)
            {
                if (hl.transform.tag == "Ground")
                {
                    groundcount++;
                }
            }

            if (groundcount == 0)
            {
                isTurn = !isTurn;
            }
        }
        Debug.Log(isTurn);
    }


    public void Move()
    {   
        //初期値と前後の移動距離との差で前後移動
        if(!isTurn)
        {
            Turn();
        }
        else if(isTurn)
        {
            reTurn();
        }
    }

    //前移動
    void Turn()
    {
        transform.position += new Vector3(speed, 0, 0);
    }

    //後ろ移動
    void reTurn()
    {
        transform.position -= new Vector3(speed, 0, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 12)
        {
            BlowMode = true;
            GetComponent<Rigidbody>().useGravity = false;
            Vector3 Pos = transform.position - collision.transform.position;
            if(Pos.z > 0)
            {
                isBlow = true;
            }
            if(Pos.z < 0)
            {
                isBlow = false;
            }
        }
        else if(collision.gameObject.layer == 14)
        {
            hp -= collision.gameObject.GetComponent<Enemy>().ThrowAtack;
        }
    }

    public void Blow()
    {
        if (isBlow)
        {
            BackBlow();
        }
        else
        {
            FrontBlow();
        }
    }

    void BackBlow()
    {
        transform.position += new Vector3(BlowOffSpeed, 0, BlowOffSpeed);
    }

    void FrontBlow()
    {
        transform.position -= new Vector3(BlowOffSpeed, 0, BlowOffSpeed);
    }
}
