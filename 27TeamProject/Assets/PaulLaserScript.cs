using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaulLaserScript : MonoBehaviour {

    [SerializeField]
    public Transform[] PaulList;
    int count;

    [SerializeField]
    GameObject LaserObject;
    [SerializeField]
    float setTime;
    [SerializeField]
    float time;

    LineRenderer lineRenderer;
    
    public bool isLaser;
    
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        count = transform.childCount;
        
        lineRenderer.enabled = false;
        time = setTime;
        isLaser = false;
    }

    void Update () {
        if (!isLaser)
        {
            lineRenderer.enabled = false;
            time -= Time.deltaTime;
            if(time < 0)
            {
                Instantiate(LaserObject, transform.position + new Vector3(0, 0, -10), Quaternion.identity);
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, PaulList[count - 2].transform.position);
                time = setTime;
                isLaser = true;
            }
        }
        else
        {
            lineRenderer.SetPosition(1, PaulList[count - 1].transform.position);
            if (PaulList[count - 1].transform.position.z < -30)
            {
                isLaser = false;
                PaulList[count - 1].transform.position = PaulList[0].transform.position;
                lineRenderer.enabled = false;
            }
        }
	}
}