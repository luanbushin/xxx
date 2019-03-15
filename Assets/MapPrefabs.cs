using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPrefabs : MonoBehaviour {

    public GameObject[] prefabs;
	// Use this for initialization
	void Start () {
        GameObject object1 = (GameObject)Resources.Load("map/Prefabs/Box_A1");
        Instantiate(object1, new Vector3(1, 2, 1), Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
