﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{

    public List<AudioClip> bgmList;
    public EnemySpawnManager enemySpawnManager;
    static BGMManager instance;
    AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        if (scene.name == "Title")
        {
            audioSource.clip = bgmList[0];
        }
        else if (scene.name == "SampleScene")
        {
            audioSource.clip = bgmList[1];
        }
        else if (scene.name == "GameClear")
        {
            audioSource.clip = bgmList[2];
        }
        else if (scene.name == "GameOver")
        {
            audioSource.clip = bgmList[3];
        }
        audioSource.Play();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void Boss_BGM()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.clip = bgmList[4];
        audioSource.Play();
    }

    public void Warning_BGM()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
        Debug.Log(this.gameObject);
        audioSource.Stop();
        audioSource.clip = bgmList[5];
        audioSource.Play();
    }

}
