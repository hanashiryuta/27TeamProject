//
//12月17日
//ライト点滅クラス
//田中　悠斗
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightWarning : MonoBehaviour
{
    public Light lt;
    [HideInInspector]
    public bool isUp = true;

    void Start()
    {
        lt = lt.GetComponent<Light>();
    }

    void Update()
    {
        Color alpha = lt.color;
        alpha.a = 1.0f;
        lt.color = alpha;

        if (lt.color.b > 1)
        {
            isUp = true;
        }
        else if (lt.color.b < 0)
        {
            isUp = false;
        }
        if (isUp == true)
        {
            lt.color -= Color.green / 1.0f * Time.deltaTime;
            lt.color -= Color.blue / 1.0f * Time.deltaTime;
        }
        else if (isUp == false)
        {
            lt.color += Color.green / 1.0f * Time.deltaTime;
            lt.color += Color.blue / 1.0f * Time.deltaTime;
        }
    }
}
