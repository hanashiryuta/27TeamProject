//
//10月3日
//作成者：安部崇寛
//エネミークラス
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaulEnemy : Enemy {

    RaycastHit[] hitlist;
    Vector3 boxcastScale;

    Status status;

    public override void Awake()
    {

    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void TriggerSet(Collider other)
    {
        
    }
}
