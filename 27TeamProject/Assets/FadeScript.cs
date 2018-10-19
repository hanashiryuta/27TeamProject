using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour {

    [SerializeField]
    float fadeSpeed;
    float alfa;
    float red, green, blue;

    public bool isFade;


	// Use this for initialization
	void Start () {
        alfa = 1;
        isFade = false;
        Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Image>().color = new Color(red, green, blue, alfa);

        alfa -= fadeSpeed;
        if(alfa < 0)
        {
            isFade = true;
            Time.timeScale = 1;
        }
	}
}
