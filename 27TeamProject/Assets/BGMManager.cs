﻿using System.Collections;
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
        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
	}

    void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        if(scene.name == "Title")
        {
            audioSource.clip = bgmList[0];
        }
        else if(scene.name == "SampleScene")
        {
            audioSource.clip = bgmList[1];
        }
        audioSource.Play();
    }
	
    
    
}