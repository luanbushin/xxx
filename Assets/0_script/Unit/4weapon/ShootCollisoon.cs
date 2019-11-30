using Game.Noticfacation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCollisoon : MonoBehaviour {

	// Use this for initialization
    public GameObject shouji;

    public GameObject selfObj;
	void Start () {

    }

    public void initdata(float t) {
        Invoke("finish", t);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject targetObj = other.gameObject;
        
        if (selfObj == targetObj)
            return;


        if (targetObj.tag == "boom")
        {
            playHitEff();
            targetObj.GetComponent<Boom>().detonateBoom();
            finish();
        }
        else if (targetObj.tag == "pohuai")
        {
            playHitEff();
            if (other.gameObject.GetComponent<EnemyAI>())
            {
                //Destroy(other.gameObject.GetComponent<EnemyAI>().enemyview);
                //Destroy(targetObj);
                Notice.MonsterBeATK.broadcast("", targetObj);
            }
            finish();
        }
        else if (targetObj.tag == "diaoluo")
        {
            playHitEff();
            Vector3 v3 = targetObj.transform.position;
            Game.Global.Assets.instance.loadPrefab(
            "prop/cube",
            (GameObject object_, string name) =>
            {
                object_.transform.position = v3;
            });
            Destroy(targetObj);
            finish();
        } else if (gameObject.tag == "trap" && other.gameObject.tag == "Player") {
            Notice.WeaponCollision.broadcast("");
        }
        else if (other.gameObject.tag == "trap")
        {
        }
        else if (other.gameObject.tag == "Player")
        {
            Notice.WeaponCollision.broadcast("");
            playHitEff();
            finish();
        }

        else if (targetObj != null && other.gameObject.tag != "Player")
        {
            if (other.gameObject.GetComponent<MonsterEntity>())
            {
                //Destroy(other.gameObject.GetComponent<EnemyAI>().enemyview);
                //Destroy(targetObj);
                Notice.MonsterBeATK.broadcast("", targetObj);
            }
            //Debug.Log(targetObj);
            //Destroy(targetObj);
            playHitEff();
            finish();
        }


    }

    private void playHitEff() {
        if (shouji)
        {
            GameObject obj = GameObject.Instantiate(shouji, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(obj, .5f);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}


    private void finish()
    {
        Destroy(this.gameObject);
    }
}
