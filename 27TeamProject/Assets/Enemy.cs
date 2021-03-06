﻿//
//10月3日
//作成者：安部崇寛
//エネミークラス
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum MoveMode
{
    HORIZONTAL,
    PLAYERCHASE,
    RANDOMMOVE,
    VERTICAL,
    ESCAPE,
}

public enum Status
{
    NORMAL,
    DAMEGE,
}

public enum HookStart
{
    Left,
    Right,
}

public class Enemy : MonoBehaviour
{

    [SerializeField]
    float speed; //移動スピード
    [SerializeField]
    public int inputHp; //HPの初期設定用
    [HideInInspector]
    public int hp; //処理で使用するHP変数

    Vector3 velosity;

    RaycastHit[] hitList; //BoxCastでHitしたものを入れる変数
    Vector3 origin; //Boxcast開始地点
    Vector3 boxcastRange; //BoxCastをどのEnemyの中点からどの距離で生成するか

    [SerializeField]
    LayerMask layerMask; //Boxcastで使用するレイヤー指定用
    [SerializeField]
    GameObject hook; //フック
    public float BlowOffSpeed; //吹き飛ぶスピード
    [HideInInspector]
    public bool isHook; //フックに捕まっているかの判定

    [HideInInspector]
    public bool isCatch = true;
    
    public bool isSticking = true;
    [HideInInspector]
    public bool BlowMode; //吹き飛ぶ前と後の切り替え用

    public int maxThrowAttack;
    public int maxSwingAttack;

    [SerializeField]
    public int ThrowAttack; //投げられた時のEnemyの攻撃力
    [SerializeField]
    public int SwingAttack;
    [SerializeField]
    MoveMode mode; //移動設定
    [SerializeField]
    float ChaseRange; //Player追跡距離

    [SerializeField]
    GameObject[] wavewall;
    [HideInInspector]
    public float x, z;

    public GameObject slap_Circle;

    [HideInInspector]
    public bool isSlap;

    [HideInInspector]
    public WaveManager waveManager;

    public GameObject origin_Damege_Particle;
    public GameObject origin_Death_Particle;
    
    [HideInInspector]
    public float angleZ;
    
    [HideInInspector]
    public Vector3 PosBlow;
    
    public float DamageSetTime;
    float DamageTime;
    protected Status status;

    protected int ThisEnemyLayer;
    protected int CatchEnemyLayer;
    protected int ThrowEnemyLayer;
    protected int GroundLayer;

    public Animator animator;

    Vector3 currentPosition;
    Vector3 previousePositoin;

    public float originFlyDeathTime = 2.0f;
    [HideInInspector]
    public float flyDeathTime;
    [HideInInspector]
    public bool isFly;
    
    float animAngle = 180;
    [HideInInspector]
    public EnemySpawnManager enemySpawnManager;

    public float playerSP;//プレイヤーが消費するSP

    [HideInInspector]
    public bool moveStop;

    [HideInInspector]
    public bool isEscape;
    public float EscapeSpeed;
    public float setEscapeDelayTime;
    [HideInInspector]
    public float EscapeDilayTime;
    [HideInInspector]
    public bool isGround;
    public string debug;

    public List<AudioClip> seList;
    protected AudioSource seAudio;

    HookStart start;
    bool isLeft;
    bool setStart;
    bool Change;

    public float attack;

    public virtual void Awake()
    {
        AwakeSub();
    }

    public virtual void AwakeSub()
    {
        //mode = MoveMode.RANDOMMOVE;
        //mode = Enum.GetValues(typeof(MoveMode)).Cast<MoveMode>().OrderBy(c => UnityEngine.Random.Range(0, 3)).FirstOrDefault();
        int random = UnityEngine.Random.Range(0, 2);
        if (random == 0)
        {
            //mode = MoveMode.RANDOMMOVE;
            mode = MoveMode.HORIZONTAL;
        }
        else if (random == 1)
        {
            mode = MoveMode.PLAYERCHASE;
        }
        wavewall = GameObject.FindGameObjectsWithTag("IronBlock");
        x = UnityEngine.Random.Range(wavewall[0].transform.position.x - 1, wavewall[3].transform.position.x + 1);
        z = UnityEngine.Random.Range(wavewall[0].transform.position.z + 1, wavewall[1].transform.position.z - 1);
    }

