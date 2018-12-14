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
    [SerializeField]
    float speed;

    LineRenderer lineRenderer;
    
    public bool isLaser;
    
    float blowHP;
    int blowcount;

    public GameObject LaserEnd;
    public GameObject LaserStart;
    public GameObject LaserCollider;
    float Length;
    float ColliderRotateR;
    float ColliderRotate;

    int PlayerLayer;
    int EnemyLayer;

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
        
        foreach(var pl in PaulList)
        {
            pl.GetComponent<PaulEnemy>().GetMasterBlow = false;
        }
        blowHP = 0.75f;
        blowcount = 0;
        Length = 0;
        ColliderRotateR = 0;
        ColliderRotate = 0;
        PlayerLayer = LayerMask.NameToLayer("Player");
        EnemyLayer = LayerMask.NameToLayer("Enemy");
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
            LaserCollider.SetActive(false);
            time -= Time.deltaTime;
            if(time < 0)
            {
                LaserEnd.transform.position = new Vector3(PaulList[blowcount].transform.position.x, 0.05f, PaulList[blowcount].transform.position.z);
                time = setTime;
                isLaser = true;
                lineRenderer.SetPosition(0, LaserStart.transform.position);
            }
        }
        else
        {
            lineRenderer.enabled = true;
            LaserCollider.SetActive(true);
            LaserEnd.transform.position -= new Vector3(0, 0, speed);

            lineRenderer.SetPosition(1, LaserEnd.transform.position);
            LaserColliderSet();
            if (LaserEnd.transform.position.z < -30)
            {
                LaserEnd.transform.position = new Vector3(PaulList[blowcount].transform.position.x, 0.05f, PaulList[blowcount].transform.position.z + 1);
                isLaser = false;
                lineRenderer.enabled = false;
                LaserCollider.SetActive(false);
            }
        }
	}

    void LaserColliderSet()
    {
        LaserCollider.transform.rotation = Quaternion.Euler(ColliderRotate, 0, 0);
        Vector3 p1 = LaserStart.transform.position;
        Vector3 p2 = LaserEnd.transform.position;
        Vector3 p3 = (p1 + p2) / 2;
        LaserCollider.transform.position = p3;

        Length = Vector3.Distance(p2, p1);
        LaserCollider.GetComponent<BoxCollider>().size = new Vector3(0.5f, 0.5f, Length / 2);

        ColliderRotateR = Mathf.Atan2(p2.y - p3.y, p2.z - p3.z);
        ColliderRotate = ColliderRotateR * 180 / Mathf.PI;
        LaserCollider.transform.rotation = Quaternion.Euler(-ColliderRotate, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (transform.parent.GetComponent<PaulLaserScript>().isLaser)
        {
            if (other.gameObject.layer == PlayerLayer)
            {
                other.GetComponent<Player>().hp -= 100;
            }
            else if (other.gameObject.layer == EnemyLayer)
            {
                other.GetComponent<Enemy>().hp -= 20;
            }
        }
    }
}