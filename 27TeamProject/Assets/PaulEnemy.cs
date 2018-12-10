//
//10月3日
//作成者：安部崇寛
//ポールエネミークラス
//

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaulEnemy : Enemy {

    public bool GetMasterBlow;
    
    public override void Awake()
    {

    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        //gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, -9.8f, 0));
        if (Math.Abs(transform.position.x) > 70 || Math.Abs(transform.position.z) > 50)
        {
            Destroy(this.gameObject);
        }

        if (GetMasterBlow)
        {
            Blow();
        }
    }

    public override void Blow()
    {
        Vector3 normal = Vector3.Normalize(PosBlow);
        transform.position += new Vector3(BlowOffSpeed * normal.x, 0, BlowOffSpeed * normal.z);
        //angleZ += 10;
        //transform.rotation = Quaternion.Euler(0, 0, angleZ);
    }

    public override void TriggerSet(Collider other)
    {
        int gethp = GetComponentInParent<PaulLaserScript>().hp;
        gethp -= other.gameObject.GetComponent<Enemy>().SwingAttack;
        GetComponentInParent<PaulLaserScript>().hp = gethp;

        if (GetMasterBlow)
        {   
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            PosBlow = transform.position - other.transform.position;
        }
    }
}
