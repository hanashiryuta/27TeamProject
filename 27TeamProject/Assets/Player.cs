///
///10/19
///葉梨竜太
///プレイヤークラス
///
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    //たたきつけパーティクル
    public GameObject slap_Particle;

    // Use this for initialization
    void Start()
    {
        //取得
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HookPointer();
        Move();

        //プレイヤーの状態で変化
        switch (playerState)
        {
            case PlayerState.NORMALMOVE://移動
                    Jump();
                    HookShot();
                //else if (Input.GetButtonUp("Jump"))
                //    hook.GetComponent<Hook>().hookState = HookState.RETURN;
                break;
            case PlayerState.HOOKMOVE://フック移動
                HookMove();
                break;
            case PlayerState.HOOKSWING://フック振り回し
                    HookSwing();
                    Jump();
                if (catchObject != null && Input.GetButtonUp("Jump"))
                {
                    //下方向なら
                    if (Input.GetAxis("Vertical") <= -0.5f)
                    {
                        //たたきつけ
                        ObjectSlap();
                    }
                    else
                    {
                        //投げつけ
                        ObjectThrow();
                    }
                    hook.GetComponent<Hook>().hookState = HookState.RETURN;
                }
                break;
            case PlayerState.HOOKRETURN://フック戻り
                    Jump();
                break;
        }
    }
    
    /// <summary>
    /// 移動処理
    /// </summary>
    public void Move()
    {
        Vector3 moveVector = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0);
        Vector3 rigidVelocity = new Vector3(rigid.velocity.x, 0);
        rigid.AddForce(moveForceMultiplier * (moveVector - rigidVelocity));
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    public void Jump()
    {
        //Bボタンでジャンプ
        if (isJumpFlag && Input.GetButtonDown("Fire2"))
        {
            isJumpFlag = false;
            rigid.AddForce(Vector2.up * jumpPower);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //グラウンドに当たったら
        if (collision.gameObject.CompareTag("Ground"))
        {
            //フラグセット
            isJumpFlag = true;
        }
    }

    /// <summary>
    /// ポインター配置
    /// </summary>
    void HookPointer()
    {
        //左スティックの方向にポインター配置
        if (Mathf.Abs(Input.GetAxis("Vertical")) >= 0.1f || Mathf.Abs(Input.GetAxis("Horizontal")) >= 0.1f)
        {
            pointerAngle = Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
            hookPointer.transform.localPosition = new Vector3(Mathf.Cos(pointerAngle) * pointerRadius, Mathf.Sin(pointerAngle) * pointerRadius, -1);
        }
    }

    /// <summary>
    /// フック射出
    /// </summary>
    void HookShot()
    {
        if (isHookShot && Input.GetButtonDown("Jump"))
        {
            //ポインターの位置に向かって射出
            hook = Instantiate(originHook, transform.position, Quaternion.identity);
            hook.GetComponent<Hook>().player = gameObject;
            hook.GetComponent<Hook>().targetPosition = new Vector3(hookPointer.transform.position.x, hookPointer.transform.position.y, 0);//new Vector3(Mathf.Cos(angle) * shotRadius, Mathf.Sin(angle) * shotRadius, 0);
            isHookShot = false;
        }
    }

    /// <summary>
    /// フック射出準備
    /// </summary>
    /// <param name="hook"></param>
    public void HookSet(GameObject hook)
    {
        hitPosition = hook.transform.position;
        hitDistance = Vector3.Distance(hook.transform.position, transform.position);
        //gameObject.AddComponent<DistanceJoint2D>();
        //GetComponent<DistanceJoint2D>().connectedAnchor = new Vector2(hitPosition.x, hitPosition.y);
        //GetComponent<DistanceJoint2D>().distance = hitDistance;
        playerState = PlayerState.HOOKMOVE;
    }

    /// <summary>
    /// フック移動
    /// </summary>
    void HookMove()
    {
        Vector3 flyVelocity = (hitPosition - transform.position).normalized;
        rigid.AddForce(flyVelocity * flySpeed);
        //ボタン離せばフック切断
        if (Input.GetButtonUp("Jump"))
        {
            //Destroy(GetComponent<DistanceJoint2D>());
            hook.GetComponent<Hook>().hookState = HookState.RETURN;
            playerState = PlayerState.HOOKRETURN;
        }
        //rigid.AddForce(new Vector3(Input.GetAxis("Horizontal") * 10, 0));
    }

    /// <summary>
    /// 回す準備
    /// </summary>
    /// <param name="m_CatchObject">回すオブジェクト</param>
    public void SwingSet(GameObject m_CatchObject)
    {
        catchObject = m_CatchObject;
        swingRadius = Vector3.Distance(transform.position, catchObject.transform.position);
        if (swingRadius <= 1)
            swingRadius = 1;
        swingAngle = Mathf.Atan2(catchObject.transform.position.y - transform.position.y, catchObject.transform.position.x - transform.position.x) * 180 / Mathf.PI;
        playerState = PlayerState.HOOKSWING;
        swingSpeed = 0;
    }

    /// <summary>
    /// フック振り回し
    /// </summary>
    void HookSwing()
    {
        //ボタン押している間
        if (Input.GetButton("Jump"))
        {
            swingSpeed += swingSpeedRate;
            if (swingSpeed >= swingSpeedRange)
                swingSpeed = swingSpeedRange;
            swingAngle += swingSpeed;
            catchObject.transform.position = transform.position + new Vector3(swingRadius * Mathf.Cos(swingAngle * Mathf.PI / 180), 0, swingRadius * Mathf.Sin(swingAngle * Mathf.PI / 180));
        }
    }
    
    /// <summary>
    /// オブジェクト投擲
    /// </summary>
    void ObjectThrow()
    {
        catchObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        catchObject.GetComponent<Rigidbody>().useGravity = false;
        Vector3 throwVelocity = (hookPointer.transform.position - transform.position).normalized;
        catchObject.transform.position = new Vector3(transform.position.x + throwVelocity.x, transform.position.y, 0);
        throwVelocity.z = 0;
        catchObject.GetComponent<Rigidbody>().AddForce(throwVelocity*throwSpeed);
        playerState = PlayerState.HOOKRETURN;
    }

    /// <summary>
    /// オブジェクトたたきつけ
    /// </summary>
    void ObjectSlap()
    {
        catchObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        catchObject.transform.position = new Vector3(transform.position.x + Input.GetAxisRaw("Horizontal"), transform.position.y, 0);
        if (!isJumpFlag)
            catchObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, -1) * throwSpeed);
        Instantiate(slap_Particle, catchObject.transform.position, Quaternion.identity);
    }
}
