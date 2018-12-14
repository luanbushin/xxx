using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("w")) {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z+0.1f);
        }
        if (Input.GetKey("s"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
        }
        if (Input.GetKey("a"))
        {
            transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
        }
        if (Input.GetKey("d"))
        {
            transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
        }

    }
}
