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

    float bulletTime;
    
    // Use this for initialization
    public override void Start () {
        bulletTime = time;
        base.Start();
    }
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
        Move();
        //一定時間ごとに弾丸を生成
        bulletTime -= Time.deltaTime;
        if (bulletTime < 0)
        {
            Instantiate(Bullet,transform.position,Quaternion.identity);
            bulletTime = time;
        }
	}
}
