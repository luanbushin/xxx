using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour {

    // Use this for initialization
    private GameObject huoyan;
    private GameObject di;
	void Start () {
       transform.Find("activeObj").gameObject.SetActive(false) ;
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.Find("activeObj").gameObject.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.Find("activeObj").gameObject.SetActive(true);
        }
    }


    // Update is called once per frame
    void Update () {
		
	}
}
