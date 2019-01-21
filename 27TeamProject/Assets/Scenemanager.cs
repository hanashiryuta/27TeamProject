using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scenemanager : MonoBehaviour {

    public string nextSceneName;
    public Button selectButton;
    public FadeScript fade;

    // Use this for initialization
    void Start()
    {
        if (Cursor.visible == true)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        selectButton.Select();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextScene()
    {
        if (fade.fadeState == FadeState.STAY)
        {
            fade.nextScene = nextSceneName;
            fade.isSceneEnd = true;
        }
    }

    public void GameEnd()
    {
        if (fade.fadeState == FadeState.STAY)
            Application.Quit();
    }
}
