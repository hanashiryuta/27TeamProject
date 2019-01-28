using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour {

    public List<AudioClip> bgmList;
    static BGMManager instance;
    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
	}

    void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        audioSource = GetComponent<AudioSource>();
        if (scene.name == "Title")
        {
            audioSource.clip = bgmList[0];
        }
        else if(scene.name == "SampleScene")
        {
            audioSource.clip = bgmList[1];
        }
        else if(scene.name == "GameClear")
        {
            audioSource.clip = bgmList[2];
        }
        else if(scene.name == "GameOver")
        {
            audioSource.clip = bgmList[3];
        }
        audioSource.Play();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }   
}
