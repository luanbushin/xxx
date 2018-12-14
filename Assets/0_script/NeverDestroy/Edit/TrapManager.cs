using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game.Config;
using Game;
using Game.Noticfacation;
using System.Timers;

public class TrapManager : MonoNotice
{
    public static TrapManager Instance;

    public List<GameObject> trapListPre;
    public GameObject trapparent;
    public GameObject placeGameObject;

    private Dictionary<int, GameObject> trapObject = new Dictionary<int, GameObject>();
    private Dictionary<GameObject, TrapVo> trapDataObject = new Dictionary<GameObject, TrapVo>();

    public int nextTrapId;
    private void Awake()
    {
        Instance = this;
    }

    public void deleteMonster(GameObject g) {
        MonsterRefreshVo trapvo = trapDataObject[g] as MonsterRefreshVo;
        trapvo.curLiveEnemy--;
    }

    // Use this for initialization
    void Start()
    {
        nextTrapId = 1;
        addListener(Notice.TrapCtrl_Vector, (string s,List<Vector3> v3) =>
        {
            string[] arr = s.Split('=');

            if (arr[1] == "4")
            {
                CubeTrap trap = trapDataObject[trapObject[int.Parse(arr[0])]] as CubeTrap;

                if (trap.AtkType == 1)
                {
                    MapManager.Instance.diaoluo(v3);
                }
                else if (trap.AtkType == 2)
                {

                }

            }
        });


        addListener(Notice.TrapCtrl, (string s) =>
        {
            string[] arr = s.Split('=');
            if (arr[1] == "1")
            {
                TrapDoor trap = trapDataObject[trapObject[int.Parse(arr[0])]] as TrapDoor;
                //trapDataObject[list[i].id] = trap;
                if (arr[2] == "trigger")
                {
                    if (trapObject[int.Parse(arr[0])].transform.position.y < 0)
                    {
                        trapObject[int.Parse(arr[0])].transform.Translate(new Vector3(0, 50, 0));

                        foreach (Transform child in trapObject[int.Parse(arr[0])].transform)
                        {
                            if (child.GetComponent<TrapCollisoon>())
                            {
                                if (child.GetComponent<TrapCollisoon>().use == "trigger")
                                {
                                    Destroy(child.gameObject);
                                }
                                else
                                {
                                    child.transform.Translate(new Vector3(0, -child.transform.position.y + 1, 0));
                                }
                            }
                        }
                    }
                }
                else if (arr[2] == "close")
                {
                    trap.curLiveEnemy++;
                    updataTrap(int.Parse(arr[0]));
                }
                else if (arr[2] == "open")
                {
                    trap.curLiveEnemy--;
                    updataTrap(int.Parse(arr[0]));
                }
            }
            else if (arr[1] == "4")
            {
                CubeTrap trap = trapDataObject[trapObject[int.Parse(arr[0])]] as CubeTrap;
                if (trap.AtkType == 2)
                {
                    if (arr[2] == "add")
                    {
                        GameMain.Instance.player.GetComponent<plyaer>().pushForceVector(arr[0], new Vector3(20,0,0));
                    }
                    else if (arr[2] == "remove")
                    {
                        GameMain.Instance.player.GetComponent<plyaer>().removeForceVector(arr[0]);
                    }
                }
            }
            else if (arr[1] == "5")
            {
                if (trapObject[int.Parse(arr[0])].transform.parent.gameObject == trapparent)
                {
                    if (trapObject[int.Parse(arr[0])].transform.position.ToString() == arr[2])
                    {
                        foreach (Transform child in trapObject[int.Parse(arr[0])].transform)
                        {
                            Vector3 v3 = child.position;
                            v3.y += .8001f;
                            GameMain.Instance.initPlayerPostion(v3);
                        }
                    }
                }
            }
            else if (arr[1] == "6")
            {
                if (arr[2] == "trigger")
                {
                    foreach (Transform child in trapObject[int.Parse(arr[0])].transform)
                    {
                        //if (child.GetComponent<TrapCollisoon>())
                        //{
                        //if (child.GetComponent<TrapCollisoon>().use == "trigger")
                        //{
                        Destroy(child.gameObject);
                        // }
                        // }
                    }
                    tirggerArr = arr;
                    Invoke("tirggerTrap", 0.5f);
                    removeTargetTrap = trapObject[int.Parse(arr[0])];
                    Invoke("removeTrap", 2f);
                }
                else if (arr[2] == "atk")
                {
                    Notice.TrapCollision.broadcast("");
                }
            }
            else if (arr[1] == "7")
            {
                TrapSpring trap = trapDataObject[trapObject[int.Parse(arr[0])]] as TrapSpring;
                if (arr[2] == "trigger")
                {
                    if (trap.curRange != -1)
                        return;

                    trap.curRange = 1;

                    GameObject obj1 = GameObject.Instantiate(placeGameObject, trap.v3, placeGameObject.transform.rotation);
                    obj1.transform.localScale = new Vector3(1, .8f, 1);
                    obj1.transform.Translate(new Vector3(-trap.curRange / 2 + .5f, .45f, 0));
                    obj1.GetComponent<TrapCollisoon>().id = int.Parse(arr[0]);
                    obj1.GetComponent<TrapCollisoon>().type = 7;
                    obj1.GetComponent<TrapCollisoon>().use = "atk";
                    obj1.transform.SetParent(trapObject[int.Parse(arr[0])].transform);

                    obj1.transform.Rotate(0, trap.rotiony, 0);
                }
                else if (arr[2] == "atk" && trap.isshrink == false)
                {
                    Vector3 v3;

                    if (trap.rotiony == 90)
                    {
                        v3 = new Vector3(0, 0, 1);
                    }
                    else if (trap.rotiony == 180)
                    {
                        v3 = new Vector3(1, 0, 0);
                    }
                    else if (trap.rotiony == 270)
                    {
                        v3 = new Vector3(0, 0, -1);
                    }
                    else
                    {
                        v3 = new Vector3(-1, 0, 0);
                    }


                    GameMain.Instance.initPlayerForce(v3 * 400);
                    //Notice.TrapCollision.broadcast("");
                }
            }
        });
    }
    public GameObject removeTargetTrap;
    public string[] tirggerArr;
    public void tirggerTrap() {
        string[] arr = tirggerArr;
        if (arr[1] == "6") { 
            TrapLandmine trap = trapDataObject[trapObject[int.Parse(arr[0])]] as TrapLandmine;
            for (int j = 0; j < trap.clearPList.Count; j++)
            {
                GameObject obj1 = GameObject.Instantiate(placeGameObject, trap.clearPList[j], placeGameObject.transform.rotation);
                obj1.transform.localScale = trap.clearRList[j];
                obj1.GetComponent<TrapCollisoon>().id = int.Parse(arr[0]);
                obj1.GetComponent<TrapCollisoon>().type = 6;
                obj1.GetComponent<TrapCollisoon>().use = "atk";
                obj1.transform.SetParent(trapObject[int.Parse(arr[0])].transform);
            }

        }
    }
    public void removeTrap() {
        Destroy(removeTargetTrap);
    }


