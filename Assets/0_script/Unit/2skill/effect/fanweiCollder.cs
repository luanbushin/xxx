using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fanweiCollder : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("finish", 3f);
        GetComponent<CapsuleCollider>().enabled = false;
        Invoke("startTrigger", .5f);
        Invoke("finishTrigger", 1f);
    }
    private void OnTriggerEnter(Collider other)
    {
         if (other.gameObject.tag == "pohuai")
        {
            if (other.gameObject.GetComponent<EnemyAI>())
            {
                Destroy(other.gameObject.GetComponent<EnemyAI>().enemyview);
                Destroy(other.gameObject);
            }
        }
    }
    private void startTrigger()
    {
        GetComponent<CapsuleCollider>().enabled = true;
    }
    private void finishTrigger()
    {
        GetComponent<CapsuleCollider>().enabled = false;
    }

    private void finish()
    {
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
