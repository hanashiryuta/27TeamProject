using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {

    [SerializeField]
    private float m_power = 0.0f;
    private Vector3 m_powerDir = Vector3.zero;
    private float time;

    private BossControl BC;
    private GameObject obj;

    [HideInInspector]
    public bool isHook; //フックに捕まっているかの判定

    // Use this for initialization
    void Start()
    {
        isHook = true;
        obj = GameObject.Find("Boss");
        BC = obj.GetComponent<BossControl>();
        if (BC.RockFlag() == false)
        {
            m_powerDir = new Vector3(Random.Range(0, 15), Random.Range(10, 20), Random.Range(0, 10));
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.AddForce(m_powerDir.normalized * m_power);
        }
        else if (BC.RockFlag() == true)
        {
            m_powerDir = new Vector3(Random.Range(0, -15), Random.Range(10, 20), Random.Range(0, 10));
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.AddForce(m_powerDir.normalized * m_power);
        }
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time > 5 && isHook == true)
        {
            Destroy(gameObject);
        }

        if (isHook == false)
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.useGravity = false;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ground")
        {
            transform.position = transform.position;
        }
    }
}
