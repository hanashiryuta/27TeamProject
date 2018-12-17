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

    //くっつく数
    public int stickingCount;

    //くっつく最大値
    int countMax;

    public List<GameObject> child;

    //初期化
    public override void Start()
    {
        base.Start();

        stickingCount = 0;

        countMax = 5;
    }

    void OnTriggerEnter(Collider hit)
    {

        if (!isHook)
        {
            if (hit.gameObject.CompareTag("Enemy"))
            {
                if (stickingCount <= countMax)
                {
                    //親にくっつく
                    hit.gameObject.transform.parent = this.transform;
                    hit.GetComponent<Enemy>().isHook = false;
                    Rigidbody rb = hit.GetComponent<Rigidbody>();
                    if (rb != null && transform.parent == null)
                    {
                        Destroy(rb);
                    }

                    child.Add(hit.gameObject);

                }
                stickingCount++;
            }
        }
    }
    //
    public override void ThrowSet(float throwSpeed, Vector3 throwVelocity)
    {
        base.ThrowSet(throwSpeed, throwVelocity);
        for (int i = 0; i < child.Count; i++)
        {
            child[i].layer = 15;
            child[i].GetComponent<BoxCollider>().isTrigger = false;
        }
    }

    //更新
    public override void Update()
    {
        base.Update();
        if (isHook)
            Move();
    }
}