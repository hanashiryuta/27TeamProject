﻿///
///10/19
///葉梨竜太
///プレイヤークラス
///
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// プレイヤー状態
/// </summary>
public enum PlayerState
{
    NORMALMOVE,//移動
    HOOKMOVE,//フック移動
    HOOKSWING,//フック振り回し
    HOOKRETURN,//フック戻り
}

public class Player : MonoBehaviour
{
    public float hp = 10;
    //リジッドボディ
    Rigidbody rigid;
    //移動スピード
    public float moveSpeed;
    //移動強さ
    public float moveForceMultiplier = 50;
    //ジャンプパワー
    public float jumpPower;
    //ジャンプできるかどうか
    bool isJumpFlag;
    //プレイヤー状態
    [HideInInspector]
    public PlayerState playerState = PlayerState.NORMALMOVE;
    //フック
    public GameObject originHook;
    GameObject hook;
    //フックポインター
    public GameObject hookPointer;
    //ポインター半径
    public float pointerRadius;
    //ポインター角度
    float pointerAngle;
    //フック半径
    public float shotRadius;
    //フック打てるか
    [HideInInspector]
    public bool isHookShot = true;

    //当たった場所
    [HideInInspector]
    public Vector3 hitPosition;
    //当たった場所までの距離
    [HideInInspector]
    public float hitDistance;

    //つかんだもの
    GameObject catchObject;
    //回す半径
    float swingRadius;
    //回す角度
    float swingAngle;
    //回すスピード
    public float swingSpeed;
    //回すスピード限界値
    public float swingSpeedRange;
    //回すスピード上昇値
    public float swingSpeedRate;
    //投げるスピード
    public float throwSpeed;
    //フック移動スピード
    public float flySpeed;
    //たたきつけスピード
    public float slapSpeed;
    //たたきつけパーティクル
    public GameObject slap_Particle;
    Vector3 pointerPosition = Vector3.zero;
    public float origin_TimingTime;
    float timingTime;

    public float swingButtonRate;
    public GameObject origin_Timing_Particle;
    GameObject timing_Particle = null;
    public GameObject good_Timing_Particle;
    public GameObject badTiming_Particle;
    
    //ポインター用レイヤー
    public LayerMask targetLayer;

    Animator anim;
    bool isPlayerDeath = false;
    public float origin_DamegeTime;
    float damegeTime;

    public float sp;
    float spLossRange = 0.1f;
    float maxHP;
    [HideInInspector]
    public float maxSP;

    public Image hpBar;
    public Image spBar;

    public GameObject origin_Swing_Particle;
    GameObject swing_Particle;

    bool isDamege;

    string shotInput = "Hook";
    string timingInput = "Fire2";

    public enum SwingState
    {
        TRIGGERSWING,
        STICKSWING,
    }

    public SwingState swingState;

    public Gradient firstColor;
    public Gradient secondColor;
    public Gradient rainbow;
    
    public List<AudioClip> seList;
    AudioSource seAudio;

    float originSeTime = 0.5f;
    float seTime;

    public FadeScript fadeScript;
    public LayerMask bossLayer;
    public LayerMask rockLayer;

    public PauseManager pauseManager;

    Dictionary<float,GameObject> timingParticleList;
    // Use this for initialization
    void Start()
    {
        //取得
        rigid = GetComponent<Rigidbody>();
        timingTime = origin_TimingTime;
        damegeTime = origin_DamegeTime;
        anim = GetComponentInChildren<Animator>();
        maxHP = hp;
        maxSP = sp;
        seAudio = GetComponent<AudioSource>();
        timingParticleList = new Dictionary<float, GameObject>()
        {
            {1, good_Timing_Particle },
            {-1, badTiming_Particle },
        };
    }

