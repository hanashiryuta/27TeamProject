///
///10月12日
///石橋功基
///ボスクラス
///


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControl : MonoBehaviour {

    private Vector3 target; //プレイヤーの現在値取得
    private BossControl BC;
    protected GameObject player;
    protected GameObject leftblock;
    protected GameObject rightblock;
    private bool leftcheck; //プレイヤーの方向左側判定
    private bool rightcheck; //プレイヤーの方向右側判定
    private float interval = 0.1f; //点滅インターバル

    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("Player");
        leftblock = GameObject.FindWithTag("Leftblock");
        rightblock = GameObject.FindWithTag("Rightblock");
        leftcheck = false;
        rightcheck = false;
        BC = this.GetComponent<BossControl>();
    }
	
	// Update is called once per frame
	void Update () {
        Move();
    }

    //移動処理
    void Move()
    {
        //プレイヤーの位置
        target = player.transform.position;

        //プレイヤーが左側
        if (!rightcheck && target.x < this.transform.position.x)
        {
            leftcheck = true;
        }

        //プレイヤーが右側
        else if (!leftcheck && target.x > this.transform.position.x)
        {
            rightcheck = true;
        }

        //移動処理
        if (leftcheck)
        {
            transform.Translate(new Vector3(-2.0f * Time.deltaTime, 0));
        }
        if (rightcheck)
        {
            transform.Translate(new Vector3(2.0f * Time.deltaTime, 0));
        }
    }

    void Motion()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Leftblock")
        {
            leftcheck = false;
            StartCoroutine("Blink");
            BC.enabled = false;

            Invoke("Release", 2.0f);//衝突時2秒間停止
        }

        if (col.gameObject.tag == "Rightblock")
        {
            rightcheck = false;
            StartCoroutine("Blink");
            BC.enabled = false;

            Invoke("Release", 2.0f);//衝突時2秒間停止
        }
    }

    //停止時処理
    void Release()
    {
        BC.enabled = true;
        StopCoroutine("Blink");
        var renderComponent = GetComponent<Renderer>();
        renderComponent.enabled = true;
    }

    //点滅処理
    IEnumerator Blink()
    {
        while (true)
        {
            var renderComponent = GetComponent<Renderer>();
            renderComponent.enabled = !renderComponent.enabled;
            yield return new WaitForSeconds(interval);
        }
    }
}
