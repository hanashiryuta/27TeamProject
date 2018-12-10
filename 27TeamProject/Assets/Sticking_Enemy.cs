//
//11月19日
//田中　悠斗
//くっつくエネミーのクラス
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticking_Enemy : Enemy
{

    void OnTriggerEnter(Collider hit)
    {
        if (!isHook)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (hit.gameObject.CompareTag("Enemy"))
            {
                hit.gameObject.transform.parent = this.transform;
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }

    public override void Update()
    {
        base.Update();
        Move();
    }
}