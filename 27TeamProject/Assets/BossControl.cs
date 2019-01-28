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

    private bool wallHit;
    private float dashDirection;

    [HideInInspector]
    public WaveManager waveManager;
    public EnemySpawnManager enemySpawnManager;
    public BGMManager bgmManager;
    
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
        BC = this.GetComponent<BossControl>();
        playerCheck = false;
        rockflag = false;
        anime = GetComponent<Animator>();
        runflag = false;
        damegeflag = false;
        isHook = true;
        animeStop = false;
        wallHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerCheck)
        {
            Move();
        }
        if(enemySpawnManager.isBossSpawn == true)
        {
            bgmManager.Boss_BGM();
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

                //プレイヤーが左側
                if (target.x < this.transform.position.x)
                {
                    rockflag = false;
                    this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    dashDirection = 90;
                }

                //プレイヤーが右側
                else if (target.x > this.transform.position.x)
                {
                    rockflag = true;
                    this.transform.rotation = Quaternion.Euler(0.0f, -180.0f, 0.0f);
                    dashDirection = -90;
                }

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
                ////移動処理
                if(!wallHit)
                {
                    wallHit = true;
                }
                float trans = -moveSpeed * Time.deltaTime;
                transform.Translate(new Vector3(trans, 0));
                if (bossDashParticle == null)
                    bossDashParticle = Instantiate(origin_BossDashParticle, dashPoint.position, Quaternion.Euler(0, 0, dashDirection), transform);
                    break;

            case BossState.Damege:
                if (!damegeflag)
                {
                    trans = 0;
                    time = 0;
                    damegeflag = true;
                    runflag = false;
                }
                transform.position = transform.position;

                if (hitCount == 0)
                {
                    bossState = BossState.Dawn;
                }

                else if (time > 1)
                {
                    bossState = BossState.Move;
                }
                break;

            case BossState.Dawn:
                if (!animeStop)
                {
                    boss.GetComponent<Animator>().SetTrigger("dawnTrigger");
                    animeStop = true;
                }
                if (time > 2)
                {
                    boss.GetComponent<Animator>().speed = 0;
                }
                if (!isHook)
                {
                    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                }
                break;
            case BossState.Fly:
                flyTime += Time.deltaTime;
                if (flyTime > origin_FlyTime)
                {
                    waveManager.WavePlus();
                    Destroy(gameObject);
                }
                break;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "IronBlock" && wallHit)
        {
            if (bossDashParticle != null)
                Destroy(bossDashParticle);
            if (stanParticle == null)
                stanParticle = Instantiate(origin_StanParticle, stanPoint.position + new Vector3(0, 3, 0), Quaternion.identity, transform);
            wallHit = false;
            boss.GetComponent<Animator>().SetTrigger("stanTrigger");
            damegeflag = false;
            //StartCoroutine("Blink");
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
            if (bossDashParticle != null)
                Destroy(bossDashParticle);
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