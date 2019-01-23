using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scenemanager : MonoBehaviour {

    public string nextSceneName;
    public Button selectButton;
    public FadeScript fade;

    public List<AudioClip> seList;
    AudioSource seAudio;

    // Use this for initialization
    void Start()
    {
        if (Cursor.visible == true)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        seAudio = gameObject.AddComponent<AudioSource>();
        selectButton.Select();
    }

    public void NextScene()
    {
        if (fade.fadeState == FadeState.STAY)
        {
            seAudio.PlayOneShot(seList[1]);
            fade.nextScene = nextSceneName;
            fade.isSceneEnd = true;
        }
    }

    public void GameEnd()
    {
        if (fade.fadeState == FadeState.STAY)
        {
            seAudio.PlayOneShot(seList[1]);
            Application.Quit();
        }
    }

    public void SelectSound()
    {
        seAudio.PlayOneShot(seList[0]);
    }
}
