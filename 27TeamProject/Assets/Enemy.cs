//
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
    VERTICAL,
    HORIZONTAL,
    PLAYERCHASE,
    RANDOMMOVE,
}

enum Status
{
    NORMAL,
    DAMEGE,
}

public class Enemy : MonoBehaviour
{

    [SerializeField]
    float speed; //移動スピード
    [SerializeField]
    public int inputHp; //HPの初期設定用
    [SerializeField]
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

    GameObject[] wavewall;
    float x, z;

    public GameObject slap_Circle;

    public GameObject enemy3;

    [HideInInspector]
    public bool isSlap;

    [HideInInspector]
    public WaveManager waveManager;

    public GameObject origin_Damege_Particle;
    public GameObject origin_Death_Particle;
    
    public float angleZ;

    float angleX;

    [HideInInspector]
    public Vector3 PosBlow;
    
    float throwSetTime = 1;
    float throwTime;

    Status status;

    int ThisEnemyLayer;
    int CatchEnemyLayer;
    int ThrowEnemyLayer;

    public Animator animator;

    Vector3 currentPosition;
    Vector3 previousePositoin;

    public float originFlyDeathTime = 2.0f;
    [HideInInspector]
    public float flyDeathTime;
    [HideInInspector]
    public bool isFly;

    [HideInInspector]
    public EnemySpawnManager enemySpawnManager;

    public virtual void Awake()
    {
        //mode = MoveMode.RANDOMMOVE;
        //mode = Enum.GetValues(typeof(MoveMode)).Cast<MoveMode>().OrderBy(c => UnityEngine.Random.Range(0, 3)).FirstOrDefault();
        int random = UnityEngine.Random.Range(0, 2);
        if (random == 0)
        {
            mode = MoveMode.RANDOMMOVE;
        }
        else if (random == 1)
        {
            mode = MoveMode.PLAYERCHASE;
        }
        wavewall = GameObject.FindGameObjectsWithTag("IronBlock");
        x = UnityEngine.Random.Range(wavewall[1].transform.position.x - 1, wavewall[3].transform.position.x + 1);
        z = UnityEngine.Random.Range(wavewall[1].transform.position.z + 1, wavewall[2].transform.position.z - 1);
    }

    // Use this for initialization
    public virtual void Start()
    {
        hp = inputHp;
        isHook = true;
        BlowMode = false;
        angleZ = transform.rotation.x;
        status = Status.NORMAL;
        ThisEnemyLayer = LayerMask.NameToLayer("Enemy");
        CatchEnemyLayer = LayerMask.NameToLayer("CatchEnemy");
        ThrowEnemyLayer = LayerMask.NameToLayer("ThrowEnemy");

        switch (mode)
        {
            case MoveMode.HORIZONTAL:
                velosity = new Vector3(1, 0, 0);
                break;
            case MoveMode.VERTICAL:
                velosity = new Vector3(0, 0, 1);
                break;
            case MoveMode.PLAYERCHASE:
                velosity = new Vector3(1, 0, 0);
                break;
        }

        GUITime = origin_GUITime;
        flyDeathTime = originFlyDeathTime;
    }

    // Update is called once per frame
    public virtual void Update()
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
        if ((hp < 1 || Mathf.Abs(transform.position.z) > 50) || !waveManager.isWave || flyDeathTime < 0)
        {
            DeathAction();
        }
        if (isSlap)
            Slap
        switch (status)
        {
            case Status.DAMEGE:
                throwTime -= Time.deltaTime;
                if (throwTime < 0)
                {
                    status = Status.NORMAL;
                }

                break;

            case Status.NORMAL:
                BoxCast();
                break;
        }
    }

    void DeathAction()
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
        origin = transform.position + velosity * transform.localScale.x / 2;
        hitList = Physics.BoxCastAll(origin, boxcastRange, -transform.up, Quaternion.identity, transform.lossyScale.y, layerMask);
        Debug.DrawRay(origin, -transform.up);

        int groundcount = 0;
        foreach (var hl in hitList)
        {
            if (hl.transform.tag == "Ground")
            {
                groundcount++;
            }
        }
        if (groundcount == 0)
        {
            velosity *= -1;
        }
    }

    public void Move()
    {
        if (mode == MoveMode.RANDOMMOVE)
        {
            RandomMove();
        }

        if (mode == MoveMode.PLAYERCHASE)
        {
            PlayerShaseMove();
        }
        else
        {
            transform.position += velosity * speed;
        }
        currentPosition = transform.position;
        Vector3 direction = currentPosition - previousePositoin;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);

        if (direction.x > 0)
        {
            scale.x *= 1;
        }
        transform.localScale = scale;

        previousePositoin = currentPosition;
    }

    void PlayerShaseMove()
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
        Vector3 pos = new Vector3(x, 0.0f, z) - transform.position;
        Vector3 normalpos = Vector3.Normalize(pos);
        if ((pos.z < 0.1f && pos.z > -0.1f) || (pos.x < 0.1f && pos.x > -0.1f))
        {
            x = UnityEngine.Random.Range(wavewall[1].transform.position.x - 1, wavewall[3].transform.position.x + 1);
            z = UnityEngine.Random.Range(wavewall[1].transform.position.z + 1, wavewall[2].transform.position.z - 1);
        }
        transform.position += normalpos * speed;
    }

    public virtual void Blow()
    {   
        Vector3 normal = Vector3.Normalize(PosBlow);
        transform.position += new Vector3(BlowOffSpeed * normal.x, BlowOffSpeed, BlowOffSpeed * normal.z);
        angleZ += 10;
        transform.rotation = Quaternion.Euler(0, 0, angleZ);
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
        GetComponent<BoxCollider>().isTrigger = false;
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
            animator.SetTrigger("isAttack");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == CatchEnemyLayer)
        {
            GUIText = other.gameObject.GetComponent<Enemy>().SwingAttack.ToString();
            isGUIDraw = true;
            Instantiate(origin_Damege_Particle, transform.position, Quaternion.identity);
            
            TriggerSet(other);
            
        }

        if (other.gameObject.layer == ThrowEnemyLayer)
        {
            GUIText = other.gameObject.GetComponent<Enemy>().ThrowAttack.ToString();
            isGUIDraw = true;
            hp -= other.gameObject.GetComponent<Enemy>().ThrowAttack;
            Instantiate(origin_Damege_Particle, transform.position, Quaternion.identity);
            status = Status.DAMEGE;
            throwTime = throwSetTime;
            Physics.IgnoreCollision(other.gameObject.GetComponent<BoxCollider>(), GetComponent<BoxCollider>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == ThrowEnemyLayer)
            Physics.IgnoreCollision(other.gameObject.GetComponent<BoxCollider>(), GetComponent<BoxCollider>(), false);
    }

    public virtual void TriggerSet(Collider other)
    {
        hp -= other.gameObject.GetComponent<Enemy>().SwingAttack;
        if (hp <= 5)
        {
            BlowMode = true;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
            PosBlow = transform.position - other.transform.position;
        }
    }

    Vector2 GUIPosition;
    public Font GUIFont;
    bool isGUIDraw;
    float GUITextArpha = 1.0f;
    string GUIText;
    public float origin_GUITime = 0.5f;
    float GUITime;

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