    private void FixedUpdate()
    {
        if(Input.GetButtonDown("Pause"))
        {
            pauseManager.isPause = true;
        }

        hpBar.fillAmount = hp / maxHP;
        spBar.fillAmount = sp / maxSP;

        if (hp < maxHP * 3 / 10)
        {
            hpBar.color = Color.red;
        }
        else
        {
            spBar.color = Color.white;
        }

        if (sp < maxSP * 3 / 10)
        {
            spBar.color = Color.red;
        }
        else
        {
            spBar.color = Color.white;
        }

        damegeTime -= Time.deltaTime;
        if (damegeTime <= 0)
            isDamege = false;
        if (hp <= 0)
        {
            Death();
            return;
        }

        HookPointer();
        Move();
        anim.SetBool("isJump", !isJumpFlag);

        Vector3 scale = transform.localScale;
        scale.x = Input.GetAxis("Horizontal");
        if (scale.x >= 0)
            scale.x = 1.5f;
        else
            scale.x = -1.5f;
        transform.localScale = scale;

        //プレイヤーの状態で変化
        switch (playerState)
        {
            case PlayerState.NORMALMOVE://移動
                Jump();
                if(!isDamege)
                HookShot();
                //else if (Input.GetButtonUp("Jump"))
                //    hook.GetComponent<Hook>().hookState = HookState.RETURN;
                sp+=1f;
                if (sp >= maxSP)
                    sp = maxSP;
                break;
            case PlayerState.HOOKMOVE://フック移動
                //HookMove();
                break;
            case PlayerState.HOOKSWING://フック振り回し
                if (isJumpFlag)
                {
                    if (catchObject == null)
                    {
                        anim.SetBool("isCatch", false);
                        hook.GetComponent<Hook>().hookState = HookState.RETURN;
                        Destroy(swing_Particle);
                        playerState = PlayerState.HOOKRETURN;
                        break;
                    }
                    else
                    {
                        anim.SetBool("isCatch", true);
                        HookSwing();
                    }
                    if ((Input.GetButtonUp(shotInput) || sp <= 0))
                    {
                        Destroy(timing_Particle);
                        anim.SetBool("isCatch", false);
                        anim.SetTrigger("isThrow");
                        ObjectThrow();
                        hook.GetComponent<Hook>().hookState = HookState.RETURN;
                    }
                }
                else if(sp > 20)
                {
                    ObjectSlap();
                    hook.GetComponent<Hook>().hookState = HookState.RETURN;
                }
                else
                {
                    hook.GetComponent<Hook>().hookState = HookState.RETURN;
                    playerState = PlayerState.HOOKRETURN;
                }
                break;
            case PlayerState.HOOKRETURN://フック戻り
                anim.SetBool("isShot", false);
                Jump();
                break;
        }
    }

