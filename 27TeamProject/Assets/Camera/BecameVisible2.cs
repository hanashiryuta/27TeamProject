﻿///11月9日
///石橋功基
///右範囲チェック用クラス


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BecameVisible2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //チェック用のブロックが画面に表示されたとき呼び出す
    void OnBecameVisible()
    {
        //カメラ取得
        var cameraCtrl = Camera.main.GetComponent<CameraControl>();
        cameraCtrl.CheckRight();
        cameraCtrl.CameraStop();
    }

    //チェック用ブロックが画面外になったとき呼び出す
    void OnBecameInvisible()
    {
        //カメラ取得
        var cameraCtrl = Camera.main.GetComponent<CameraControl>();
        cameraCtrl.CheckRight();
    }
}
