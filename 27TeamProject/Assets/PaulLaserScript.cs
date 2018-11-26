using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaulLaserScript : MonoBehaviour {

    [SerializeField]
    public Transform[] PaulList;
    int count;

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
                PaulList[count - 1].transform.position = new Vector3(PaulList[0].transform.position.x, PaulList[0].transform.position.y, PaulList[0].transform.position.z);
                time = setTime;
                isLaser = true;
                lineRenderer.SetPosition(0, PaulList[count - 2].transform.position);
            }
        }
        else
        {
            lineRenderer.enabled = true;
            
            lineRenderer.SetPosition(1, PaulList[count - 1].transform.position);
            if (PaulList[count - 1].transform.position.z < -30)
            {
                PaulList[count - 1].transform.position = new Vector3(PaulList[0].transform.position.x, PaulList[0].transform.position.y, PaulList[0].transform.position.z + 1);
                isLaser = false;
                lineRenderer.enabled = false;
            }
        }
	}
}