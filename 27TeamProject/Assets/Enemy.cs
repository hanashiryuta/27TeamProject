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

public class Enemy : MonoBehaviour {

    [SerializeField]
    float speed; //移動スピード
    [SerializeField]
    int inputHp; //HPの初期設定用
    [SerializeField]
    int hp; //処理で使用するHP変数

    bool isTurn; //進行方向変更用フラグ
    RaycastHit[] hitList; //BoxCastでHitしたものを入れる変数
    Vector3 origin; //Boxcast開始地点
    Vector3 boxcastScale; //BoxCastをどのEnemyの中点からどの距離で生成するか

    [SerializeField]
    LayerMask layerMask; //Boxcastで使用するレイヤー指定用
    [SerializeField]
    GameObject hook; //フック
    [SerializeField]
    float BlowOffSpeed; //吹き飛ぶスピード
    [HideInInspector]
    public bool isHook; //フックに捕まっているかの判定

    bool isBlow; //吹き飛ぶ時の方向判定

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

    public Animator animator;

    Vector3 currentPosition;
    Vector3 previousePositoin;

    float flyDeathTime = 2.0f;
    [HideInInspector]
    public bool isFly;

    public virtual void Awake()
    {
        mode = MoveMode.RANDOMMOVE;
        //mode = Enum.GetValues(typeof(MoveMode)).Cast<MoveMode>().OrderBy(c => UnityEngine.Random.Range(0, 3)).FirstOrDefault();
        wavewall = GameObject.FindGameObjectsWithTag("IronBlock");
        x = UnityEngine.Random.Range(wavewall[1].transform.position.x - 1, wavewall[3].transform.position.x + 1);
        z = UnityEngine.Random.Range(wavewall[1].transform.position.z + 1, wavewall[2].transform.position.z - 1);
    }

    // Use this for initialization
    public virtual void Start()
    {
        isTurn = false;
        hp = inputHp;
        isHook = true;
        BlowMode = false;
        GUITime = origin_GUITime;
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
            Instantiate(origin_Death_Particle, transform.position, Quaternion.identity);
            if (waveManager.isWave)
                waveManager.enemyDeathNum++;
            Destroy(this.gameObject);
        }
        if (isSlap)
            Slap();

        if (mode == MoveMode.PLAYERCHASE || mode == MoveMode.RANDOMMOVE)
        {
            return;
        }
        else
        {
            switch (mode)
            {
                case MoveMode.VERTICAL:
                    VerticalBoxCast();
                    break;

                case MoveMode.HORIZONTAL:
                    HorizontalBoxCast();
                    break;
            }
        }
        Debug.Log(isTurn);
    }

    private void Slap()
    {
        Instantiate(slap_Circle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void VerticalBoxCast()
    {
        if (!isTurn)
        {
            origin = new Vector3(transform.position.x, transform.position.y, transform.position.z + transform.lossyScale.z / 2);
            hitList = Physics.BoxCastAll(origin, boxcastScale, -transform.up, Quaternion.identity, transform.lossyScale.y, layerMask);
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
                isTurn = !isTurn;
            }
        }
        else if (isTurn)
        {
            origin = new Vector3(transform.position.x, transform.position.y, transform.position.z - transform.lossyScale.z / 2);
            hitList = Physics.BoxCastAll(origin, boxcastScale, -transform.up, Quaternion.identity, transform.lossyScale.y, layerMask);
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
                isTurn = !isTurn;
            }
        }
    }

    void HorizontalBoxCast()
    {
        if (!isTurn)
        {
            origin = new Vector3(transform.position.x + transform.lossyScale.x / 2, transform.position.y, transform.position.z);
            hitList = Physics.BoxCastAll(origin, boxcastScale, -transform.up, Quaternion.identity, transform.lossyScale.y, layerMask);
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
                isTurn = !isTurn;
            }
        }
        else if (isTurn)
        {
            origin = new Vector3(transform.position.x - transform.lossyScale.x / 2, transform.position.y, transform.position.z);
            hitList = Physics.BoxCastAll(origin, boxcastScale, -transform.up, Quaternion.identity, transform.lossyScale.y, layerMask);
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
                isTurn = !isTurn;
            }
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
            if (!isTurn)
            {
                switch (mode)
                {
                    case MoveMode.HORIZONTAL:
                        TurnHorizontal();
                        break;

                    case MoveMode.VERTICAL:
                        TurnVertical();
                        break;
                }
            }
            else if (isTurn)
            {
                switch (mode)
                {
                    case MoveMode.HORIZONTAL:
                        reTurnHorizontal();
                        break;

                    case MoveMode.VERTICAL:
                        reTurnVertical();
                        break;
                }
            }
        }
        currentPosition = transform.position;
        Vector3 direction = currentPosition - previousePositoin;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);

        if (direction.x > 0)
        {
            scale.x *= -1;
        }
        transform.localScale = scale;

        previousePositoin = currentPosition;
    }

    void TurnHorizontal()
    {
        transform.position += new Vector3(speed, 0, 0);

    }

    void reTurnHorizontal()
    {
        transform.position -= new Vector3(speed, 0, 0);
    }

    void TurnVertical()
    {
        transform.position += new Vector3(0, 0, speed);
    }

    void reTurnVertical()
    {
        transform.position -= new Vector3(0, 0, speed);
    }

    void PlayerShaseMove()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 pos = player.transform.position - transform.position;
        Vector3 normalpos = Vector3.Normalize(pos);
        if (Mathf.Abs(pos.x) < ChaseRange && Mathf.Abs(pos.z) < ChaseRange)
            transform.position += normalpos * speed;
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

    public void Blow()
    {
        if (isBlow)
        {
            BackBlow();
        }
        else
        {
            FrontBlow();
        }
    }

    void BackBlow()
    {
        transform.position -= new Vector3(BlowOffSpeed, 0, BlowOffSpeed);
    }

    void FrontBlow()
    {
        transform.position += new Vector3(BlowOffSpeed, 0, BlowOffSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {

        //if (collision.gameObject.layer == 12)
        //{
        //    BlowMode = true;
        //    //GetComponent<Rigidbody>().useGravity = false;
        //    Vector3 Pos = transform.position - collision.transform.position;
        //    if(Pos.z > 0)
        //    {
        //        isBlow = true;
        //    }
        //    if(Pos.z < 0)
        //    {
        //        isBlow = false;
        //    }
        //    hp -= collision.gameObject.GetComponent<Enemy>().SwingAttack;
        //}
        //else 
        if (collision.gameObject.layer == 15)
        {
            GUIText = collision.gameObject.GetComponent<Enemy>().ThrowAttack.ToString();
            isGUIDraw = true;
            hp -= collision.gameObject.GetComponent<Enemy>().ThrowAttack;
            Instantiate(origin_Damege_Particle, transform.position, Quaternion.identity);
        }
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
        if (other.gameObject.layer == 12)
        {
            //BlowMode = true;
            //GetComponent<Rigidbody>().useGravity = false;
            Vector3 Pos = transform.position - other.transform.position;
            if (Pos.z > 0)
            {
                isBlow = true;
            }
            if (Pos.z < 0)
            {
                isBlow = false;
            }
            GUIText = other.gameObject.GetComponent<Enemy>().SwingAttack.ToString();
            isGUIDraw = true;
            hp -= other.gameObject.GetComponent<Enemy>().SwingAttack;
            Instantiate(origin_Damege_Particle, transform.position, Quaternion.identity);
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
