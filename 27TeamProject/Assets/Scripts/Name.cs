//
//10月5日
//田中悠斗
//mainシーンに入るステージを決めるクラス
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Name : MonoBehaviour {

    //static SelectManager instance;
    static Name instance;
    //public WrapGate warpGate;
    public string stagename;

    // Use this for initialization
    void Awake()
    {   
 
        DontDestroyOnLoad(gameObject);
        stagename = "1-1";
        SceneManager.LoadScene("SampleScene");
        
    }

    // Update is called once per frame
    void Update () {
		
	}
}