    public void Death()
    {
        anim.SetBool("isDeath", true);
        isPlayerDeath = true;
        fadeScript.nextScene = "GameOver";
        fadeScript.isSceneEnd = true;
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    public void Move()
    {
        Vector3 moveVector = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0, Input.GetAxis("Vertical") * moveSpeed);
        Vector3 rigidVelocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
        rigid.AddForce(moveForceMultiplier * (moveVector - rigidVelocity));
        anim.SetFloat("move", moveVector.sqrMagnitude);
        
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    public void Jump()
    {
        //if (testMoveState == TestMoveState.SIDEVIEW)
        //{
        //Bボタンでジャンプ
        if (isJumpFlag && Input.GetButtonDown(timingInput))
        {
            seAudio.PlayOneShot(seList[0]);
            isJumpFlag = false;
            rigid.AddForce(Vector2.up * jumpPower);
        }
        anim.SetFloat("jumpVelocity", rigid.velocity.y);
        //}
        //else
        //{

        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        //グラウンドに当たったら
        if (collision.gameObject.CompareTag("Ground"))
        {
            //フラグセット
            isJumpFlag = true;
        }
        if (collision.gameObject.CompareTag("Enemy") && damegeTime <= 0&&catchObject != collision.gameObject)
        {
            if (collision.gameObject.GetComponent<Enemy>().attack > 0)
            {
                isDamege = true;
                damegeTime = origin_DamegeTime;
                anim.SetTrigger("isDamege");
                hp -= collision.gameObject.GetComponent<Enemy>().attack;
            }
        }
        else if (collision.gameObject.CompareTag("Boss") && damegeTime <= 0 && catchObject != collision.gameObject)
        {
            isDamege = true;
            damegeTime = origin_DamegeTime;
            anim.SetTrigger("isDamege");
            hp -= 2;
        }
    }

    /// <summary>
    /// ポインター配置
    /// </summary>
    void HookPointer()
    {
        if (catchObject != null && catchObject.CompareTag("Boss"))
        {
            pointerAngle = Mathf.Atan2(1,0);
            pointerPosition = new Vector3(Mathf.Cos(pointerAngle) * pointerRadius, 2, Mathf.Sin(pointerAngle) * pointerRadius);
            hookPointer.transform.position = pointerPosition + transform.position;
        }
        else
        {
            //左スティックの方向にポインター配置
            if (Mathf.Abs(Input.GetAxis("Vertical")) >= 0.1f || Mathf.Abs(Input.GetAxis("Horizontal")) >= 0.1f)
            {
                pointerAngle = Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
                pointerPosition = new Vector3(Mathf.Cos(pointerAngle) * pointerRadius, 2, Mathf.Sin(pointerAngle) * pointerRadius);

            }

            if (!PointerBox(targetLayer) && !PointerBox(bossLayer) && !PointerBox(rockLayer))
            {
                hookPointer.transform.position = pointerPosition + transform.position;
                Color color = hookPointer.GetComponent<Renderer>().material.color;
                color = Color.white;
                hookPointer.GetComponent<Renderer>().material.color = color;
            }       
        }        
    }

    bool PointerBox(LayerMask layer)
    {
        //左スティックの方向に四角形のあたり判定を飛ばす
        Collider[] list = Physics.OverlapBox(new Vector3((transform.position.x + (transform.position.x + pointerPosition.x)) / 2, transform.position.y, (transform.position.z + (transform.position.z + pointerPosition.z)) / 2),
            new Vector3(transform.localScale.x, transform.localScale.y * 2, pointerRadius / 2),
            Quaternion.Euler(0, (pointerAngle - 90), 0), layer);

        //1つ以上検知していれば
        if (list.Length > 0)
        {
            //一番近いものを検索し、その位置にポインターを配置する
            GameObject nearEnemy = list[0].gameObject;
            foreach (var cx in list)
            {
                float length = Vector3.Distance(transform.position, cx.transform.position);
                float nearLength = Vector3.Distance(transform.position, nearEnemy.transform.position);

                if (length <= nearLength)
                {
                    nearEnemy = cx.gameObject;
                }
            }
            hookPointer.transform.position = nearEnemy.transform.position;
            Color color = hookPointer.GetComponent<Renderer>().material.color;
            color = Color.yellow;
            hookPointer.GetComponent<Renderer>().material.color = color;
            return true;
        }
        return false;
    }

    /// <summary>
    /// フック射出
    /// </summary>
    void HookShot()
    {        
        if (isHookShot && Input.GetButtonDown(shotInput))
        {
            //ポインターの位置に向かって射出
            hook = Instantiate(originHook, transform.position+new Vector3(0,2,0), Quaternion.identity);
            hook.GetComponent<Hook>().player = gameObject;
            hook.GetComponent<Hook>().targetPosition = hookPointer.transform.position;
            hook.GetComponent<Hook>().targetDistance = Vector3.Distance(hookPointer.transform.position, transform.position);
            isHookShot = false;
            anim.SetBool("isShot",true);
            seAudio.PlayOneShot(seList[3]);
        }
        
    }

    /// <summary>
    /// 回す準備
    /// </summary>
    /// <param name="m_CatchObject">回すオブジェクト</param>
    public void SwingSet(GameObject m_CatchObject)
    {
        catchObject = m_CatchObject;
        catchObject.GetComponent<BoxCollider>().isTrigger = true;
        catchObject.GetComponent<Rigidbody>().useGravity = false;
        if (!catchObject.CompareTag("Boss"))
        {
            catchObject.GetComponent<Enemy>().isFly = false;
            catchObject.GetComponent<Enemy>().flyDeathTime = catchObject.GetComponent<Enemy>().originFlyDeathTime;
        }
        swingRadius = Vector3.Distance(transform.position, catchObject.transform.position);
        if (swingRadius <= 1)
            swingRadius = 1;
        swingAngle = Mathf.Atan2(catchObject.transform.position.y - transform.position.y, catchObject.transform.position.x - transform.position.x) * 180 / Mathf.PI;
        playerState = PlayerState.HOOKSWING;
        swingSpeed = 0;
        swing_Particle = Instantiate(origin_Swing_Particle,catchObject.transform.position,Quaternion.identity,catchObject.transform);
        
    }

    //現在の角度
    float currentAngle = 0;
    //1フレーム前の角度
    float previouseAngle = 0;
    //回転方向
    float setAngle = 0;

    /// <summary>
    /// フック振り回し
    /// </summary>
    void HookSwing()
    {
        //現在の角度取得
        currentAngle = Mathf.Abs(Mathf.Atan2
            (Input.GetAxis("Vertical"),
            Input.GetAxis("Horizontal")) * 180.0f / Mathf.PI);

        //スティックの回転方向によってオブジェクトを回転させる
        if (currentAngle != previouseAngle) {
            if (SpinCheck(Input.GetAxisRaw("Vertical")))
                RightSpin();
            else
                LeftSpin();
        }

        //回転加速処理
        if(Math.Abs(setAngle) >= 360)
        {
            //方向指定
            float direction = setAngle / Math.Abs(setAngle);
            setAngle = 0;
            //回転スピードアップ
            swingSpeed += swingButtonRate * direction;
            Instantiate(timingParticleList[direction],
                new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, transform.position.z), 
                Quaternion.identity,
                transform);
        }

        //パーティクル色変更
        Gradient particleColor = firstColor;
        if (Mathf.Abs(swingSpeed) >= swingSpeedRange / 3)
        {
            if (Mathf.Abs(swingSpeed) >= swingSpeedRange * 2 / 3)
                particleColor = rainbow;
            else
                particleColor = secondColor;
        }

        //色設定
        ParticleSystem.MainModule mains = swing_Particle.GetComponent<ParticleSystem>().main;
        mains.startColor = particleColor;

        //上限設定
        if (swingSpeed >= swingSpeedRange)
            swingSpeed = swingSpeedRange;
        else if (swingSpeed <= -swingSpeedRange)
            swingSpeed = -swingSpeedRange;
       
        swingAngle += swingSpeed;
        //位置設定
        catchObject.transform.position = transform.position + 
            new Vector3(swingRadius * Mathf.Cos(swingAngle * Mathf.PI / 180),
            2,
            swingRadius * Mathf.Sin(swingAngle * Mathf.PI / 180));

        //SE設定
        seTime += Time.deltaTime;
        if(seTime >= originSeTime)
        {
            seTime = 0;
            seAudio.PlayOneShot(seList[2]);
        }

        //現在の角度に保存
        previouseAngle = currentAngle;  

        //ボスを掴んでいたら終了
        if (catchObject.CompareTag("Boss"))
            return;
        
        Enemy enemy = catchObject.GetComponent<Enemy>();

        //SP減少処理
        spLossRange = enemy.playerSP;
        sp -= spLossRange;
        if (sp <= 0)
            sp = 0;   
    }

