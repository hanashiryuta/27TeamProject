///
///10月12日
///石橋功基
///ボスクラス
///


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    Move,
    Stop,
    Attack,
}

public class BossControl : MonoBehaviour {

    private Vector3 target; //プレイヤーの現在値取得
    private BossControl BC;
    protected GameObject player;
    protected GameObject leftblock;
    protected GameObject rightblock;
    private bool leftcheck; //プレイヤーの方向左側判定
    private bool rightcheck; //プレイヤーの方向右側判定
    private float interval = 0.1f; //点滅インターバル
    private bool playerCheck;

    float smoothTime = 0.5f;
    Vector3 velocity = Vector3.zero;

    [HideInInspector]
    public BossState bossState = BossState.Move;
    private float time = 0;

    public GameObject rock;
    public Transform point;
    private bool rockflag;
    public float hp = 0;

    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("Player");
        leftblock = GameObject.FindWithTag("Leftblock");
        rightblock = GameObject.FindWithTag("Rightblock");
        leftcheck = false;
        rightcheck = false;
        BC = this.GetComponent<BossControl>();
        playerCheck = false;
        rockflag = false;
    }

    // Update is called once per frame
    void Update() {
        if (playerCheck)
        {
            Move();
        }
    }

    //移動処理
    void Move()
    {
        time += Time.deltaTime;
        switch (bossState)
        {
            case BossState.Move:
                Vector3 targetPos = transform.position;
                targetPos.z = player.transform.position.z;
                transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
                if(time >= 10)
                {
                    bossState = BossState.Stop;
                    time = 0;
                }
                break;

            case BossState.Stop:
                transform.position = transform.position;
                if (time >= 5)
                {
                    bossState = BossState.Attack;
                    time = 0;
                }
                break;

            case BossState.Attack:
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
                    transform.Translate(new Vector3(-3.0f * Time.deltaTime, 0));
                }
                if (rightcheck)
                {
                    transform.Translate(new Vector3(3.0f * Time.deltaTime, 0));

                }
                break;
        }
    }

    void Hit()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Leftblock" && leftcheck)
        {
            leftcheck = false;
            StartCoroutine("Blink");
            rockflag = false;
            for (int count = 0; count < 3; count++)
            {
                RockInstantiate();
            }
            BC.enabled = false;
            Invoke("Release", 2.0f);//衝突時2秒間停止
        }

        if (col.gameObject.tag == "Rightblock" && rightcheck)
        {
            rightcheck = false;
            StartCoroutine("Blink");
            rockflag = true;
            for (int count = 0; count < 3; count++)
            {
                RockInstantiate();
            }
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
        bossState = BossState.Move;
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

    //岩のインスタンス生成
    public void RockInstantiate()
    {
        GameObject shot = Instantiate(rock, point.transform.position, Quaternion.identity);
    }

    public bool RockFlag()
    {
        return rockflag;
    }

    void OnBecameInvisible()
    {
        playerCheck = false;
    }
    void OnBecameVisible()
    {
        playerCheck = true;
    }
}