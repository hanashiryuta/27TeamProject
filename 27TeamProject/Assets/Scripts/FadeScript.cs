using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum FadeState
{
    STARTFADE,
    STAY,
    ENDFADE
}

public class FadeScript : MonoBehaviour {

    [SerializeField]
    float fadeSpeed;
    
    public FadeState fadeState = FadeState.STARTFADE;

    public bool isSceneEnd;

    public string nextScene;

	// Use this for initialization
	void Start () {
        fadeState = FadeState.STARTFADE;
	}
	
	// Update is called once per frame
	void Update () {
        Color color = GetComponent<Image>().color;

        switch (fadeState)
        {
            case FadeState.STARTFADE:
                color.a -= fadeSpeed;
                if (color.a <= 0)
                    fadeState = FadeState.STAY;
                break;
            case FadeState.STAY:
                if (isSceneEnd)
                    fadeState = FadeState.ENDFADE;
                break;
            case FadeState.ENDFADE:
                color.a += fadeSpeed;
                if(color.a >= 1)
                SceneManager.LoadScene(nextScene);
                break;
        }

        GetComponent<Image>().color = color;
	}
}