    /// <summary>
    /// 回転方向判定
    /// </summary>
    /// <param name="vartical">スティックY軸入力</param>
    /// <returns></returns>
    bool SpinCheck(float vartical)
    {
        //スティックが上方向（０～180の間）
        if(vartical > 0)
        {
            //現在の角度が1フレーム前の角度より大きければ
            if (currentAngle > previouseAngle)
            {
                //右回転
                return true;
            }
            else 
            {
                //左回転
                return false;
            }
        }
        //スティックが下方向（-180～0の間）
        else
        {
            //現在の角度が1フレーム前の角度より大きければ
            if (currentAngle > previouseAngle)
            {
                //左回転
                return false;
            }
            else 
            {
                //右回転
                return true;
            }
        }
    }

    /// <summary>
    /// 右回転メソッド
    /// </summary>
    void RightSpin()
    {
        //前回と逆ならリセット
        if (setAngle < 0)
            setAngle = 0;
        //右方向に足す
        setAngle += Mathf.Abs(currentAngle - previouseAngle);
    }

    /// <summary>
    /// 左回転メソッド
    /// </summary>
    void LeftSpin()
    {
        //前回と逆ならリセット
        if (setAngle > 0)
            setAngle = 0;
        //左方向に足す
        setAngle -= Mathf.Abs(currentAngle - previouseAngle);
    }

