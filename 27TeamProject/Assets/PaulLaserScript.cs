using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaulLaserScript : Enemy {

    [SerializeField]
    public Transform[] PaulList;
    //public int childCount;
    public int listCount;

    [SerializeField]
    float setTime;
    [SerializeField]
    float time;
    [SerializeField]
    float LaserSpeed;

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

    public Animator anim;

    public float setAnimTime;
    float animTime;

    public GameObject LaserAnimObject;

    public override void Awake()
    {
        
    }

    public override void Start()
    {
        base.Start();
        lineRenderer = GetComponent<LineRenderer>();
        //childCount = transform.childCount;
        
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

        LaserAnimObject.SetActive(false);

        listCount = PaulList.Length;
    }

    public override void Update () {
        int nullcount = 0;
        foreach (var pl in PaulList)
        {
            if(pl == null)
            {
                nullcount++;
            }
        }
        
        if (nullcount == 4)
        {
            Destroy(gameObject);
        }

        float HP = (float)hp / (float)inputHp;

        if (HP < blowHP)
        {
            PaulList[blowcount].GetComponent<PaulEnemy>().GetMasterBlow = true;
            hp = (int)((float)inputHp * blowHP);
            blowHP -= 0.25f;
            blowcount++;
        }
        
        base.Update();

        if (LaserAnimObject == null) return;
        if (LaserAnimObject.activeInHierarchy) anim.SetBool("isBool", !isLaser);
         
        animTime = setAnimTime;
        
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

            if (time < animTime && time > 1)
            {
                LaserAnimObject.SetActive(true);
                anim.SetTrigger("isTriggerLaser");
            }
        }
        else
        {
            lineRenderer.enabled = true;
            LaserCollider.SetActive(true);
            LaserEnd.transform.position -= new Vector3(0, 0, LaserSpeed);

            lineRenderer.SetPosition(1, LaserEnd.transform.position);
            LaserColliderSet();
            if (LaserEnd.transform.position.z < -30)
            {
                LaserEnd.transform.position = new Vector3(PaulList[blowcount].transform.position.x, 0.05f, PaulList[blowcount].transform.position.z + 1);
                isLaser = false;
                lineRenderer.enabled = false;
                LaserCollider.SetActive(false);
                LaserAnimObject.SetActive(false);
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

    public override void TriggerSetRotate()
    {
        
    }
}