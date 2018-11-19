using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scenemanager : MonoBehaviour {
    public Button selectButton;

    // Use this for initialization
    void Start()
    {
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

    public void Title()
    {
        SceneManager.LoadScene("Title");
    }

    public void GameEnd()
    {
        Application.Quit();
    }
}
