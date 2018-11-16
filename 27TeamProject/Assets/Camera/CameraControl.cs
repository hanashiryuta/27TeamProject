///11月9日
///石橋功基
///カメラクラス


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    GameObject player;　//追従するプレイヤーを入れるオブジェクト
    private Vector3 cameraPosition;　//カメラの初期値格納用
    private bool checkLeft;　//カメラ範囲の左チェック用
    private bool checkRight;　//カメラ範囲の右チェック用
    private float x;　//プレイヤー追従のｘ座標
    private Vector3 stopPosition;　//カメラ停止時のポジション格納用

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //カメラ初期値格納
        cameraPosition = transform.position;
        checkLeft = false;
        checkRight = false;
    }	
	// Update is called once per frame
	void Update() {
        //プレイヤー追従
        CameraMove();
    }

    public void CheckLeft()
    {
        checkLeft = !checkLeft;
    }

    public void CheckRight()
    {
        checkRight = !checkRight;
    }

    public void CameraMove()
    {
        //左端にプレイヤーがいる場合
        if (checkLeft && stopPosition.x > player.transform.position.x)
        {
            transform.position = transform.position;
        }

        //右端にプレイヤーがいる場合
        else if (checkRight && stopPosition.x < player.transform.position.x)
        {
            transform.position = transform.position;
        }

        //プレイヤーの通常追従時
        else
        {
            x = player.transform.position.x;
            transform.position = new Vector3(x, cameraPosition.y, cameraPosition.z);
        }
    }

    public void CameraStop()
    {
        //カメラ停止時ポジション格納
        stopPosition = player.transform.position;
    }
}