    public void updataTrap(int id) {
        if (trapDataObject[trapObject[id]].type == 1)
        {
            TrapDoor trap = trapDataObject[trapObject[id]] as TrapDoor;
            Debug.Log(trap.curLiveEnemy);
            if (trap.curLiveEnemy > 0)
            {
                trapObject[id].GetComponent<BoxCollider>().isTrigger = false;
            }
            else
            {
                trapObject[id].GetComponent<BoxCollider>().isTrigger = true;
            }
        }
        else if (trapDataObject[trapObject[id]].type == 6)
        {

        }

    }


    public void inittrap(List<TrapVo> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].id = nextTrapId;
            nextTrapId++;
            if (list[i].type == 1)
            {
                TrapDoor trap = list[i] as TrapDoor;
                trap.curLiveEnemy = 0;
                GameObject obj = GameObject.Instantiate(trapListPre[0], trap.v3, trapListPre[0].transform.rotation);
                trapObject[list[i].id] = obj;
                trapDataObject[trapObject[list[i].id]] = trap;
                obj.transform.SetParent(trapparent.transform);
                if (trap.triggerPList.Count > 0)
                    obj.transform.Translate(new Vector3(0, -50, 0));

                for (int j = 0; j < trap.triggerPList.Count; j++)
                {
                    GameObject obj1 = GameObject.Instantiate(placeGameObject, trap.triggerPList[j], placeGameObject.transform.rotation);
                    obj1.transform.localScale = trap.triggerRList[j];
                    obj1.GetComponent<TrapCollisoon>().id = list[i].id;
                    obj1.GetComponent<TrapCollisoon>().type = 1;
                    obj1.GetComponent<TrapCollisoon>().use = "trigger";
                    obj1.transform.SetParent(obj.transform);
                }

                for (int j = 0; j < trap.clearPList.Count; j++)
                {
                    GameObject obj1 = GameObject.Instantiate(placeGameObject, trap.clearPList[j], placeGameObject.transform.rotation);
                    obj1.transform.localScale = trap.clearRList[j];
                    obj1.GetComponent<TrapCollisoon>().id = list[i].id;
                    obj1.GetComponent<TrapCollisoon>().type = 1;
                    obj1.GetComponent<TrapCollisoon>().use = "clear";
                    obj1.transform.SetParent(obj.transform);
                }
                updataTrap(nextTrapId - 1);
            }
            else if (list[i].type == 2)
            {
                MonsterRefreshVo trap = list[i] as MonsterRefreshVo;
                trap.curLiveEnemy = 0;
                trap.currefreshindex = 0;
                GameObject obj = GameObject.Instantiate(trapListPre[1], trap.v3, trapListPre[1].transform.rotation);
                trapObject[list[i].id] = obj;
                trapDataObject[trapObject[list[i].id]] = trap;
                obj.transform.SetParent(trapparent.transform);
            }
            else if (list[i].type == 3)
            {
                AtkOffice trap = list[i] as AtkOffice;
                GameObject obj = GameObject.Instantiate(trapListPre[2], trap.v3, trapListPre[1].transform.rotation);
                trapObject[list[i].id] = obj;
                trapDataObject[trapObject[list[i].id]] = trap;
                obj.transform.SetParent(trapparent.transform);
            }
            else if (list[i].type == 4)
            {
                CubeTrap trap = list[i] as CubeTrap;
                GameObject obj = GameObject.Instantiate(placeGameObject, trap.v3, trapListPre[1].transform.rotation);
                obj.transform.localScale = trap.sv3;
                obj.GetComponent<TrapCollisoon>().id = list[i].id;
                obj.GetComponent<TrapCollisoon>().type = 4;
                obj.GetComponent<TrapCollisoon>().use = "atk"+ trap.AtkType;
                trapObject[list[i].id] = obj;
                trapDataObject[trapObject[list[i].id]] = trap;
                obj.transform.SetParent(trapparent.transform);
            }
            else if (list[i].type == 5)
            {
                Transfer trap = list[i] as Transfer;
                GameObject obj = GameObject.Instantiate(trapListPre[3], trap.v3, trapListPre[3].transform.rotation);
                obj.AddComponent<TrapCollisoon>().id = list[i].id;
                obj.GetComponent<TrapCollisoon>().type = 5;
                trapObject[list[i].id] = obj;
                trapDataObject[trapObject[list[i].id]] = trap;
                obj.transform.SetParent(trapparent.transform);

                GameObject obj1 = GameObject.Instantiate(trapListPre[3], trap.tv3, trapListPre[3].transform.rotation);
                obj1.AddComponent<TrapCollisoon>().id = list[i].id;
                obj1.GetComponent<TrapCollisoon>().type = 5;
                obj1.transform.SetParent(obj.transform);
            }
            else if (list[i].type == 6)
            {
                TrapLandmine trap = list[i] as TrapLandmine;
                trap.curLiveEnemy = 0;
                GameObject obj = GameObject.Instantiate(trapListPre[5], trap.v3, trapListPre[0].transform.rotation);
                trapObject[list[i].id] = obj;
                trapDataObject[trapObject[list[i].id]] = trap;
                obj.transform.SetParent(trapparent.transform);


                for (int j = 0; j < trap.triggerPList.Count; j++)
                {
                    GameObject obj1 = GameObject.Instantiate(placeGameObject, trap.triggerPList[j], placeGameObject.transform.rotation);
                    obj1.transform.localScale = trap.triggerRList[j];
                    obj1.GetComponent<TrapCollisoon>().id = list[i].id;
                    obj1.GetComponent<TrapCollisoon>().type = 6;
                    obj1.GetComponent<TrapCollisoon>().use = "trigger";
                    obj1.transform.SetParent(obj.transform);
                }


                // updataTrap(nextTrapId - 1);
            }
            else if (list[i].type == 7)
            {
                TrapSpring trap = list[i] as TrapSpring;
                GameObject obj = GameObject.Instantiate(trapListPre[5], trap.v3, trapListPre[0].transform.rotation);
                trapObject[list[i].id] = obj;
                trapDataObject[trapObject[list[i].id]] = trap;
                obj.transform.SetParent(trapparent.transform);

                GameObject obj1 = GameObject.Instantiate(placeGameObject, trap.v3, placeGameObject.transform.rotation);
                obj1.transform.localScale = new Vector3(trap.range,0.1f,1);
                obj1.transform.Translate(new Vector3(-trap.range/2+.5f,0,0));
                obj1.GetComponent<TrapCollisoon>().id = list[i].id;
                obj1.GetComponent<TrapCollisoon>().type = 7;
                obj1.GetComponent<TrapCollisoon>().use = "trigger";
                obj1.transform.SetParent(obj.transform);

                obj.transform.Rotate(0, trap.rotiony, 0) ;


                Debug.Log(trap.rotiony);
            }
        }
        //Timer time = new Timer(13, () => { Debug.Log("alwaysDo");
       // }, () => { Debug.Log("stopTimer1")};

    }


    // Update is called once per frame
    void Update()
    {
        foreach (GameObject trapobj in trapDataObject.Keys) {
            TrapVo trap = trapDataObject[trapobj];
            if (trap.type == 2)
            {
                MonsterRefreshVo trapvo = trap as MonsterRefreshVo;
                if (trapvo.curLiveEnemy < trapvo.maxLiveEnemy)
                {
                    trapvo.currefreshindex++;
                    if (trapvo.currefreshindex >= trapvo.refreshCD * 5)
                    {
                        trapvo.curLiveEnemy++;
                        Notice.EnemyCreat.broadcast("", trapObject[trap.id]);
                        trapvo.currefreshindex = 0;
                    }
                }
            }
            else if (trap.type == 3)
            {
                AtkOffice trapvo = trap as AtkOffice;
                trapvo.curcd++;
                if (trapvo.curcd >= trapvo.atkCD)
                {
                    trapvo.curcd = 0;

                    GameObject obj = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/ShootCollisoon 1"), trapObject[trap.id].transform.position, trapObject[trap.id].transform.rotation);
                    obj.tag = "trap";
                    obj.GetComponent<ShootCollisoon>().initdata(trapvo.rang / trapvo.speed);
                    obj.transform.Translate(new Vector3(0, 0.5f, 0));
                    obj.GetComponent<Rigidbody>().AddForce(obj.transform.forward * 1000);
                    obj.transform.SetParent(trapObject[trap.id].transform);
                }
            }
            else if(trap.type == 7){
                TrapSpring trapvo = trap as TrapSpring;
                if (trapvo.isshrink)
                {
                    //Debug.Log(trapvo.curRange);
                    trapvo.curRange -= 0.5f;
                    if (trapvo.curRange <= 0)
                    {
                        foreach (Transform child in trapobj.transform)
                        {
                            if (child.GetComponent<TrapCollisoon>())
                            {
                                if (child.GetComponent<TrapCollisoon>().use == "trigger")
                                {
                                    // Destroy(child.gameObject);
                                }
                                else
                                {
                                    trapvo.isshrink = false;
                                    trapvo.curRange = -1;
                                    Destroy(child.gameObject);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (Transform child in trapobj.transform)
                        {
                            if (child.GetComponent<TrapCollisoon>())
                            {
                                if (child.GetComponent<TrapCollisoon>().use == "trigger")
                                {
                                    // Destroy(child.gameObject);
                                }
                                else
                                {
                                    child.transform.localScale = new Vector3(trapvo.curRange, 0.1f, 1);
                                    child.transform.Translate(new Vector3(0.25f, 0, 0));
                                }
                            }
                        }
                    }
                }
                else if (trapvo.curRange != -1) {
                    if (trapvo.curRange < trapvo.range)
                    {
                        trapvo.curRange += 0.5f;
                        foreach (Transform child in trapobj.transform)
                        {
                            if (child.GetComponent<TrapCollisoon>())
                            {
                                if (child.GetComponent<TrapCollisoon>().use == "trigger")
                                {
                                    // Destroy(child.gameObject);
                                }
                                else
                                {
                                    child.transform.localScale = new Vector3(trapvo.curRange, 0.1f, 1);
                                    child.transform.Translate(new Vector3(-0.25f, 0, 0));
                                }
                            }
                        }
                    }
                    else {
                        trapvo.isshrink = true;
                    }
                }  
            }
        }

    }
}
