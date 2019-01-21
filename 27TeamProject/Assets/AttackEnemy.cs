//
//10月3日
//作成者：安部崇寛
//攻撃するエネミークラス
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : Enemy {

    [SerializeField]
    GameObject Bullet;//生成する弾丸を参照
    [SerializeField]
    float time;//発射する間隔

    float bulletTime; //再発射までの時間

    [SerializeField]
    float BulletRange;
    
    // Use this for initialization
    public override void Start () {
        bulletTime = time;
        base.Start();
    }
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
        if (!BlowMode)
        {
            if (isHook)
            {
                Move();
                //一定時間ごとに弾丸を生成
                bulletTime -= Time.deltaTime;
                GameObject Player = GameObject.FindGameObjectWithTag("Player");
                Vector3 pos = Player.transform.position - transform.position;
                Vector3 normalpos = Vector3.Normalize(pos);
                if (bulletTime < 0)
                {
                    //レンジの外なら発射しない
                    if (Mathf.Abs(pos.x) < BulletRange && Mathf.Abs(pos.z) < BulletRange)
                    {
                        Instantiate(Bullet, transform.position + new Vector3(normalpos.x * 1.2f, 0, normalpos.z * 1.2f), Quaternion.identity);
                        bulletTime = time;
                    }
                }
            }
        }
        else
        {
            Blow();
        }
	}
}
