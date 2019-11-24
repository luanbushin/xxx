using Game;
using Game.Noticfacation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoNotice
{
    public static MapManager Instance;

    private Dictionary<Vector3, GameObject> mapObj = new Dictionary<Vector3, GameObject>();

    private GameObject[] mapItemList;
    private string[] boxNames;
    public GameObject mapparent;

    public bool mazeMode = false;
    // Use this for initialization

    public MapConfig mapConfig;
    
    private void Awake()
    {
        Instance = this;
    }


    void Start() {
        mapConfig = new MapConfig();
        boxNames = mapConfig.boxNames;
        mapItemList = mapConfig.mapItemList;
        addListener(Notice.MAZE_CREAT_COMPLETE, (maplist) =>
        {
            for (int i = 0; i < maplist.GetLength(0); i++)
            {
                for (int j = 0; j < maplist.GetLength(1); j++)
                {
                    if (maplist[i, j] != 1)
                    {
                        GameObject obj = GameObject.Instantiate(mapItemList[UnityEngine.Random.Range(0, 4)], new Vector3(i, 1, j), gameObject.transform.rotation);
                        obj.transform.SetParent(mapparent.transform);
                        mapObj[obj.transform.position] = obj;
                    }
                }
            }
        });
    }



    public void initmap(Dictionary<Vector3, int> mapIndexObj)
    {
        //MapGenerate.Instance.creatDungeon();
        //return;

        if (mazeMode)
        {
            for (int i = 0; i < 199; i++)
            {
                for (int j = 0; j < 199; j++)
                {
                    GameObject obj = GameObject.Instantiate(mapItemList[getBaseFloor()], new Vector3(i, 0, j), gameObject.transform.rotation);
                    obj.transform.SetParent(mapparent.transform);
                    mapObj[obj.transform.position] = obj;
                }
            }
            //MapGenerate.Instance.creatBaseMap(200, 200);

            MapGenerate.Instance.creatDungeon(199,199,200,10,5);


        }
        else
        {
            foreach (Vector3 list in mapIndexObj.Keys)
            {
                GameObject obj = GameObject.Instantiate(mapItemList[mapIndexObj[list]], list, gameObject.transform.rotation);
                obj.transform.SetParent(mapparent.transform);
                mapObj[obj.transform.position] = obj;
            }
        }
    }
    private int getBaseFloor(){
        int[] arr = { 0,0,0,0,0,0,2,1,2,2,3,9,10,5,6,7};
        return arr[UnityEngine.Random.Range(0, arr.Length)];
    }



    public GameObject getRandomWall() {
        int[] arr = {8, 12, 13, 14, 15 };
        return mapItemList[arr[UnityEngine.Random.Range(0, arr.Length)]];
    }
    public GameObject getRandomFlower()
    {
        return mapConfig.flowersList[UnityEngine.Random.Range(0, mapConfig.flowersList.Length)];
    }

    public void diaoluo(List<Vector3> list)
    {
        for (var i = 0; i < list.Count; i++)
        {
            Debug.Log(mapObj.ContainsKey(list[i]));
            if (mapObj.ContainsKey(list[i]))
            {
                //mapObj[list[i]].GetComponent<Rigidbody>().useGravity = true;
                mapObj[list[i]].AddComponent<Rigidbody>();
            }
        }
    }

    public GameObject getMapPrefabsByName(string name) {
        return (GameObject)Resources.Load("map/Prefabs/" + name);
    }


    public Vector3 pathfinding(Vector3 fromv3,Vector3 tov3) {
        if (isLineArrive(fromv3, tov3))
        {
            return tov3 - fromv3;
        }
        else {
            /*return Vector3.zero;
            List<List<Vector3>> allway = new List<List<Vector3>>();
            List<Vector3> list = new List<Vector3>();
            list.Add(fromv3);
            findV3(allway,list, tov3);


            Debug.Log(allway);*/
            return Vector3.zero;
        }
    }

    public void findV3(List<List<Vector3>> allway,List<Vector3> list, Vector3 target)
    {
        if (allway.Count > 0)
            return;
        Vector3 curV3 = list[list.Count - 1];

        /*if (curV3.x > target.x)
        {
            if (othervox("r", curV3))
            {
                Vector3 nextV3 = new Vector3(curV3.x + 1, curV3.y, curV3.z);
                list.Add(nextV3);
                if (nextV3.x == target.x && nextV3.z == target.z)
                {
                    allway.Add(list);
                }
                else {
                    findV3(allway,new List<Vector3>(list), target);
                }
              
            }
            if (othervox("l", curV3))
            {
                Vector3 nextV3 = new Vector3(curV3.x - 1, curV3.y, curV3.z);
                list.Add(nextV3);
                if (nextV3.x == target.x && nextV3.z == target.z)
                {
                    allway.Add(list);
                }
                else
                {
                    findV3(allway, new List<Vector3>(list), target);
                }
            }
            if (curV3.z > target.z)
            {
                if (othervox("t", curV3))
                {
                    Vector3 nextV3 = new Vector3(curV3.x, curV3.y, curV3.z+1);
                    list.Add(nextV3);
                    if (nextV3.x == target.x && nextV3.z == target.z)
                    {
                        allway.Add(list);
                    }
                    else
                    {
                        findV3(allway, new List<Vector3>(list), target);
                    }
                }
                if (othervox("d", curV3))
                {
                    Vector3 nextV3 = new Vector3(curV3.x, curV3.y, curV3.z - 1);
                    list.Add(nextV3);
                    if (nextV3.x == target.x && nextV3.z == target.z)
                    {
                        allway.Add(list);
                    }
                    else
                    {
                        findV3(allway, new List<Vector3>(list), target);
                    }
                }
            }
            else
            {
                if (othervox("d", curV3))
                {
                    Vector3 nextV3 = new Vector3(curV3.x, curV3.y, curV3.z - 1);
                    list.Add(nextV3);
                    if (nextV3.x == target.x && nextV3.z == target.z)
                    {
                        allway.Add(list);
                    }
                    else
                    {
                        findV3(allway, new List<Vector3>(list), target);
                    }
                }
                if (othervox("t", curV3))
                {
                    Vector3 nextV3 = new Vector3(curV3.x, curV3.y, curV3.z + 1);
                    list.Add(nextV3);
                    if (nextV3.x == target.x && nextV3.z == target.z)
                    {
                        allway.Add(list);
                    }
                    else
                    {
                        findV3(allway, new List<Vector3>(list), target);
                    }
                }
            }
        }
        else if (curV3.x == target.x)
        {
            if (curV3.z > target.z)
            {
                if (othervox("t", curV3))
                {
                    Vector3 nextV3 = new Vector3(curV3.x, curV3.y, curV3.z + 1);
                    list.Add(nextV3);
                    if (nextV3.x == target.x && nextV3.z == target.z)
                    {
                        allway.Add(list);
                    }
                    else
                    {
                        findV3(allway, new List<Vector3>(list), target);
                    }
                }
                if (othervox("d", curV3))
                {
                    Vector3 nextV3 = new Vector3(curV3.x, curV3.y, curV3.z - 1);
                    list.Add(nextV3);
                    if (nextV3.x == target.x && nextV3.z == target.z)
                    {
                        allway.Add(list);
                    }
                    else
                    {
                        findV3(allway, new List<Vector3>(list), target);
                    }
                }
            }
            else
            {
                if (othervox("d", curV3))
                {
                    Vector3 nextV3 = new Vector3(curV3.x, curV3.y, curV3.z - 1);
                    list.Add(nextV3);
                    if (nextV3.x == target.x && nextV3.z == target.z)
                    {
                        allway.Add(list);
                    }
                    else
                    {
                        findV3(allway, new List<Vector3>(list), target);
                    }
                }
                if (othervox("t", curV3))
                {
                    Vector3 nextV3 = new Vector3(curV3.x, curV3.y, curV3.z + 1);
                    list.Add(nextV3);
                    if (nextV3.x == target.x && nextV3.z == target.z)
                    {
                        allway.Add(list);
                    }
                    else
                    {
                        findV3(allway, new List<Vector3>(list), target);
                    }
                }
            }
            if (othervox("r", curV3))
            {
                Vector3 nextV3 = new Vector3(curV3.x + 1, curV3.y, curV3.z);
                list.Add(nextV3);
                if (nextV3.x == target.x && nextV3.z == target.z)
                {
                    allway.Add(list);
                }
                else
                {
                    findV3(allway, new List<Vector3>(list), target);
                }

            }
            if (othervox("l", curV3))
            {
                Vector3 nextV3 = new Vector3(curV3.x - 1, curV3.y, curV3.z);
                list.Add(nextV3);
                if (nextV3.x == target.x && nextV3.z == target.z)
                {
                    allway.Add(list);
                }
                else
                {
                    findV3(allway, new List<Vector3>(list), target);
                }
            }
        }
        else
        {
            if (othervox("l", curV3))
            {
                Vector3 nextV3 = new Vector3(curV3.x - 1, curV3.y, curV3.z);
                list.Add(nextV3);
                if (nextV3.x == target.x && nextV3.z == target.z)
                {
                    allway.Add(list);
                }
                else
                {
                    findV3(allway, new List<Vector3>(list), target);
                }
            }
            if (othervox("r", curV3))
            {
                Vector3 nextV3 = new Vector3(curV3.x + 1, curV3.y, curV3.z);
                list.Add(nextV3);
                if (nextV3.x == target.x && nextV3.z == target.z)
                {
                    allway.Add(list);
                }
                else
                {
                    findV3(allway, new List<Vector3>(list), target);
                }
            }
            if (curV3.z > target.z)
            {
                if (othervox("d", curV3))
                {
                    Vector3 nextV3 = new Vector3(curV3.x, curV3.y, curV3.z - 1);
                    list.Add(nextV3);
                    if (nextV3.x == target.x && nextV3.z == target.z)
                    {
                        allway.Add(list);
                    }
                    else
                    {
                        findV3(allway, new List<Vector3>(list), target);
                    }
                }
                if (othervox("t", curV3))
                {
                    Vector3 nextV3 = new Vector3(curV3.x, curV3.y, curV3.z + 1);
                    list.Add(nextV3);
                    if (nextV3.x == target.x && nextV3.z == target.z)
                    {
                        allway.Add(list);
                    }
                    else
                    {
                        findV3(allway, new List<Vector3>(list), target);
                    }
                }
            }
            else
            {
                if (othervox("t", curV3))
                {
                    Vector3 nextV3 = new Vector3(curV3.x, curV3.y, curV3.z + 1);
                    list.Add(nextV3);
                    if (nextV3.x == target.x && nextV3.z == target.z)
                    {
                        allway.Add(list);
                    }
                    else
                    {
                        findV3(allway, new List<Vector3>(list), target);
                    }
                }
                if (othervox("d", curV3))
                {
                    Vector3 nextV3 = new Vector3(curV3.x, curV3.y, curV3.z - 1);
                    list.Add(nextV3);
                    if (nextV3.x == target.x && nextV3.z == target.z)
                    {
                        allway.Add(list);
                    }
                    else
                    {
                        findV3(allway, new List<Vector3>(list), target);
                    }
                }
            }
        }*/
    }
    public bool othervox(string dic, Vector3 v3) {
        switch (dic) {
            case "l":
                if (mapObj.ContainsKey(new Vector3(v3.x - 1, v3.y, v3.z))) {
                    return false;
                }
                break;
            case "r":
                if (mapObj.ContainsKey(new Vector3(v3.x + 1, v3.y, v3.z)))
                {
                    return false;
                }
                break;
            case "t":
                if (mapObj.ContainsKey(new Vector3(v3.x, v3.y, v3.z+1)))
                {
                    return false;
                }
                break;
            case "d":
                if (mapObj.ContainsKey(new Vector3(v3.x, v3.y, v3.z-1)))
                {
                    return false;
                }
                break;
        }
        return true;
    }

    public bool isLineArrive(Vector3 mv3, Vector3 target) {
        RaycastHit hitt = new RaycastHit();
        Vector3 v3 = target - mv3;
        Physics.Raycast(mv3, v3.normalized, out hitt);
        //Debug.Log(Physics.Raycast(mv3, v3.normalized, out hitt));
        //Debug.Log(hitt.transform.tag == "Player");
        
        if (hitt.transform&&hitt.transform.position.x == target.x&& hitt.transform.position.z == target.z) {
            return true;
        }



        return false;
    }





    // Update is called once per frame
    void Update () {
        //debugTxt1.text = mapItemList.Length+"";
        //debugTxt2.text = boxNames.ToString() ;
	}
}
