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

    Vector3 normalpos;

    void Awake()
    {
        normalpos = Vector3.Normalize(GameObject.FindGameObjectWithTag("Player").transform.position - transform.position);
        GetComponent<Rigidbody>().AddForce(normalpos * BulletSpeed);
    }

    // Update is called once per frame
    void Update () {
        //画面外に行ったら消滅
        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(this.gameObject);
        }
    }
}
