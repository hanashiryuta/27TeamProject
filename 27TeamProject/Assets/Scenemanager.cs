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
    
    float buttonScale;
    public float buttonScaleRate = 0.005f;

    RectTransform buttonRect;

    // Use this for initialization
    public virtual void  Start()
    {
        if (Cursor.visible == true)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        seAudio = gameObject.AddComponent<AudioSource>();
        selectButton.Select();
    }

    public virtual void Update()
    {

    }

    public virtual void NextScene()
    {
        if (fade.fadeState == FadeState.STAY)
        {
            seAudio.PlayOneShot(seList[1]);
            fade.nextScene = nextSceneName;
            fade.isSceneEnd = true;
        }
    }

    public virtual void GameEnd()
    {
        if (fade.fadeState == FadeState.STAY)
        {
            seAudio.PlayOneShot(seList[1]);
            Application.Quit();
        }
    }

    public virtual void Selected(Button button)
    {
        seAudio.PlayOneShot(seList[0]);
        buttonScale = 1.0f;
        this.buttonRect = button.GetComponent<RectTransform>();
    }

    public virtual void SelectUpdate()
    {
        buttonRect.localScale = new Vector3(buttonScale, buttonScale, buttonScale);
        buttonScale += buttonScaleRate;
        if (buttonScale <= 1)
        {
            buttonScaleRate = Mathf.Abs(buttonScaleRate);
        }
        else if (buttonScale >= 1.2f)
        {
            buttonScaleRate = -Mathf.Abs(buttonScaleRate);
        }
    }

    public virtual void Deselect()
    {
        buttonScale = 1.0f;
        buttonRect.localScale = new Vector3(buttonScale, buttonScale, buttonScale);
    }
}
