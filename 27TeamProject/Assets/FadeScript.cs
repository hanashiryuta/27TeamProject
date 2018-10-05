using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour {

    [SerializeField]
    float fadeSpeed;
    float alfa;
    float red, green, blue;


	// Use this for initialization
	void Start () {
        alfa = 1;
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Image>().color = new Color(red, green, blue, alfa);

        alfa -= fadeSpeed;
	}
}
