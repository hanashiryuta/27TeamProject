using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour {

    float alfa;
    public float speed = 0.01f;
    float red, green, blue;
    bool alfaflag;

    // Use this for initialization
    void Start () {
        red = GetComponent<Image>().color.r;
        green = GetComponent<Image>().color.g;
        blue = GetComponent<Image>().color.b;
        alfaflag = true;
    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<Image>().color = new Color(red, green, blue, alfa);

        if (alfaflag)
        {
            alfa += speed;
            if (alfa >= 1)
                alfaflag = false;
        }
        else
        {
            alfa -= speed;
            if(alfa <= 0)
            {
                alfaflag = true;
            }
        }
        
    }
}