    // Use this for initialization
    public virtual void Start () {
        StartSub();
    }

    public virtual void StartSub()
    {
        hp = inputHp;
        isHook = true;
        BlowMode = false;
        setStart = false;
        Change = false;
        angleZ = transform.rotation.x;
        status = Status.NORMAL;
        ThisEnemyLayer = LayerMask.NameToLayer("Enemy");
        CatchEnemyLayer = LayerMask.NameToLayer("CatchEnemy");
        ThrowEnemyLayer = LayerMask.NameToLayer("ThrowEnemy");
        GroundLayer = LayerMask.NameToLayer("Ground");
        
        Debug.Log(ThrowEnemyLayer);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 pos = player.transform.position - transform.position;
        Vector3 normalpos = Vector3.Normalize(pos);

        switch (mode)
        {
            case MoveMode.HORIZONTAL:
                velosity = new Vector3(1 * normalpos.x, 0, 0);
                break;
            case MoveMode.VERTICAL:
                velosity = new Vector3(0, 0, 1 * normalpos.z);
                break;
            case MoveMode.PLAYERCHASE:
                velosity = new Vector3(1 * normalpos.x, 0, 0);
                break;
            case MoveMode.ESCAPE:
                velosity = new Vector3(1 * normalpos.x, 0, 0);
                break;
        }

        GUITime = origin_GUITime;

        moveStop = false;
        DamageTime = DamageSetTime;
        flyDeathTime = originFlyDeathTime;
        seAudio = gameObject.AddComponent<AudioSource>();
        isEscape = false;
        EscapeDilayTime = setEscapeDelayTime;
        isGround = false;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        UpdateSub();
    }

    public virtual void UpdateSub()
    {
        if (isGUIDraw)
        {
            GUITime -= Time.deltaTime;
            if (GUITime < 0)
            {
                GUITime = origin_GUITime;
                isGUIDraw = false;
            }
        }

        if (isFly)
            flyDeathTime -= Time.deltaTime;

        if ((/*hp < 1 ||*/ Mathf.Abs(transform.position.z) > 50) || !waveManager.isWave || flyDeathTime < 0)
        {
            DeathAction();
        }
        if (isSlap)
            Slap();
        switch (status)
        {
            case Status.DAMEGE:
                DamageTime -= Time.deltaTime;
                if(DamageTime < 0)
                {
                    DamageTime = DamageSetTime;
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    moveStop = !moveStop;
                    status = Status.NORMAL;
                }
                break;

            case Status.NORMAL:
                if(mode != MoveMode.RANDOMMOVE) BoxCast();
                break;
        }
    }

    public virtual void DeathAction()
    {
        Instantiate(origin_Death_Particle, transform.position, Quaternion.identity);
        if (waveManager.isWave)
            waveManager.enemyDeathNum++;
        enemySpawnManager.enemyCount--;
        Destroy(this.gameObject);
    }

