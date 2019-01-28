using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warning_Change : MonoBehaviour
{

   
    public Image im;
    public BGMManager bgmManager;
    [HideInInspector]
    public bool isUp = true;
    int count;
    void Start()
    {
        bgmManager = GameObject.FindGameObjectWithTag("BGM").GetComponent<BGMManager>();
        count = 0;
        im = im.GetComponent<Image>();
        im.color = new Color(0, 0, 0, 0);
        bgmManager.Warning_BGM();

    }

    void Update()
    {

       
        if (count < 150)
        {
            if (im.color.b > 1)
            {
                isUp = true;
            }
            else if (im.color.b < 0)
            {
                isUp = false;
            }
            if (isUp == true)
            {
                //im.color -= Color.red / 1.0f * Time.deltaTime;
                im.color -= Color.green / 1.0f * Time.deltaTime;
                im.color -= Color.blue / 1.0f * Time.deltaTime;
            }
            else if (isUp == false)
            {
                im.color += Color.red / 1.0f * Time.deltaTime;
                im.color += Color.green / 1.0f * Time.deltaTime;
                im.color += Color.blue / 1.0f * Time.deltaTime;
            }
            count++;
            Debug.Log(count);
        }
        else
        {
            im.color -= Color.red / 1.5f * Time.deltaTime;
            im.color -= Color.green / 1.5f * Time.deltaTime;
            im.color -= Color.blue / 1.5f * Time.deltaTime;
        }
    }


}
