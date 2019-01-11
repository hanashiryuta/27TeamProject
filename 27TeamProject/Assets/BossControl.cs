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
    Damege,
    Dawn,
    Fly
}

public class BossControl : MonoBehaviour
{

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
    [SerializeField]
    GameObject hook;
    public bool isHook;
    private float time = 0;

    public GameObject rock;
    public Transform point;
    public GameObject boss;
    public float hitCount = 0;
    private bool rockflag;
    private Animator anime;
    private bool runflag;
    private bool damegeflag;
    private bool animeStop;
    private float angle;
    public float hp = 0;

    [HideInInspector]
    public WaveManager waveManager;

    public float origin_FlyTime;
    float flyTime;

    public float moveSpeed;

    public GameObject origin_BossDashParticle;
    GameObject bossDashParticle;

    public GameObject origin_StanParticle;
    GameObject stanParticle;

    public Transform dashPoint;
    public Transform stanPoint;

    public GameObject bossBombParticle;

    public Transform bombPoint;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        leftcheck = false;
        rightcheck = false;
        BC = this.GetComponent<BossControl>();
        playerCheck = false;
        rockflag = false;
        anime = GetComponent<Animator>();
        runflag = false;
        damegeflag = false;
        isHook = true;
        animeStop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerCheck)
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
                if (!runflag)
                {
                    boss.GetComponent<Animator>().SetTrigger("runTrigger");
                    runflag = true;
                }
                Vector3 targetPos = transform.position;
                targetPos.z = player.transform.position.z;
                transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
                if (time >= 3)
                {
                    bossState = BossState.Stop;
                    runflag = false;
                    time = 0;
                }
                break;

            case BossState.Stop:
                transform.position = transform.position;
                if (time >= 1)
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
                    if (bossDashParticle == null)
                        bossDashParticle = Instantiate(origin_BossDashParticle, dashPoint.position, Quaternion.Euler(0, 0, 90), transform);
                }

                //プレイヤーが右側
                else if (!leftcheck && target.x > this.transform.position.x)
                {
                    rightcheck = true;
                    if (bossDashParticle == null)
                        bossDashParticle = Instantiate(origin_BossDashParticle, dashPoint.position, Quaternion.Euler(0,0,-90), transform);
                }
                
                ////移動処理
                float trans = -moveSpeed * Time.deltaTime;
                transform.Translate(new Vector3(trans, 0));
                break;

            case BossState.Damege:
                if (!damegeflag)
                {
                    trans = 0;
                    time = 0;
                    damegeflag = true;
                    boss.GetComponent<Animator>().SetTrigger("runTrigger");
                }

                transform.position = transform.position;

                if (time < 3)
                {
                    //プレイヤーの位置
                    target = player.transform.position;

                    //プレイヤーが左側
                    if (target.x < this.transform.position.x)
                    {
                        leftcheck = true;
                        rightcheck = false;
                        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    }

                    //プレイヤーが右側
                    else if (target.x > this.transform.position.x)
                    {
                        rightcheck = true;
                        leftcheck = false;
                        this.transform.rotation = Quaternion.Euler(0.0f, -180.0f, 0.0f);
                    }
                }

                if (hitCount == 0)
                {
                    bossState = BossState.Dawn;
                }

                else if (3 < time)
                {
                    if (bossDashParticle == null)
                    {
                        if (rightcheck)
                            bossDashParticle = Instantiate(origin_BossDashParticle, dashPoint.position, Quaternion.Euler(0, 0, -90), transform);
                        else if (leftcheck)
                            bossDashParticle = Instantiate(origin_BossDashParticle, dashPoint.position, Quaternion.Euler(0, 0, 90), transform);
                    }
                    Debug.Log(Time.deltaTime);
                    ////移動処理
                    boss.GetComponent<Animator>().SetTrigger("runTrigger");
                    transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0));
                }
                break;

            case BossState.Dawn:
                if (!animeStop)
                {
                    boss.GetComponent<Animator>().SetTrigger("dawnTrigger");
                    animeStop = true;
                }
                if(time>2)
                {
                    boss.GetComponent<Animator>().speed = 0;
                }
                if(!isHook)
                {
                    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                }
                break;
            case BossState.Fly:
                flyTime += Time.deltaTime;
                if(flyTime > origin_FlyTime)
                {
                    waveManager.WavePlus();
                    Destroy(gameObject);
                }
                break;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "IronBlock" && leftcheck)
        {
            if (bossDashParticle != null)
                Destroy(bossDashParticle);
            if(stanParticle == null)
                stanParticle = Instantiate(origin_StanParticle,stanPoint.position + new Vector3(0,3,0),Quaternion.identity,transform);
            leftcheck = false;
            boss.GetComponent<Animator>().SetTrigger("stanTrigger");
            angle = 180;
            damegeflag = false;
            //StartCoroutine("Blink");
            rockflag = false;
            for (int count = 0; count < 3; count++)
            {
                RockInstantiate();
            }
            BC.enabled = false;
            Invoke("Release", 4.5f);//衝突時4.5秒間停止
        }

        if (col.gameObject.tag == "IronBlock" && rightcheck)
        {
            if (bossDashParticle != null)
                Destroy(bossDashParticle);
            if (stanParticle == null)
                stanParticle = Instantiate(origin_StanParticle, stanPoint.position + new Vector3(0, 3, 0), Quaternion.identity, transform);
            rightcheck = false;
            boss.GetComponent<Animator>().SetTrigger("stanTrigger");
            angle = -180;
            damegeflag = false;
            //StartCoroutine("Blink");
            rockflag = true;
            for (int count = 0; count < 3; count++)
            {
                RockInstantiate();
            }
            BC.enabled = false;
            Invoke("Release", 4.5f);//衝突時4.5秒間停止
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (LayerMask.LayerToName(col.gameObject.layer) == "ThrowEnemy")
        {
            hitCount--;
            Debug.Log(hitCount);
            if (hitCount > 0)
                boss.GetComponent<Animator>().SetTrigger("damegeTrigger");
            Instantiate(bossBombParticle, bombPoint.position, Quaternion.identity);
            bossState = BossState.Damege;
            Destroy(col.gameObject);
        }
    }

    //停止時処理
    void Release()
    {
        BC.enabled = true;
        //StopCoroutine("Blink");
        //var renderComponent = GetComponent<Renderer>();
        //renderComponent.enabled = true;
        transform.Rotate(new Vector3(0, angle, 0));
        time = 0;
        if (stanParticle != null)
            Destroy(stanParticle);
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

    public BossState BossStateReturn()
    {
        return bossState;
    }

    //画面に表示時のみに処理を行うなら
    //void OnBecameInvisible()
    //{
    //    playerCheck = false;
    //}
    //void OnBecameVisible()
    //{
    //    playerCheck = true;
    //}
}