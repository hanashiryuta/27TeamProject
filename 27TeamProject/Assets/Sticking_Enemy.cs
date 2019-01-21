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

    public override void Awake()
    {
        
    }

    //初期化
    public override void Start()
    {
        base.Start();

        stickingCount = 0;

        countMax = 5;
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.layer == CatchEnemyLayer)
        {
            GUIText = hit.gameObject.GetComponent<Enemy>().SwingAttack.ToString();
            isGUIDraw = true;
            hp -= hit.gameObject.GetComponent<Enemy>().SwingAttack;
            Instantiate(origin_Damege_Particle, transform.position, Quaternion.identity);
            status = Status.DAMEGE;
            TriggerSetRotate();
            moveStop = !moveStop;

            TriggerSet(hit);
        }

        if (hit.gameObject.layer == ThrowEnemyLayer)
        {
            GUIText = hit.gameObject.GetComponent<Enemy>().ThrowAttack.ToString();
            isGUIDraw = true;
            hp -= hit.gameObject.GetComponent<Enemy>().ThrowAttack;
            Instantiate(origin_Damege_Particle, transform.position, Quaternion.identity);
            status = Status.DAMEGE;
            TriggerSetRotate();
            moveStop = !moveStop;
            Physics.IgnoreCollision(hit.gameObject.GetComponent<BoxCollider>(), GetComponent<BoxCollider>());
            TriggerSetRotate();
        }

        if (!isHook)
        {
            if (hit.gameObject.CompareTag("Enemy") && hit.gameObject.GetComponent<Enemy>().isSticking == true)
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

                    // rb.useGravity = false;
                    // //Freeze固定
                    // rb.constraints = RigidbodyConstraints.FreezeRotation;
                    //// rb.velocity = Vector3.zero;

                    child.Add(hit.gameObject);
                    playerSP += 0.1f;
                }
                stickingCount++;
                //GetComponent<BoxCollider>().isTrigger = false;
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
        if (!BlowMode)
        {
            if (isHook) Move();
        }

        else
        {
            Blow();
        }
        for (int i = 0; i < child.Count; i++)
        {
            if(child[i] == null)
            {
                child.Remove(child[i]);
            }
        }
    }

    public override void AttackAnime()
    {
        animator.SetTrigger("isAttack");
    }

    public override void DeathAction()
    {
        Instantiate(origin_Death_Particle, transform.position, Quaternion.identity);
        if (waveManager.isWave)
            waveManager.enemyDeathNum += (stickingCount + 1);
        enemySpawnManager.enemyCount -= (stickingCount + 1);
        Destroy(this.gameObject);
    }
}