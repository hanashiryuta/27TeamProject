//
//10月3日
//田中　悠斗
//titleのシーン遷移
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Button selectButton;

    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        selectButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Decision"))
        //{

        //    SceneManager.LoadScene("Select");
        //}

    }

    public void GameStart()
    {
        SceneManager.LoadScene("Select");
    }

    public void GameEnd()
    {
        Application.Quit();
    }
}
