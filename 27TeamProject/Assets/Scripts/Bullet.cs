//
//10月3日
//作成者：安部崇寛
//エネミーが発射する弾丸
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField]
    float BulletSpeed;//弾丸のスピード

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //常にスピード分進み続ける
        transform.position += new Vector3(BulletSpeed, 0);

        //画面外に行ったら消滅
        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(this.gameObject);
        }
    }
}
