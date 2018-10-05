//
//10月5日
//田中悠斗
//mainシーンに入るステージを決めるクラス
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Name : MonoBehaviour {

    //static SelectManager instance;
    static Name stageName;
    //public WrapGate warpGate;
    

    // Use this for initialization
    void Awake()
    {   
 
        DontDestroyOnLoad(gameObject);
        
    }

    // Update is called once per frame
    void Update () {
		
	}
}
