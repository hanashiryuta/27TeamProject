using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaulLaserScript : Enemy {

    [SerializeField]
    public Transform[] PaulList;
    public int count;

    [SerializeField]
    float setTime;
    [SerializeField]
    float time;

    LineRenderer lineRenderer;
    
    public bool isLaser;
    
    float blowHP;
    int blowcount;

    public override void Awake()
    {
        
    }

    public override void Start()
    {
        base.Start();
        lineRenderer = GetComponent<LineRenderer>();
        count = transform.childCount;
        
        lineRenderer.enabled = false;
        time = setTime;
        isLaser = false;
        
        for(int i = 0; i < count - 1; i++)
        {
            PaulList[i].GetComponent<PaulEnemy>().GetMasterBlow = false;
        }
        blowHP = 0.75f;
        blowcount = 0;
    }

    public override void Update () {
        base.Update();

        float HP = (float)hp / (float)inputHp;

        if(HP < blowHP)
        {
            PaulList[blowcount].GetComponent<PaulEnemy>().GetMasterBlow = true;
            blowHP -= 0.25f;
            blowcount++;
        }
        
        if (!isLaser)
        {
            lineRenderer.enabled = false;
            time -= Time.deltaTime;
            if(time < 0)
            {
                PaulList[count - 1].transform.position = new Vector3(PaulList[blowcount].transform.position.x, PaulList[blowcount].transform.position.y, PaulList[blowcount].transform.position.z);
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
                PaulList[count - 1].transform.position = new Vector3(PaulList[blowcount].transform.position.x, PaulList[blowcount].transform.position.y, PaulList[blowcount].transform.position.z + 1);
                isLaser = false;
                lineRenderer.enabled = false;
            }
        }
	}
}