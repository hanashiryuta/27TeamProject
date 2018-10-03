//
//10月3日
//作成者：安部崇寛
//エネミーの攻撃
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackEnemy : MonoBehaviour {

    [SerializeField]
    GameObject Bullet;//生成する弾丸を参照
    [SerializeField]
    float time;//発射する間隔

    float bulletTime;
	// Use this for initialization
	void Start () {
        bulletTime = time;
	}
	
	// Update is called once per frame
	void Update () {
        //一定時間ごとに弾丸を生成
        bulletTime -= Time.deltaTime;
        if (bulletTime < 0)
        {
            Instantiate(Bullet,transform.position,Quaternion.identity);
            bulletTime = time;
        }
	}
}
