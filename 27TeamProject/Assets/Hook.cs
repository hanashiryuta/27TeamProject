﻿///
///10/19
///葉梨竜太
///フッククラス
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フック状態
/// </summary>
public enum HookState
{
    MOVE,//移動
    STAY,//待機
    CATCH,//キャッチ
    DEATH,//削除
    RETURN,//帰還
}

public class Hook : MonoBehaviour {

    //プレイヤー
    [HideInInspector]
    public GameObject player;
    //目標地点
    [HideInInspector]
    public Vector3 targetPosition;
    //フック移動方向
    Vector3 hookVelocity;
    //移動スピード
    public float moveSpeed;
    //フック状態
    [HideInInspector]
    public HookState hookState = HookState.MOVE;
    //つかんだオブジェクト
    GameObject catchObject;

	// Use this for initialization
	void Start () {
        //移動方向設定
        hookVelocity = targetPosition - transform.position;
	}

    void FixedUpdate()
    {
        //フック状態で変化
        switch (hookState)
        {
            case HookState.MOVE://移動
                Move();
                break;
            case HookState.STAY://待機
                break;
            case HookState.CATCH://キャッチ
                transform.position = catchObject.transform.position;
                break;
            case HookState.RETURN://帰還
                hookVelocity = player.transform.position - transform.position;
                transform.position += hookVelocity.normalized * moveSpeed;
                break;
            case HookState.DEATH://削除
                Death();
                break;
        }
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    private void Move()
    {
        transform.position += hookVelocity.normalized * moveSpeed;
        if (Vector3.Distance(targetPosition, transform.position) <= 0.2f)
        {
            hookState = HookState.RETURN;
        }
    }

    /// <summary>
    /// 削除処理
    /// </summary>
    void Death()
    {
        player.GetComponent<Player>().isHookShot = true;
        player.GetComponent<Player>().playerState = PlayerState.NORMALMOVE;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        //地面に当たったら
        if (collision.gameObject.CompareTag("Ground") && hookState == HookState.MOVE)
        {
            //待機処理
            hookState = HookState.STAY;
            player.GetComponent<Player>().HookSet(gameObject);
        }
        //敵に当たったら
        if (collision.gameObject.CompareTag("Enemy") && hookState == HookState.MOVE)
        {
            //キャッチ処理
            hookState = HookState.CATCH;
            catchObject = collision.gameObject;
            player.GetComponent<Player>().SwingSet(collision.gameObject);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        //プレイヤーに当たったら
        if (collider.gameObject.CompareTag("Player") && (hookState == HookState.RETURN||hookState == HookState.STAY))
        {
            //削除処理
            hookState = HookState.DEATH;
        }
    }
}