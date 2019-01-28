using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warning : MonoBehaviour
{

    public float offset;

    public List<GameObject> ImageList;


    // Update is called once per frame
    void Update()
    {

        foreach (var s in ImageList)
        {
            s.transform.position += new Vector3(-offset, 0, 0);
            if (s.transform.position.x < 0 - (s.GetComponent<RectTransform>().sizeDelta.x / 2))
            {
                s.transform.position = new Vector3(Screen.width + s.GetComponent<RectTransform>().sizeDelta.x * 1.8f, s.transform.position.y, s.transform.position.z);

            }
        }



    }
}
