using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Dragon : MonsterEntity
{
    void Start()
    {
        loadModel("Prefabs/Characters/25200");
        target = GameMain.Instance.player;

    }

    void Update()
    {
        if (state == 0)
        {
            if (chackAttack())
            {

                state = 1;
                anim.CrossFade("atk", 0.08f);
                Invoke("atk", 0.2f);
                Invoke("atkesc", 0.8f);
                Invoke("backatk", 1);
            }
            else if (chackAttackRange())
            {
                Vector3 v3 = MapManager.Instance.pathfinding(new Vector3(transform.position.x, 1.5f, transform.position.z), new Vector3(target.transform.position.x, 1.5f, target.transform.position.z));
                Vector3 preDir = new Vector3(transform.position.x, 0, transform.position.z) - new Vector3(v3.x, 0, v3.z);
                Vector3 moveV3 = v3 - new Vector3(transform.position.x, 1.5f, transform.position.z);

                transform.position += moveV3.normalized * 3 * Time.deltaTime;
                preDir = preDir.normalized;
                transform.rotation = Quaternion.FromToRotation(Vector3.back, preDir);

                anim.CrossFade("run", 0.08f);
            }
        }
    }
}
