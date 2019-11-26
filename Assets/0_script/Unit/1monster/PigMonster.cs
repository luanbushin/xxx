﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class PigMonster: MonsterEntity
{
    private GameObject collision;

    void Start()
    {
        animAction = "idle";
        target = GameMain.Instance.player;

        loadModel("Prefabs/Characters/25501");
        anim.CrossFade("idle", 0.08f);

        model.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        collision = GameObject.CreatePrimitive(PrimitiveType.Cube);
        collision.transform.localScale = new Vector3(0.8f,0.5f,0.8f);
        collision.transform.position = transform.position + new Vector3(0, 0.5f, 0.4f);
        collision.transform.SetParent(transform);
        collision.SetActive(false);
        collision.GetComponent<BoxCollider>().isTrigger = true;
        collision.AddComponent<CollisionLogic>();
        collision.GetComponent<MeshRenderer>().enabled = false;;
    }

    public void backatk()
    {
        base.backatk();
    }

    public void atkesc() 
    {
        collision.SetActive(false);
    }

    public void atk()
    {
        collision.GetComponent<CollisionLogic>().init(gameObject.name+Time.time);
        collision.SetActive(true);
    }

    void Update()
    {
        if(state == 0)
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

                //transform.Translate(Vector3.right * Time.deltaTime * 1);
                //Vector3 preDir = new Vector3(transform.position.x, 0, transform.position.z) - new Vector3(target.transform.position.x, 0, target.transform.position.z);
                Vector3 preDir = new Vector3(transform.position.x, 0, transform.position.z) - new Vector3(v3.x, 0, v3.z);
                //Vector3 v3 = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
                //Vector3 mv3 = new Vector3(transform.position.x, 1.5f, transform.position.z);
                //Vector3 tv3 = new Vector3(target.transform.position.x, 1.5f, target.transform.position.z);
                //RaycastHit hitt = new RaycastHit();

                //Debug.Log(Physics.Raycast(mv3, v3.normalized, out hitt));
                //Debug.Log(hitt.transform.tag == "Player");

                //Debug.DrawLine(mv3, tv3);
                Vector3 moveV3 = v3 - new Vector3(transform.position.x, 1.5f, transform.position.z);

                transform.position += moveV3.normalized * 3 * Time.deltaTime;
                preDir = preDir.normalized;

                transform.rotation = Quaternion.FromToRotation(Vector3.back, preDir);

               

                anim.CrossFade("run", 0.08f);
            }
        }
    }


}