    private void Slap()
    {
        Instantiate(slap_Circle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void BoxCast()
    {
        origin = transform.position + new Vector3(transform.localScale.x * velosity.x, transform.localScale.y * velosity.y, transform.localScale.z * velosity.z) / 2;
        //boxcastRange = transform.localScale / 2;
        hitList = Physics.BoxCastAll(origin, boxcastRange, -transform.up, Quaternion.identity, transform.lossyScale.y, layerMask);
        Debug.DrawRay(origin, -transform.up);
        
        int groundcount = 0;
        foreach (var hl in hitList)
        {
            if (hl.transform.tag == "Ground")
            {
                groundcount++;
                isGround = false;
            }
        }

        if (groundcount == 0)
        {
            velosity *= -1;
            isGround = true;
        }
    }

    public void Move()
    {
        if (moveStop) return;

        if (mode == MoveMode.RANDOMMOVE)
        {
            RandomMove();
        }

        if(mode == MoveMode.ESCAPE)
        {
            EscapeMove();
        }

        if (mode == MoveMode.PLAYERCHASE)
        {
            PlayerChaseMove();
        }
        else
        {
            transform.position += velosity * speed;
        }
        currentPosition = transform.position;
        Vector3 direction = currentPosition - previousePositoin;

        Vector3 scale = transform.localScale;
        Quaternion rota = transform.rotation;
        scale.x = Mathf.Abs(scale.x);
        rota.y = 0;

        if (direction.x > 0)
        {
            //scale.x *= -1;
            //transform.rotation = Quaternion.Euler(0, animAngle * -1, 0);
            rota.y = 180;
        }
        transform.localScale = scale;
        transform.rotation = rota;

        previousePositoin = currentPosition;
    }

    void PlayerChaseMove()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 pos = player.transform.position - transform.position;
        Vector3 normalpos = Vector3.Normalize(pos);
        if (Mathf.Abs(pos.x) < ChaseRange && Mathf.Abs(pos.z) < ChaseRange)
        {
            transform.position += normalpos * speed;
        }
        else
        {
            transform.position += velosity * speed;
        }
    }

    void RandomMove()
    {
        Vector3 pos = new Vector3(x, transform.position.y, z) - transform.position;
        float distance = Vector3.Distance(new Vector3(x, transform.position.y, z), transform.position);
        if (Math.Abs(distance) < 0.1f)
        {
            x = UnityEngine.Random.Range(wavewall[0].transform.position.x - 1, wavewall[3].transform.position.x + 1);
            z = UnityEngine.Random.Range(wavewall[0].transform.position.z + 1, wavewall[1].transform.position.z - 1);
        }
        pos = new Vector3(x, transform.position.y, z) - transform.position;
        Vector3 normalpos = Vector3.Normalize(pos);
        transform.position += normalpos * speed;
    }

    void EscapeMove()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 pos = player.transform.position - transform.position;
        Vector3 normalpos = Vector3.Normalize(pos);
        if (Mathf.Abs(pos.x) < ChaseRange && Mathf.Abs(pos.z) < ChaseRange)
        {
            isEscape = true;
        }

        if (isEscape)
        {
            EscapeDilayTime -= Time.deltaTime;
        }
        else
        {
            transform.position += velosity * speed;
        }

        if(EscapeDilayTime < 0)
        {
            normalpos = new Vector3(normalpos.x, 0, 0);
            transform.position += -normalpos * EscapeSpeed;
        }
    }

