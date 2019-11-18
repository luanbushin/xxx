using UnityEngine;
using System.Collections;

public class Archer01 :MonsterEntity
{

    // Use this for initialization
    private GameObject shootObject;
    void Start()
    {
        int random = UnityEngine.Random.Range(1, 2);
        loadModel("Archer_0" + random);

        shootObject = (GameObject)Resources.Load("Prefabs/ShootCollisoon");
    }

    public void shoot() {
        GameObject obj = GameObject.Instantiate(shootObject, gameObject.transform.position, transform.rotation);
        //anim.CrossFade("skill_q", 0.08f);
        obj.GetComponent<Rigidbody>().AddForce(obj.transform.forward * 1000);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
