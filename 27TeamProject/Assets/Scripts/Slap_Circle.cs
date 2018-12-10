using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slap_Circle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale += new Vector3(0.3f, 0.3f, 0.3f);
        if (transform.localScale.x >= 6)
            Destroy(gameObject);
	}
}
