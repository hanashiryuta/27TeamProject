using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warning_Change : MonoBehaviour
{

    public Image im;
    [HideInInspector]
    public bool isUp = true;

    void Start()
    {
        im = im.GetComponent<Image>();
    }

    void Update()
    {

        Color alpha = im.color;
        alpha.a = 1.0f;
        im.color = alpha;

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
            im.color -= Color.green / 1.0f * Time.deltaTime;
            im.color -= Color.blue / 1.0f * Time.deltaTime;
        }
        else if (isUp == false)
        {
            im.color += Color.green / 1.0f * Time.deltaTime;
            im.color += Color.blue / 1.0f * Time.deltaTime;
        }
    }


}
