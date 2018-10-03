//
//10月3日
//田中　悠斗
//titleのシーン遷移
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown("Decision"))
        {
            Debug.Log("a");
            SceneManager.LoadScene("Select");
        }
		
	}
}
