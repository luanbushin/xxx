using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMapelement : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject[] all = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
        Debug.Log(all);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
