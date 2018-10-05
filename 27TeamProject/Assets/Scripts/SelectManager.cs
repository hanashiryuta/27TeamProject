//
//10月5日
//田中悠斗
//Gameシーンを読み込むクラス
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SelectManager : MonoBehaviour
{
    
    void Start()
    {
        
    }
    void Update()
    {
       
    }
    public void Change()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
