using UnityEngine;
using System.Collections;
using Game;
using Game.Noticfacation;
using System;

public class SkillManager : MonoNotice
{
    public static SkillManager Instance;

    public plyaer player;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        player = GameMain.Instance.player.GetComponent<plyaer>();
        addListener(Notice.Click_Use_Skill, (id) =>
        {
            if (id == "Space")
            {
                GameObject obj = GameObject.Instantiate(player.shootObject, player.transform.position, player.transform.Find("430").transform.rotation);
                obj.GetComponent<ShootCollisoon>().selfObj = player.gameObject;
                player.anim.CrossFade("atk", 0.08f);
                obj.GetComponent<Rigidbody>().AddForce(obj.transform.forward * 1000);
                player.setPlaterState(1, 0.5f, "idle");
                //Debug.Log(1111111);
                //useSkillByData();
                /**player.anim.CrossFade("dance", 0.08f);
                player.setPlaterState(1, 5.3f, "idle");

                SkillData data = new SkillData();
                data.id = 10002;
                useSkillByData(data, player.gameObject);*/
            }
            if (id == "J")
            {
                //speeduptime = 100;
                float angle = player.selfrotation;

                Vector3 v3 = new Vector3((float)Math.Sin(angle * Math.PI / 180), 0, (float)Math.Cos(angle * Math.PI / 180));
                StartCoroutine(CreateExplosions(v3));
                //float num = Mathf.Cos(Mathf.PI / 4);
                /**switch (player.playerRotion + "")
                {
                    case "0":
                        StartCoroutine(CreateExplosions(Vector3.forward));
                        break;
                    case "45":
                        StartCoroutine(CreateExplosions((Vector3.forward + Vector3.right) * num));
                        break;
                    case "90":
                        StartCoroutine(CreateExplosions(Vector3.right));
                        break;
                    case "135":
                        StartCoroutine(CreateExplosions((Vector3.back + Vector3.right) * num));
                        break;
                    case "180":
                        StartCoroutine(CreateExplosions(Vector3.back));
                        break;
                    case "225":
                        StartCoroutine(CreateExplosions((Vector3.back + Vector3.left) * num));
                        break;
                    case "270":
                        StartCoroutine(CreateExplosions(Vector3.left));
                        break;
                    case "315":
                        StartCoroutine(CreateExplosions((Vector3.forward + Vector3.left) * num));
                        break;
                }*/
            }
            if (id == "L")
            {
                /**GameObject obj = GameObject.Instantiate(player.shootObject, player.transform.position, player.transform.Find("430").transform.rotation);
                obj.GetComponent<ShootCollisoon>().selfObj = player.gameObject;
                player.anim.CrossFade("atk", 0.08f);
                obj.GetComponent<Rigidbody>().AddForce(obj.transform.forward * 1000);
                player.setPlaterState(1, 0.5f, "idle");*/
                SkillData data = new SkillData();
                data.id = 10001;
                useSkillByData(data, player.gameObject);
            }
            if (id == "K")
            {
                Vector3 v3 = new Vector3(Mathf.Round(player.transform.position.x), player.transform.position.y - 0.8f, Mathf.Round(player.transform.position.z)); ;
                GameObject obj = GameObject.Instantiate(getEff("map/Prefabs/Box_E1"), v3, transform.rotation);

            }
            if (id == "I")
            {
                GameObject obj = GameObject.Instantiate(player.fanwei, player.transform.position+new Vector3(0,0.2f,0), transform.rotation);
                obj.transform.Translate(new Vector3(0, -0.8f, 0));
                obj.transform.SetParent(player.transform);
                Destroy(obj, 3);
            }
        });
    }

    public void useSkillByData(SkillData data,GameObject target) {
        switch (data.id) {
            case 10001:
                Vector3 v3 = new Vector3(Mathf.Round(target.transform.position.x), target.transform.position.y - 0.3f, Mathf.Round(target.transform.position.z));
                skill10001(v3);
                break;
            case 10002:
                target.GetComponent<plyaer>().collision.SetActive(true);
                Quaternion quaternion = target.transform.Find("430").transform.rotation;
                var eff = Instantiate(getEff("Prefabs/Particles/063_W_4"), target.transform.position,quaternion) as GameObject;
                Destroy(eff,1);
                break;
            case 10003:

                break;
            case 10004:

                break;
            case 10005:

                break;
            case 10006:

                break;
            case 10007:

                break;
            case 10008:

                break;
        }
    }

    public void skill10001(Vector3 v3) {
        //transform.rotation
        //new Vector3(Mathf.Round(gameObject.transform.position.x), gameObject.transform.position.y - 0.3f, Mathf.Round(gameObject.transform.position.z))
        GameObject.Instantiate(player.CreateObject,v3, new Quaternion(0,0,0,1));
    }


    public GameObject getEff(string str)
    {
        //return (GameObject)Resources.Load("enemy/"+str);.
        return (GameObject)Resources.Load(str);
    }

    private IEnumerator CreateExplosions(Vector3 direction)
    {
        for (int i = 1; i < 4; i++)
        {
            RaycastHit hit;

            Physics.Raycast(player.transform.position + new Vector3(0.0f, -0.1f, 0.0f), direction, out hit, 1, 1 << 0);

            if (!hit.collider)
            {
                player.transform.Translate(direction);
            }
            else
            {
                break;
            }
        }


        Vector3 v3 = player.transform.position;
        GameObject obj = GameObject.Instantiate(getEff("Prefabs/Particles/015_Q_5"), v3 - new Vector3(0,0.5f,0), transform.rotation);
        Destroy(obj, 3);
        yield return new WaitForSeconds(.05f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
