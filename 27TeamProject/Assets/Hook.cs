///
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
    ATTACK,//アタック
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
    public GameObject catchObject;

    public GameObject origin_ArmLine;
    GameObject player_ArmLine;

    LineRenderer armLine;

    string hookInput = "Hook";

    [HideInInspector]
    public float targetDistance;

    // Use this for initialization
    void Start () {
        //移動方向設定
        hookVelocity = targetPosition - transform.position;
        Vector3 scale = transform.localScale;
        scale.x = hookVelocity.normalized.x;
        if (scale.x >= 0)
            scale.x = 1;
        else
            scale.x = -1;
        transform.localScale = scale;
        LineSet();
	}

    void FixedUpdate()
    {
        armLine.SetPosition(0, player.transform.position + new Vector3(0, 2, 0));
        armLine.SetPosition(1, transform.position);
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
            case HookState.ATTACK://ノックバック攻撃
                Attack();
                break;
            case HookState.RETURN://帰還
                hookVelocity = player.transform.position+new Vector3(0,2,0) - transform.position;
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
        if (Vector3.Distance(transform.position,player.transform.position) >= targetDistance)
        {
            hookState = HookState.RETURN;
            player.GetComponent<Player>().playerState = PlayerState.HOOKRETURN;
        }
    }
    
    /// <summary>
    /// ノックバック攻撃処理
    /// </summary>
    void Attack()
    {
        Vector3 attackVelocity = catchObject.transform.position - transform.position;
        catchObject.GetComponent<Rigidbody>().AddForce(attackVelocity * 1000);
        hookState = HookState.RETURN;
    }

    /// <summary>
    /// 削除処理
    /// </summary>
    void Death()
    {
        player.GetComponent<Player>().isHookShot = true;
        player.GetComponent<Player>().playerState = PlayerState.NORMALMOVE;
        Destroy(player_ArmLine);
        Destroy(gameObject);
    }

    void LineSet()
    {
        player_ArmLine = Instantiate(origin_ArmLine);
        armLine = player_ArmLine.GetComponent<LineRenderer>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        ////地面に当たったら
        //if (collision.gameObject.CompareTag("Ground") && hookState == HookState.MOVE)
        //{
        //    //待機処理
        //    hookState = HookState.STAY;
        //    player.GetComponent<Player>().HookSet(gameObject);
        //}
        //敵に当たったら
        if (collision.gameObject.CompareTag("Enemy") && hookState == HookState.MOVE && collision.gameObject.GetComponent<Enemy>().isHook)
        {

            //当たった時にボタン押していたら
            if (Input.GetButton(hookInput)&&catchObject == null)
            {
                //キャッチ処理
                hookState = HookState.CATCH;
                catchObject = collision.gameObject;
                player.GetComponent<Player>().SwingSet(collision.gameObject);
                
               collision.gameObject.layer = 12;
               collision.GetComponent<Enemy>().isHook = false;
            }
            ////押していなければ
            //else
            //{
            //    catchObject = collision.gameObject;
            //    hookState = HookState.ATTACK;
            //}
        }

        //岩に当たったら
        if (collision.gameObject.CompareTag("Rock") && hookState == HookState.MOVE)
        {

            //当たった時にボタン押していたら
            if (Input.GetButton("Jump"))
            {
                //キャッチ処理
                hookState = HookState.CATCH;
                catchObject = collision.gameObject;
                player.GetComponent<Player>().SwingSet(collision.gameObject);

                collision.gameObject.layer = 12;
                collision.GetComponent<Rock>().isHook = false;
            }
            //押していなければ
            else
            {
                catchObject = collision.gameObject;
                hookState = HookState.ATTACK;
            }
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
