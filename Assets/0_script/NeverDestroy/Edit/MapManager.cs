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







    // Update is called once per frame
    void Update () {
        //debugTxt1.text = mapItemList.Length+"";
        //debugTxt2.text = boxNames.ToString() ;
	}
}
