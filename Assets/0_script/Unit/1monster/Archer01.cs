using UnityEngine;
using System.Collections;

public class Archer01 :MonsterEntity
{

    // Use this for initialization
    private GameObject shootObject;
    void Start()
    {

        target = GameMain.Instance.player;

        int random = UnityEngine.Random.Range(1, 3);
        loadModel("enemy/Archer_0" + random);

        shootObject = (GameObject)Resources.Load("Prefabs/ShootCollisoon 1");

        state = 0;
    }

    public void shoot() {
        anim.CrossFade("AttackRange2", 0.08f);
        Invoke("creatShoot", 0.3f);
    }

    public void creatShoot() {
        GameObject obj = GameObject.Instantiate(shootObject, gameObject.transform.position + new Vector3(0, 0.5f, 0), transform.rotation);
        obj.GetComponent<ShootCollisoon>().selfObj = gameObject;
        obj.GetComponent<Rigidbody>().AddForce(obj.transform.forward * 1000);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0 && chackAttack()) {
            Vector3 preDir = new Vector3(transform.position.x, 0, transform.position.z) - new Vector3(target.transform.position.x, 0, target.transform.position.z);
            preDir = preDir.normalized;
            transform.rotation = Quaternion.FromToRotation(Vector3.back, preDir);


            shoot();
            state = 1;
            Invoke("backatk", 2);
        }
    }
}
