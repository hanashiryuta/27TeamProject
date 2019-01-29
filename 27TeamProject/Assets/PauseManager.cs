using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PauseState
{
    PAUSESTAY,
    OBJECTSET,
    PAUSESTART,
    PAUSEEND,
    SCENECHANGE,
}

public class PauseManager : Scenemanager {

    public GameObject pauseImages;

    [HideInInspector]
    public PauseState pauseState = PauseState.PAUSESTAY;

    [HideInInspector]
    public bool isPause;
    
    List<GameObject> pauseList;//ポーズするオブジェクト
    public List<string> tagNameList;//ポーズするタグ
    
    public RectTransform arrowRect;

    // Use this for initialization
    public override void Start ()
    {
        if (Cursor.visible == true)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        seAudio = gameObject.AddComponent<AudioSource>();
        pauseList = new List<GameObject>();
        isPause = false;
    }
	
	// Update is called once per frame
	public override void Update () {
        pauseImages.SetActive(isPause);

		switch(pauseState)
        {
            case PauseState.PAUSESTAY:
                StayAction();
                break;
            case PauseState.OBJECTSET:
                ObjectSet();
                break;
            case PauseState.PAUSESTART:
                PauseStart();
                break;
            case PauseState.PAUSEEND:
                PauseEnd();
                break;
        }
	}
   
    private void StayAction()
    {
        if(isPause)
        {
            selectButton.Select();
            pauseState = PauseState.OBJECTSET;
        }
    }

    public override void Selected(Button button)
    {
        base.Selected(button);
        arrowRect.position = new Vector3(arrowRect.position.x, buttonRect.position.y, arrowRect.position.z);
    }

    private void ObjectSet()
    { 
        //ポーズリストクリア
        pauseList.Clear();
        //設定したタグをポーズ対象に選択
        foreach (var tagName in tagNameList)
        {
            //タグで探す
            GameObject[] objList = GameObject.FindGameObjectsWithTag(tagName);

            //リスト追加
            foreach (var obj in objList)
            {
                pauseList.Add(obj);
            }
        }
        pauseState = PauseState.PAUSESTART;
    }

    void PauseStart()
    {//ポーズ対象から1つずつ呼び出す
        foreach (var pauseObj in pauseList)
        {
            //空なら飛ばす
            if (pauseObj == null)
                continue;

            //スクリプト取得
            Behaviour[] pauseBehavs = Array.FindAll(pauseObj.GetComponentsInChildren<Behaviour>(), (obj) => { return obj.enabled; });
            //スクリプトを一時非アクティブ化
            foreach (var com in pauseBehavs)
            {
                com.enabled = false;
            }

            //リジットボディ取得
            Rigidbody[] rgBodies = Array.FindAll(pauseObj.GetComponentsInChildren<Rigidbody>(), (obj) => { return !obj.IsSleeping(); });
            //リジッドボディがあれば
            if (rgBodies.Length != 0)
            {
                //移動量保存用配列
                Vector3[] rgBodyVels = new Vector3[rgBodies.Length];
                Vector3[] rgBodyAVels = new Vector3[rgBodies.Length];
                for (var i = 0; i < rgBodies.Length; ++i)
                {
                    //移動量保存用
                    rgBodyVels[i] = rgBodies[i].velocity;
                    rgBodyAVels[i] = rgBodies[i].angularVelocity;
                    //リジッドボディ止める
                    rgBodies[i].Sleep();
                }
            }
        }
        if (Input.GetButtonDown("Pause"))
        {
            pauseState = PauseState.PAUSEEND;
        }
    }

    public void PauseEnd()
    {
        //ポーズ対象から1つずつ呼び出す
        foreach (var pauseObj in pauseList)
        {
            //空なら飛ばす
            if (pauseObj == null)
                continue;

            //スクリプト取得
            Behaviour[] pauseBehavs = Array.FindAll(pauseObj.GetComponentsInChildren<Behaviour>(), (obj) => { return !obj.enabled; });
            //スクリプトをアクティブ化
            foreach (var com in pauseBehavs)
            {
                com.enabled = true;
            }

            //リジットボディ取得
            Rigidbody[] rgBodies = Array.FindAll(pauseObj.GetComponentsInChildren<Rigidbody>(), (obj) => { return obj.IsSleeping(); });
            if (rgBodies.Length != 0)
            {
                //移動量配列
                Vector3[] rgBodyVels = new Vector3[rgBodies.Length];
                Vector3[] rgBodyAVels = new Vector3[rgBodies.Length];
                for (var i = 0; i < rgBodies.Length; ++i)
                {
                    //リジッドボディ止める
                    rgBodies[i].WakeUp();
                    //移動量回帰
                    rgBodies[i].velocity = rgBodyVels[i];
                    rgBodies[i].angularVelocity = rgBodyAVels[i];
                }
            }
        }
        isPause = false;
        pauseState = PauseState.PAUSESTAY;
    }
}
