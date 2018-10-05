//
//10月5日
//田中悠斗
//ゲートに当たったか判定するクラス
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WrapGate : MonoBehaviour {

    public string stageName;
    public SelectManager selectManager;

    
   
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameObject.Find("Nametransprot");
            selectManager.Change();
            Debug.Log(stageName);
        }
    }
}
