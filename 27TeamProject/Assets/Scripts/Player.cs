using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    NORMALMOVE,
    HOOKMOVE,
}

public class Player : MonoBehaviour
{

    Rigidbody2D rigid;
    public float moveSpeed;
    public float moveForceMultiplier = 50;
    public float jumpPower;
    bool isJumpFlag;
    PlayerState playerState = PlayerState.NORMALMOVE;

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        switch (playerState)
        {
            case PlayerState.NORMALMOVE:
                Move();
                HookPoint();
                if (Input.GetButtonDown("Jump"))
                    Jump();
                break;
            case PlayerState.HOOKMOVE:
                break;
        }
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    public void Move()
    {
        Vector2 moveVector = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, 0);
        Vector2 rigidVelocity = new Vector2(rigid.velocity.x, 0);
        rigid.AddForce(moveForceMultiplier * (moveVector - rigidVelocity));
    }

    public void Jump()
    {
        if (isJumpFlag)
        {
            isJumpFlag = false;
            rigid.AddForce(Vector2.up * jumpPower);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumpFlag = true;
        }
    }

    void HookPoint()
    {

    }

}
