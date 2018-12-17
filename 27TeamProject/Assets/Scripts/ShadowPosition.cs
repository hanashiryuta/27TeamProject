///
///12/17
///影クラス
///葉梨竜太
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPosition : MonoBehaviour {
    

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
	}
}
