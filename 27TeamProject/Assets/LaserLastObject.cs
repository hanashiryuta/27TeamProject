using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLastObject : MonoBehaviour {
    
    int PlayerLayer;
    int EnemyLayer;

    bool isLaser;

    private void Start()
    {
        PlayerLayer = LayerMask.NameToLayer("Player");
        EnemyLayer = LayerMask.NameToLayer("Enemy");
    }

    //void Update () {
    //    if (transform.parent.GetComponent<PaulLaserScript>().isLaser)
    //    {
    //        transform.position -= new Vector3(0, 0, speed);
    //    }
    //}

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
                //other.GetComponent<Enemy>().hp -= 20;
            }
        }
    }
}
