using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Enemy {

    [SerializeField]
    private float m_power = 0.0f;
    private Vector3 m_powerDir = Vector3.zero;
    private float time;

    private BossControl BC;
    private GameObject obj;

    int BossLayer;
    bool isGround;

    float angle;

    public override void AwakeSub()
    {
        
    }
    // Use this for initialization
    public override void StartSub()
    {
        base.StartSub();
        BossLayer = LayerMask.NameToLayer("Boss");
        isHook = true;
        obj = GameObject.FindGameObjectWithTag("Boss");
        BC = obj.GetComponent<BossControl>();
        if (BC.RockFlag() == false)
        {
            m_powerDir = new Vector3(Random.Range(0, 15), Random.Range(5, 10), Random.Range(-5, 5));
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.AddForce(m_powerDir.normalized * m_power);
        }
        else if (BC.RockFlag() == true)
        {
            m_powerDir = new Vector3(Random.Range(0, -15), Random.Range(10, 20), Random.Range(-5, 5));
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.AddForce(m_powerDir.normalized * m_power);
        }
    }

    public override void UpdateSub()
    {
        time += Time.deltaTime;

        if(!isGround)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
            angle++;
        }

        if (time > 5 && isHook == true)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isGround = true;
            transform.position = transform.position;
        }
    }

    public override void TriggerAction(Collider other)
    {
    //    if (LayerMask.LayerToName(other.gameObject.layer) == "Boss")
    //    {
    //        Destroy(gameObject);
    //    }
    //    if (other.gameObject.layer == BossLayer)
    //    {
    //        float boss = GetComponent<BossEnemy>().Hp();
    //         boss -= other.gameObject.GetComponent<Enemy>().SwingAttack;
    //        Instantiate(origin_Damege_Particle, transform.position, Quaternion.identity);
    //    }
    }
}