    public virtual void Blow()
    {
        Vector3 normal = Vector3.Normalize(PosBlow);
        transform.position += new Vector3(BlowOffSpeed * normal.x, BlowOffSpeed, BlowOffSpeed * normal.z);
        angleZ += 10;
        Quaternion q = transform.rotation;
        q = Quaternion.Euler(0,0,angleZ);
        transform.rotation = q;// Quaternion.Euler(0, 0, angleZ);
    }
    public virtual void ThrowSet(float throwSpeed, Vector3 throwVelocity)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().useGravity = false;
            transform.position = new Vector3(transform.position.x + throwVelocity.x * 2, 3, transform.position.z + throwVelocity.z * 2);
            GetComponent<Rigidbody>().AddForce(throwVelocity * throwSpeed);
        }
        gameObject.layer = ThrowEnemyLayer;
        GetComponent<BoxCollider>().isTrigger = true;
        GetComponent<Enemy>().isFly = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Slap_Circle"))
        {
            Vector3 slapVelocity = transform.position - collision.gameObject.transform.position;
            GetComponent<Rigidbody>().AddForce(slapVelocity.normalized * 400);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            AttackAnime();
        }
    }

    public virtual void AttackAnime()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerAction(other);
    }

    public virtual void TriggerAction(Collider other)
    {
        if (other.gameObject.layer == CatchEnemyLayer)
        {
            GUIText = other.gameObject.GetComponent<Enemy>().SwingAttack.ToString();
            isGUIDraw = true;
            hp -= other.gameObject.GetComponent<Enemy>().SwingAttack;
            Instantiate(origin_Damege_Particle, transform.position, Quaternion.identity);
            status = Status.DAMEGE;
            TriggerSetRotate();
            moveStop = !moveStop;

            TriggerSet(other);

        }

        if (other.gameObject.layer == ThrowEnemyLayer)
        {
            GUIText = other.gameObject.GetComponent<Enemy>().ThrowAttack.ToString();
            isGUIDraw = true;
            hp -= other.gameObject.GetComponent<Enemy>().ThrowAttack;
            Instantiate(origin_Damege_Particle, transform.position, Quaternion.identity);
            status = Status.DAMEGE;
            TriggerSetRotate();
            moveStop = !moveStop;
            Physics.IgnoreCollision(other.gameObject.GetComponent<BoxCollider>(), GetComponent<BoxCollider>());

            TriggerSet(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == ThrowEnemyLayer)
            Physics.IgnoreCollision(other.gameObject.GetComponent<BoxCollider>(), GetComponent<BoxCollider>(), false);
    }

    public virtual void TriggerSet(Collider other)
    {
        if (hp <= 0)
        {
            BlowMode = true;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
            PosBlow = transform.position - other.transform.position;
            seAudio.PlayOneShot(seList[1]);
        }
        else
        {
            seAudio.PlayOneShot(seList[2]);
        }
    }

    public virtual void TriggerSetRotate()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -30));
    }

    public virtual void HookSwing()
    {   
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Vector3 pos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z) - transform.position;
        Vector3 normal = Vector3.Normalize(pos);
        if (normal.x > 0)
        {
            isLeft = true;
            if (!setStart)
            {
                start = HookStart.Left;
                Change = false;
                setStart = true;
            }
        }
        else
        {
            isLeft = false;
            if (!setStart)
            {
                start = HookStart.Right;
                Change = true;
                setStart = true;
            }
        }

        if (isLeft)
        {
            switch (start)
            {
                case HookStart.Left:
                    if (Change == isLeft)
                    {
                        Change = false;
                        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + 180, 0);
                    }
                    break;
                case HookStart.Right:
                    if (Change == isLeft)
                    {
                        Change = false;
                        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + 180, 0);
                    }
                    break;
            }
        }
        else
        {
            switch (start)
            {
                case HookStart.Left:
                    if (Change == isLeft)
                    {
                        Change = true;
                        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + 180, 0);
                    }
                    break;
                case HookStart.Right:
                    if (Change == isLeft)
                    {
                        Change = true;
                        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + 180, 0);
                    }
                    break;
            }
        }
    }

    protected Vector2 GUIPosition;
    public Font GUIFont;
    protected bool isGUIDraw;
    protected float GUITextArpha = 1.0f;
    protected string GUIText;
    public float origin_GUITime = 0.5f;
    protected float GUITime;

    private void TextDraw(Vector2 position, int fontSize, Color color, string text, float arpha)
    {
        GUIStyle guiStyle = new GUIStyle();
        GUIStyleState styleState = new GUIStyleState();
        guiStyle.font = GUIFont;
        guiStyle.fontSize = fontSize;
        styleState.textColor = color;
        guiStyle.normal = styleState;
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, arpha);
        GUI.Label(new Rect(position, new Vector2(100, 100)), text, guiStyle);
    }

    private void OnGUI()
    {
        if (isGUIDraw)
        {
            TextDraw(Camera.main.WorldToScreenPoint(transform.position), 110, Color.black, GUIText, GUITextArpha);
            TextDraw(Camera.main.WorldToScreenPoint(transform.position), 100, Color.red, GUIText, GUITextArpha);
        }
    }
}