    /// <summary>
    /// オブジェクト投擲
    /// </summary>
    void ObjectThrow()
    {
        if (catchObject.tag == "Enemy")
        {
            Enemy enemy = catchObject.GetComponent<Enemy>();
            enemy.ThrowAttack = (int)(enemy.maxThrowAttack * Mathf.Abs(swingSpeed) / swingSpeedRange);
            enemy.SwingAttack = (int)(enemy.maxSwingAttack * Mathf.Abs(swingSpeed) / swingSpeedRange);
            throwSpeed = Mathf.Abs(swingSpeed) * 400;
            //catchObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //catchObject.GetComponent<Rigidbody>().useGravity = false;
            Vector3 throwVelocity = (hookPointer.transform.position - transform.position).normalized;
            throwVelocity.y = 0;
            catchObject.GetComponent<Enemy>().ThrowSet(throwSpeed,throwVelocity);
            catchObject.transform.position = new Vector3(transform.position.x + throwVelocity.x*2, 3, transform.position.z + throwVelocity.z*2);
            //catchObject.GetComponent<Rigidbody>().AddForce(throwVelocity * throwSpeed);
            playerState = PlayerState.HOOKRETURN;
            //catchObject.GetComponent<BoxCollider>().isTrigger = false;
            //catchObject.gameObject.layer = 15;
            Destroy(swing_Particle);
            //catchObject.GetComponent<Enemy>().isFly = true;
            timingTime = origin_TimingTime;
            seAudio.PlayOneShot(seList[1]);
            swingSpeed = 0;
        }

        else if(catchObject.tag == "Boss")
        {
            throwSpeed = Mathf.Abs(swingSpeed) * 400;
            catchObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            catchObject.GetComponent<Rigidbody>().useGravity = false;
            Vector3 throwVelocity = (hookPointer.transform.position - transform.position).normalized;
            throwVelocity.y = 0;
            catchObject.transform.position = new Vector3(transform.position.x + throwVelocity.x * 2, 3, transform.position.z + throwVelocity.z * 2);
            catchObject.GetComponent<Rigidbody>().AddForce(throwVelocity * throwSpeed);
            playerState = PlayerState.HOOKRETURN;
            //catchObject.GetComponent<BoxCollider>().isTrigger = false;
            catchObject.gameObject.layer = 15;
            Destroy(swing_Particle);
            timingTime = origin_TimingTime;
            seAudio.PlayOneShot(seList[1]);
            catchObject.GetComponent<BossControl>().bossState = BossState.Fly;
            swingSpeed = 0;
        }
    }

    /// <summary>
    /// オブジェクトたたきつけ
    /// </summary>
    void ObjectSlap()
    {
        //if (testMoveState == TestMoveState.SIDEVIEW)
        //{
        catchObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        catchObject.transform.position = new Vector3(transform.position.x+ Input.GetAxisRaw("Horizontal") , transform.position.y -1, transform.position.z);
        if (!isJumpFlag)
            catchObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, -1) * slapSpeed);
        Instantiate(slap_Particle, catchObject.transform.position, Quaternion.identity);
        playerState = PlayerState.HOOKRETURN;
        catchObject.GetComponent<Enemy>().isSlap = true;
        catchObject.gameObject.layer = 18;
        sp -= 20;
        //}
        //else
        //{

        //}
    }
}
