using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLastObject : MonoBehaviour {

    [SerializeField]
    float speed;
	
	void Update () {
        if (transform.parent.GetComponent<PaulLaserScript>().isLaser)
        {
            transform.position -= new Vector3(0, 0, speed);
        }
	}
}
