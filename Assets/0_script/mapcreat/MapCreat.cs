﻿using Game.Global;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MapCreat : MonoBehaviour
{
    public Button saveBtn;
    public Button loadBtn;

    public Button cancelBtn;
    public Button newBtn;
    public Button changeBtn;
    public Button deleteBtn;


    public GameObject chooseBuild;
    public GameObject creatPanel;
    public Text widthTxt;
    public Text heightTxt;
    public Button creatBtn;

    public GameObject map;

    private GameObject[] mapItemList;
    public Dropdown boxList;

    private GameObject curNewObj;
    private GameObject curChangeObj;
    private GameObject targetChangObj;

    public GameObject surface;

    private bool isDelete = false;

    private Dictionary<Vector3, GameObject> mapObject = new Dictionary<Vector3, GameObject>();
    private Dictionary<Vector3, int> mapIndexObject = new Dictionary<Vector3, int>();

    public int curIndex = 0;

    private string[] boxNames;

 

    // Use this for initialization
    void Start()
    {
        Debug.Log("===================");
        Debug.Log(Application.streamingAssetsPath);

        creatBtn.onClick.AddListener(creatMap);

        saveBtn.onClick.AddListener(onSaveMap);
        loadBtn.onClick.AddListener(onLoadMap);

        cancelBtn.onClick.AddListener(onCanelBuild);
        newBtn.onClick.AddListener(onNewBuild);
        changeBtn.onClick.AddListener(onChangeBuild);
        deleteBtn.onClick.AddListener(onDleleteBuild);

        getBoxNames();
    }
    private void getBoxNames() {
        string boxname = "";

        string fullPath = "Assets/Resources/map/Prefabs" + "/";





        boxList.ClearOptions();

        if (Directory.Exists(fullPath))
        {
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

          
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }

                string str = files[i].Name.Replace(".prefab", "");

                if (boxname.Length > 0)
                    boxname += ";";
                boxname += str;
                
                Dropdown.OptionData op = new Dropdown.OptionData();
                op.text = str;
                boxList.options.Add(op);
            }
            boxNames = boxname.Split(';');
            Debug.Log(boxNames);
        }

        mapItemList = new GameObject[boxNames.Length];

        for (int i = 0; i < boxNames.Length; i++) {
            mapItemList[i] = (GameObject)Resources.Load("map/Prefabs/" + boxNames[i]);
        }
        
        boxList.onValueChanged.AddListener(onValueChanged);
        boxList.value = 1;
        boxList.value = 0;
    }

    private void onValueChanged(int arg0)
    {
        curIndex = boxList.value;
        if (curNewObj)
        {
            if (curIndex > mapItemList.Length - 1)
                curIndex = mapItemList.Length - 1;
            Destroy(curNewObj);
            curNewObj = GameObject.Instantiate(mapItemList[curIndex], Vector3.zero, gameObject.transform.rotation);
            curNewObj.layer = 2;
        }
        else if (curChangeObj)
        {
            if (curIndex > mapItemList.Length - 1)
                curIndex = mapItemList.Length - 1;
            Destroy(curChangeObj);
            curChangeObj = GameObject.Instantiate(mapItemList[curIndex], Vector3.zero, gameObject.transform.rotation);
            curChangeObj.layer = 2;
        }
    }


    private void onDleleteBuild()
    {
        isDelete = true;
    }
    private void onCanelBuild()
    {
        isDelete = false;
        if (curChangeObj)
        {
            Destroy(curChangeObj);
            curChangeObj = null;
        }
        if (curNewObj)
        {
            Destroy(curNewObj);
            curNewObj = null;
        }
        if (targetChangObj)
        {
            targetChangObj.GetComponent<MeshRenderer>().enabled = true;
            targetChangObj = null;
        }
    }

    private void creatBuildList() {
        for (int i = 0; i < mapItemList.Length; i++) {
            GameObject obj = GameObject.Instantiate(mapItemList[i], Vector3.zero, gameObject.transform.rotation);
            obj.transform.position = new Vector3(i*2,-25,0);
        }
    }
    private void onNewBuild()
    {
        isDelete = false;
        if (curChangeObj)
        {
            Destroy(curChangeObj);
            curChangeObj = null;
        }
        if (targetChangObj)
        {
            targetChangObj.GetComponent<MeshRenderer>().enabled = true;
            targetChangObj = null;
        }
        curNewObj = GameObject.Instantiate(mapItemList[curIndex], Vector3.zero, gameObject.transform.rotation);
        curNewObj.layer = 2;

    }
    private void onChangeBuild()
    {
        isDelete = false;
        if (curNewObj)
        {
            Destroy(curNewObj);
            curNewObj = null;
        }
        if (targetChangObj)
        {
            targetChangObj.GetComponent<MeshRenderer>().enabled = true;
            targetChangObj = null;
        }
        curChangeObj = GameObject.Instantiate(mapItemList[curIndex], Vector3.zero, gameObject.transform.rotation);
        curChangeObj.layer = 2;
    }

    private void onSaveMap()
    {
        XMLTest test = new XMLTest();
        widthTxt.text = "0";
        heightTxt.text = "0";
        test.creatMap(int.Parse(widthTxt.text), int.Parse(heightTxt.text), mapObject, mapIndexObject);
    }
    private void onLoadMap()
    {
        foreach (Vector3 list in mapObject.Keys)
        { 
            Destroy(mapObject[list]);
            //mapObject.Remove(list);
            mapIndexObject.Remove(list);
        }
        mapObject = null;
        mapObject = new Dictionary<Vector3, GameObject>();

        XMLTest xml = new XMLTest();
        //gameObject.SetActive(false);
        mapIndexObject = xml.LoadXml();


        foreach (Vector3 list in mapIndexObject.Keys)
        {
            GameObject obj = GameObject.Instantiate(mapItemList[mapIndexObject[list]], list, gameObject.transform.rotation);
            mapObject[obj.transform.position] = obj;
            obj.transform.SetParent(map.transform);
        }
        creatPanel.SetActive(false);

    }

    private void creatMap()
    {
        creatPanel.SetActive(false);




        for (int i = 0; i < int.Parse(widthTxt.text); i++)
        {
            for (int j = 0; j < int.Parse(heightTxt.text); j++)
            {
                GameObject obj = GameObject.Instantiate(mapItemList[curIndex], new Vector3(i, 0, j), gameObject.transform.rotation);
                obj.transform.SetParent(map.transform);
                mapObject[obj.transform.position] = obj;
                mapIndexObject[obj.transform.position] = 0;
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) {
            surface.transform.Translate(new Vector3(0,1,0));
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            surface.transform.Translate(new Vector3(0, -1, 0));
        }


        if (isDelete)
        {
            RaycastHit hitt = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
            {
                float _x = Mathf.Round(hitt.point.x);
                float _y = Mathf.Round(hitt.point.y) - 1;
                float _z = Mathf.Round(hitt.point.z);

                if (targetChangObj)
                {
                    targetChangObj.GetComponent<MeshRenderer>().enabled = true;
                }
                if (mapObject.ContainsKey(new Vector3(_x, _y, _z)))
                {
                    targetChangObj = mapObject[new Vector3(_x, _y, _z)];

                    if (targetChangObj)
                    {
                        targetChangObj.GetComponent<MeshRenderer>().enabled = false;
                    }
                    if (Input.GetMouseButtonDown(0) && targetChangObj)
                    {
                        mapObject.Remove(new Vector3(_x, _y, _z));
                        mapIndexObject.Remove(new Vector3(_x, _y, _z));
                        Destroy(targetChangObj);
                        targetChangObj = null;
                    }
                }
            }
        }
        else if (curNewObj)
        {
            RaycastHit hitt = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
            {
                Debug.DrawLine(ray.origin, hitt.point);
                float _x = Mathf.Round(hitt.point.x);
                float _y = Mathf.Round(hitt.point.y);
                // Debug.Log(_y+"--------------"+ hitt.point.y);
                float _z = Mathf.Round(hitt.point.z);
                curNewObj.transform.position = new Vector3(_x, _y, _z);

                if (Input.GetMouseButtonDown(0) && !mapObject.ContainsKey(curNewObj.transform.position))
                {
                    GameObject obj = Instantiate(curNewObj, curNewObj.transform.position, Quaternion.identity);
                    obj.layer = 0;
                    mapObject[curNewObj.transform.position] = obj;
                    mapIndexObject[curNewObj.transform.position] = curIndex;
                }

            }
            
            if (Input.GetKeyDown(KeyCode.PageUp))
            {
                curIndex--;
                if (curIndex < 0)
                    curIndex = 0;
                Destroy(curNewObj);
                curNewObj = GameObject.Instantiate(mapItemList[curIndex], Vector3.zero, gameObject.transform.rotation);
                curNewObj.layer = 2;
            }
            if (Input.GetKeyDown(KeyCode.PageDown))
            {
                curIndex++;
                if (curIndex > mapItemList.Length - 1)
                    curIndex = mapItemList.Length - 1;
                Destroy(curNewObj);
                curNewObj = GameObject.Instantiate(mapItemList[curIndex], Vector3.zero, gameObject.transform.rotation);
                curNewObj.layer = 2;
            }
        }
        else if (curChangeObj)
        {
            RaycastHit hitt = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
            {
                float _x = Mathf.Round(hitt.point.x);
                float _y = Mathf.Round(hitt.point.y) - 1;
                float _z = Mathf.Round(hitt.point.z);
                curChangeObj.transform.position = new Vector3(_x, _y, _z);
                if (targetChangObj)
                {
                    targetChangObj.GetComponent<MeshRenderer>().enabled = true;
                    targetChangObj = null;
                }


                if (mapObject.ContainsKey(curChangeObj.transform.position))
                {
                    targetChangObj = mapObject[curChangeObj.transform.position];
                    targetChangObj.GetComponent<MeshRenderer>().enabled = false;
                    if (Input.GetMouseButtonDown(0))
                    {
                        Destroy(targetChangObj);
                        GameObject obj = Instantiate(curChangeObj, curChangeObj.transform.position, Quaternion.identity);
                        obj.layer = 0;
                        mapObject[curChangeObj.transform.position] = obj;
                        mapIndexObject[curChangeObj.transform.position] = curIndex;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.PageUp))
            {
                curIndex--;
                if (curIndex < 0)
                    curIndex = 0;
                Destroy(curChangeObj);
                curChangeObj = GameObject.Instantiate(mapItemList[curIndex], Vector3.zero, gameObject.transform.rotation);
                curChangeObj.layer = 2;
            }
            if (Input.GetKeyDown(KeyCode.PageDown))
            {
                curIndex++;
                if (curIndex > mapItemList.Length - 1)
                    curIndex = mapItemList.Length - 1;
                Destroy(curChangeObj);
                curChangeObj = GameObject.Instantiate(mapItemList[curIndex], Vector3.zero, gameObject.transform.rotation);
                curChangeObj.layer = 2;
            }
        }
    }
}
