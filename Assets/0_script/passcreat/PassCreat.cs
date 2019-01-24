using Game.Config;
using Game.Noticfacation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Game;

public class PassCreat : MonoNotice
{
    public GameObject ListContent;


    public Button loadBtn;
    public Button creatBtn;
    public Button creatBox;


    private Dictionary<Vector3, GameObject> mapObject = new Dictionary<Vector3, GameObject>();
    private Dictionary<Vector3, int> mapIndexObject = new Dictionary<Vector3, int>();


    private Dictionary<Vector3, GameObject> enemyObject = new Dictionary<Vector3, GameObject>();
    private Dictionary<Vector3, int> enemyIDObject = new Dictionary<Vector3, int>();
    private Text[] txtlist;

    private GameObject[] mapItemList;
    private string[] boxNames;

    public GameObject map;
    public GameObject enemyparent;
    public GameObject enemyGameObject;

    private Dictionary<Vector3, GameObject> boxObject = new Dictionary<Vector3, GameObject>();
    public GameObject boxparent;
    public GameObject boxGameObject;
    public GameObject curBoxObject;

    private GameObject curEnemy;
    private GameObject newEnemy;
    private int monsterId;

    public Button loadEnemyBtn;
    public Button saveEnemyBtn;
    public GameObject presetPanel;
    public GameObject monsterPanel;
    public Button deleteBtn;
    public Button bornBtn;

    public Button deletePatrol;
    public Button editPatrol;
    public Button placePatrol;
    public GameObject placeGameObject;
    private GameObject newPlaceGameObject;
    private GameObject curPlaceGameObject;
    private Dictionary<Vector3, List<Vector3>> placeObject = new Dictionary<Vector3, List<Vector3>>();
    private List<GameObject> curPatrolList;
    private Dictionary<Vector3, Dictionary<Vector3, PatrolData>> patrolObject = new Dictionary<Vector3, Dictionary<Vector3, PatrolData>>();
    public GameObject bronCube;
    private GameObject curBronCube;
    private Dictionary<Vector3, GameObject> bornObject = new Dictionary<Vector3, GameObject>();
    public InputField passtype;
    public InputField targetVetor;
    private PassInfo passInfo;
    private Vector3 curVetorIndex;
    public GameObject patrolPanel;
    // Use this for initialization

    private List<String> trapList = new List<string>();
    public Button trapBtn;
    public Text wordsTxt;

    public InputField trapText;


    public List<GameObject> trapListPre;


    void Start () {
        trapText.enabled = false;
        patrolPanel.SetActive(false);
        loadBtn.onClick.AddListener(onLoadMap);
        creatBtn.onClick.AddListener(getMonsterInfor);
        creatBox.onClick.AddListener(onCreatBox);
        deleteBtn.onClick.AddListener(onDelete);
        bornBtn.onClick.AddListener(onBorn);
        trapBtn.onClick.AddListener(onTrap);


        saveEnemyBtn.onClick.AddListener(saveEnemy);
        loadEnemyBtn.onClick.AddListener(onLoadEnemy);


        deletePatrol.onClick.AddListener(ondeletePatrol);
        editPatrol.onClick.AddListener(oneditPatrol);
        placePatrol.onClick.AddListener(onplacePatrol);

        getBoxNames();

        patrolPanel.transform.Find("savePatrol").GetComponent<Button>().onClick.AddListener(onSavePatrol);
        presetPanel.SetActive(false);

        addListener(Notice.CREAT_MONSTER,(id) =>
        {
            presetPanel.SetActive(false);
            onCreatEnemy();
            monsterId = id;
        });
        addListener(Notice.CLICK_MONSTER, (v3) =>
        {
            curVetorIndex = v3;

            if (curPatrolList!= null) {
                for (int i= 0; i < curPatrolList.Count; i++) {
                    Destroy(curPatrolList[i]);
                }
            }

            curPatrolList = null;
            curPatrolList = new List<GameObject>();
            if (placeObject.ContainsKey(curVetorIndex))
            {
                for (int i = 0; i < placeObject[curVetorIndex].Count; i++)
                {
                    curPatrolList.Add(GameObject.Instantiate(placeGameObject, placeObject[curVetorIndex][i], gameObject.transform.rotation));
                }
            }
        });

        addListener(Notice.CREAT_Trap, initTrap);

        updataMonsterList();
    }

    private List<GameObject> curTrap;
    private int curTrapId;
    private string curtrapbianma;

 

    private void onTrap()
    {
        presetPanel.SetActive(true);
        int trapIndex = 0;
        foreach (Transform child in presetPanel.transform)
        {
            if (trapIndex < trapNameList.Count)
            {
                child.GetComponent<MonsterPresetItem>().setTrapData(trapNameList[trapIndex]);
            }
            else
            {
                Destroy(child.gameObject);
            }
            trapIndex++;
        }
    }

    public void ondeletePatrol() {
        if (curPatrolList != null)
        {
            for (int i = 0; i < curPatrolList.Count; i++)
            {
                Destroy(curPatrolList[i]);
            }
        }

        curPatrolList = null;
        curPatrolList = new List<GameObject>();

        if (placeObject.ContainsKey(curVetorIndex)) {
            placeObject.Remove(curVetorIndex);
        }

        if (patrolObject.ContainsKey(curVetorIndex))
        {
            patrolObject.Remove(curVetorIndex);
        }
    }

    public void oneditPatrol()
    {
        if (isEditPatrol)
        {
            editPatrol.transform.Find("Text").GetComponent<Text>().text = "编辑寻路";
            isEditPatrol = false;
        }
        else
        {
            editPatrol.transform.Find("Text").GetComponent<Text>().text = "退出寻路";
            isEditPatrol = true;
        }
    }
    private bool isEditPatrol = false;

    public void onplacePatrol()
    {
        newPlaceGameObject = GameObject.Instantiate(placeGameObject, new Vector3(0, 0, 0), gameObject.transform.rotation);


    }

    private void onBorn()
    {
        curBronCube = GameObject.Instantiate(bronCube, new Vector3(0, 0, 0), gameObject.transform.rotation);
        curBronCube.layer = 2;
    }

    private void onDelete()
    {
        if (curVetorIndex != null) {
            if(enemyObject[curVetorIndex] != null) { 
                Destroy(enemyObject[curVetorIndex]);
                enemyIDObject.Remove(curVetorIndex);
                enemyObject.Remove(curVetorIndex);
                updataMonsterList();
             }
        }
    }

    private void updataMonsterList()
    {
        int index = 0;
        Vector3[] vlist = new Vector3[enemyObject.Count];
        foreach (Vector3 list in enemyObject.Keys) {
            vlist[index] = list;
            index++;
        }
        index = 0;
        foreach (Transform child in monsterPanel.transform)
        {
            if (index > enemyObject.Keys.Count)
            {
                child.gameObject.SetActive(false);
            }
            else {
                if (index != 0) {
                    child.GetComponent<MonsterItem>().setData(MonsterPresetData.get(enemyIDObject[vlist[index - 1]]), vlist[index - 1]);
                }
                child.gameObject.SetActive(true);
            }
            index++;
        }
    }
    private void getMonsterInfor() {
        presetPanel.SetActive(true);
        int monsterindex = 1;
        foreach (Transform child in presetPanel.transform)
        {
            if (MonsterPresetData.get(monsterindex) != null)
            {
                child.GetComponent<MonsterPresetItem>().setData(MonsterPresetData.get(monsterindex));
            }
            else {
                Destroy(child.gameObject);
            }
            monsterindex++;
        }
    }

    private void onCreatBox()
    {
        curBoxObject = GameObject.Instantiate(boxGameObject, new Vector3(0, 0, 0), gameObject.transform.rotation);
        curBoxObject.transform.SetParent(boxparent.transform);
        curBoxObject.layer = 2;
    }

    private void saveEnemy()
    {
        XMLTest test = new XMLTest();
        test.creatEnemy(enemyObject, boxObject,enemyIDObject,bornObject,passtype.text,targetVetor.text,patrolObject,trapList);
    }

    private void onLoadEnemy()
    {
        foreach (Vector3 list in enemyObject.Keys)
        {
            Destroy(enemyObject[list]);
        }
        enemyObject = null;
        enemyObject = new Dictionary<Vector3, GameObject>();
        enemyIDObject = null;
        enemyIDObject = new Dictionary<Vector3, int>();


        XMLTest xml = new XMLTest();
        //gameObject.SetActive(false);
        //mapIndexObject = xml.LoadXml();


        foreach (Vector3 list in xml.LoadEnemyXml(patrolObject).Keys)
        {
            GameObject obj = GameObject.Instantiate(enemyGameObject, list, gameObject.transform.rotation);
            enemyObject[obj.transform.position] = obj;
            enemyIDObject[obj.transform.position] = xml.LoadEnemyXml(patrolObject)[list];
            obj.transform.SetParent(enemyparent.transform);
        }

        foreach (Vector3 list in patrolObject.Keys) {
            foreach (Vector3 plist in patrolObject[list].Keys)
            {
                if (placeObject.ContainsKey(list) == false)
                    placeObject[list] = new List<Vector3>();
                placeObject[list].Add(plist);
            }
        }
        
        updataMonsterList();


        foreach (Vector3 list in xml.LoadBoxXml().Keys)
        {
            GameObject obj = GameObject.Instantiate(boxGameObject, list, gameObject.transform.rotation);
            boxObject[obj.transform.position] = obj;
            obj.transform.SetParent(boxparent.transform);
        }

        passInfo = xml.loadPassInfo();
        passtype.text = passInfo.passtye;
        targetVetor.text = passInfo.targetStr;

        for (int i = 0; i < passInfo.bornList.Count; i++) {
            GameObject obj = GameObject.Instantiate(bronCube, passInfo.bornList[i], gameObject.transform.rotation);
            bornObject[obj.transform.position] = obj;
        }

    }

    private void openMoensterPanel()
    {
        monsterPanel.SetActive(true);
    }

    private void onCreatEnemy()
    {
        newEnemy = GameObject.Instantiate(enemyGameObject, new Vector3(0, 0, 0), gameObject.transform.rotation);
        newEnemy.transform.SetParent(enemyparent.transform);
    }


    private void updataList(int type) {
        txtlist = new Text[20];
        foreach (Vector3 list in enemyObject.Keys)
        {
            GameObject t2 = new GameObject();
            t2.AddComponent<Text>();
            t2.GetComponent<Text>().text = "111111";
            t2.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            RectTransform rectTransform2 = t2.GetComponent<RectTransform>();
            rectTransform2.localPosition = new Vector3(0, 0, 0);
            t2.transform.parent = ListContent.transform;
        }
    }

    private void getBoxNames()
    {
        string boxname = "";
        string fullPath = "Assets/Resources/map/Prefabs" + "/";



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
            }
            boxNames = boxname.Split(';');
        }

        mapItemList = new GameObject[boxNames.Length];

        for (int i = 0; i < boxNames.Length; i++)
        {
            mapItemList[i] = (GameObject)Resources.Load("map/Prefabs/" + boxNames[i]);
        }
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
    }

    private void onSavePatrol() {
        PatrolData data = patrolObject[curVetorIndex][curPlaceGameObject.transform.position];
        data.stayTime = int.Parse(patrolPanel.transform.Find("stayTime").GetComponent<InputField>().text);
        data.agentmin = int.Parse(patrolPanel.transform.Find("agentmin").GetComponent<InputField>().text);
        data.agentmax = int.Parse(patrolPanel.transform.Find("agentmax").GetComponent<InputField>().text);
        data.loop = int.Parse(patrolPanel.transform.Find("loop").GetComponent<InputField>().text);
    }

    // Update is called once per frame
    void Update () {

        updataTrap();
        if (isEditPatrol) {
            RaycastHit hitt = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitt, 500f,1<<2))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    curPlaceGameObject = hitt.collider.gameObject;
                    if (curPlaceGameObject.name == "patrolCube(Clone)"){
                        patrolPanel.SetActive(true);
                    }

                    if (!patrolObject[curVetorIndex].ContainsKey(curPlaceGameObject.transform.position) || patrolObject[curVetorIndex][curPlaceGameObject.transform.position] == null)
                    {
                        patrolObject[curVetorIndex][curPlaceGameObject.transform.position] = new PatrolData();
                   
                    }
                    PatrolData data = patrolObject[curVetorIndex][curPlaceGameObject.transform.position];
                    patrolPanel.transform.Find("stayTime").GetComponent<InputField>().text = data.stayTime+"";
                    patrolPanel.transform.Find("agentmin").GetComponent<InputField>().text = data.agentmin + "";
                    patrolPanel.transform.Find("agentmax").GetComponent<InputField>().text = data.agentmax + "";
                    patrolPanel.transform.Find("loop").GetComponent<InputField>().text = data.loop + "";
                }
            }
            return;
        }


        if (newEnemy) {
                RaycastHit hitt = new RaycastHit();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
                {
                    Debug.DrawLine(ray.origin, hitt.point);
                    float _x = Mathf.Round(hitt.point.x);
                    float _y = Mathf.Round(hitt.point.y);
                    // Debug.Log(_y+"--------------"+ hitt.point.y);
                    float _z = Mathf.Round(hitt.point.z);
                    newEnemy.transform.position = new Vector3(_x, _y, _z);

                    if ( !enemyObject.ContainsKey(newEnemy.transform.position)&& Input.GetMouseButtonDown(0))
                    {
                    //GameObject obj = Instantiate(curEnemy, curEnemy.transform.position, Quaternion.identity);
                    newEnemy.layer = 0;
                        enemyObject[newEnemy.transform.position] = newEnemy;
                    enemyIDObject[newEnemy.transform.position] = monsterId;
                    newEnemy = null;
                    updataMonsterList();
                    //updataList(1);
                }

            }
        }
        if (curBronCube) {
            RaycastHit hitt = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
            {
                Debug.DrawLine(ray.origin, hitt.point);
                float _x = Mathf.Round(hitt.point.x);
                float _y = Mathf.Round(hitt.point.y);
                // Debug.Log(_y+"--------------"+ hitt.point.y);
                float _z = Mathf.Round(hitt.point.z);
                curBronCube.transform.position = new Vector3(_x, _y, _z);

                if (!enemyObject.ContainsKey(curBronCube.transform.position) && Input.GetMouseButtonDown(0))
                {
                    //GameObject obj = Instantiate(curEnemy, curEnemy.transform.position, Quaternion.identity);
                    curBronCube.layer = 0;
                    bornObject[curBronCube.transform.position] = curBronCube;
                    curBronCube = null;
                    //updataList(1);
                }

            }
        }

        if (newPlaceGameObject) {
            RaycastHit hitt = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
            {
                float _x = Mathf.Round(hitt.point.x);
                float _y = Mathf.Round(hitt.point.y);
                // Debug.Log(_y+"--------------"+ hitt.point.y);
                float _z = Mathf.Round(hitt.point.z);
                newPlaceGameObject.transform.position = new Vector3(_x, _y, _z);

           
                if (Input.GetMouseButtonDown(0))
                {
                    if (!placeObject.ContainsKey(curVetorIndex))
                    {
                        placeObject[curVetorIndex] = new List<Vector3>();
                        patrolObject[curVetorIndex] = new Dictionary<Vector3, PatrolData>();
                    }
                    patrolObject[curVetorIndex][newPlaceGameObject.transform.position] = null;
                    placeObject[curVetorIndex].Add(newPlaceGameObject.transform.position);
                    //GameObject obj = Instantiate(curEnemy, curEnemy.transform.position, Quaternion.identity);
                    curPatrolList.Add(newPlaceGameObject);
                    newPlaceGameObject = null;
                    //updataList(1);
                }
            }
        }

        if (curBoxObject)
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
                curBoxObject.transform.position = new Vector3(_x, _y, _z);

                if (!boxObject.ContainsKey(curBoxObject.transform.position) && Input.GetMouseButtonDown(0))
                {
                    //GameObject obj = Instantiate(curEnemy, curEnemy.transform.position, Quaternion.identity);
                    curBoxObject.layer = 0;
                    boxObject[curBoxObject.transform.position] = curBoxObject;
                    curBoxObject = null;
                    //updataList(1);
                }
            }
        }

    }

    private void initTrap(string name)
    {
        presetPanel.SetActive(false);
        if (name == "门")
        {
            curTrap = new List<GameObject>();

            wordsTxt.text = "请放置门的位置";
            GameObject obj = GameObject.Instantiate(trapListPre[0], new Vector3(0, 0, 0), trapListPre[0].transform.rotation);
            obj.layer = 2;
            curTrap.Add(obj);

            curTrapId = 1;

            curtrapbianma = "";
        }
        else if (name == "刷怪点")
        {
            curTrap = new List<GameObject>();

            wordsTxt.text = "请放置刷怪点的位置";
            GameObject obj = GameObject.Instantiate(trapListPre[1], new Vector3(0, 0, 0), trapListPre[1].transform.rotation);
            obj.layer = 2;
            curTrap.Add(obj);

            curTrapId = 2;

            trapNumList[0] = 1;
            trapNumList[1] = 30;

            curtrapbianma = "";
        }
        else if (name == "攻击机关")
        {
            curTrap = new List<GameObject>();

            wordsTxt.text = "请放置刷怪点的位置";
            GameObject obj = GameObject.Instantiate(trapListPre[2], new Vector3(0, 0, 0), trapListPre[1].transform.rotation);
            obj.layer = 2;
            curTrap.Add(obj);

            curTrapId = 3;

            trapNumList[0] = 1000;
            trapNumList[1] = 1000;
            trapNumList[2] = 10;
            trapNumList[3] = 1;
            trapNumList[4] = 1;
            trapNumList[5] = 10;

            curtrapbianma = "";
        }
        else if (name == "陷阱")
        {
            curTrap = new List<GameObject>();
            wordsTxt.text = "放置陷阱位置IJOK范围";

            GameObject obj = GameObject.Instantiate(placeGameObject, new Vector3(0, 0, 0), trapListPre[1].transform.rotation);
            obj.layer = 2;
            curTrap.Add(obj);

            curTrapId = 4;
            trapNumList[0] = 1;
            trapNumList[1] = 100;
            curtrapbianma = "";
        }
        else if (name == "传送")
        {
            curTrap = new List<GameObject>();
            wordsTxt.text = "放置传送点位置";

            GameObject obj = GameObject.Instantiate(trapListPre[4], new Vector3(0, 0, 0), trapListPre[4].transform.rotation);
            obj.layer = 2;
            curTrap.Add(obj);

            curTrapId = 5;
            curtrapbianma = "";
        }
        else if (name == "自爆雷")
        {
            curTrap = new List<GameObject>();
            wordsTxt.text = "放置自爆点的位置";

            GameObject obj = GameObject.Instantiate(trapListPre[5], new Vector3(0, 0, 0), trapListPre[5].transform.rotation);
            obj.layer = 2;
            curTrap.Add(obj);

            curTrapId = 6;
            curtrapbianma = "";
        }
        else if (name == "弹簧") {
            curTrap = new List<GameObject>();
            wordsTxt.text = "放置自爆点的位置";

            GameObject obj = GameObject.Instantiate(trapListPre[5], new Vector3(0, 0, 0), trapListPre[5].transform.rotation);
            obj.layer = 2;
            curTrap.Add(obj);

            curTrapId = 7;
            curtrapbianma = "";
        }
    }

    private List<string> trapNameList = new List<string>() { "门", "刷怪点", "攻击机关", "陷阱", "传送", "自爆雷", "弹簧", "风", "Buff", "光感" };


    private void updataTrap()
    {
        if (curTrap != null)
        {
            if (curTrapId == 1)
            {
                doorUpdata();
            }
            else if (curTrapId == 2)
            {
                remonsterupdata();
            }
            else if (curTrapId == 3)
            {
                shootTrapupdata();
            }
            else if (curTrapId == 4)
            {
                cubeTrapupdata();
            }
            else if (curTrapId == 5)
            {
                townportal();
            }
            else if (curTrapId == 6)
            {
                sixTrapupata();
            }
            else if (curTrapId == 7) {
                tanhuang();
            }
        }

    }

    private void tanhuang() {
        if (curTrap.Count == 1)
        {
            RaycastHit hitt = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
            {
                float _x = Mathf.Round(hitt.point.x);
                float _y = Mathf.Round(hitt.point.y);
                float _z = Mathf.Round(hitt.point.z);
                curTrap[0].transform.position = new Vector3(_x, _y, _z);
                if (Input.GetMouseButtonDown(0))
                {
                    wordsTxt.text = "放置触发范围 IJ控制";

                    curtrapbianma += curTrap[0].transform.position.x + "," + curTrap[0].transform.position.y + "," + curTrap[0].transform.position.z;
                    trapText.enabled = true;

                    GameObject obj = GameObject.Instantiate(placeGameObject, curTrap[0].transform.position, placeGameObject.transform.rotation);
                    obj.transform.SetParent(curTrap[0].transform);
                    obj.layer = 2;
                    curTrap.Add(obj);
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                curTrap[0].transform.Rotate(new Vector3(0, 90, 0));
            }
        }
        else if (curTrap.Count > 1)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                curTrap[0].transform.Rotate(new Vector3(0, 90, 0));
            }
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                curtrapbianma += "," + curTrap[0].transform.localEulerAngles.y;
                curtrapbianma += "=";
                Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                //curtrapbianma += curTrap[curTrap.Count - 1].transform.position.x + "," + curTrap[curTrap.Count - 1].transform.position.y + "," + curTrap[curTrap.Count - 1].transform.position.z;
                //curtrapbianma += "," + v3.x + "," + v3.y + "," + v3.z;
                curtrapbianma += v3.x;
                trapList.Add(curTrapId + "=" + curtrapbianma);
                wordsTxt.text = "设置完成";
                for (int i = 1; i < curTrap.Count; i++)
                {
                    Destroy(curTrap[i]);
                }
                curTrap = null;
                return; ;
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x + 1, v3.y, v3.z);
                curTrap[curTrap.Count - 1].transform.Translate(new Vector3(0.5f,0,0));
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                if (v3.x == 1)
                    return;
                curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x - 1, v3.y, v3.z);
                curTrap[curTrap.Count - 1].transform.Translate(new Vector3(-0.5f, 0, 0));
            }
        }

    }


    private List<int> trapNumList = new List<int> { 0,0,0,0,0,0,0,0};

    private void sixTrapupata() {
        if (curtrapbianma.Split('=').Length == 1)
        {
            RaycastHit hitt = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
            {
                float _x = Mathf.Round(hitt.point.x);
                float _y = Mathf.Round(hitt.point.y);
                float _z = Mathf.Round(hitt.point.z);
                curTrap[0].transform.position = new Vector3(_x, _y, _z);
                if (Input.GetMouseButtonDown(0))
                {
                    wordsTxt.text = "放置触发范围 IJOK控制";
                    curtrapbianma += curTrap[0].transform.position.x + "," + curTrap[0].transform.position.y + "," + curTrap[0].transform.position.z;
                    curtrapbianma += "=";
                    trapText.enabled = true;

                    GameObject obj = GameObject.Instantiate(placeGameObject, new Vector3(0, 0, 0), placeGameObject.transform.rotation);
                    obj.layer = 2;
                    curTrap.Add(obj);
                }
            }
        }
        else if (curtrapbianma.Split('=').Length == 2)
        {
            RaycastHit hitt = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
            {
                float _x = Mathf.Round(hitt.point.x);
                float _y = Mathf.Round(hitt.point.y);
                float _z = Mathf.Round(hitt.point.z);
                curTrap[curTrap.Count - 1].transform.position = new Vector3(_x, _y, _z);

                if (Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    wordsTxt.text = "放置爆炸范围 IJOK控制";

                   Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                   curtrapbianma += curTrap[curTrap.Count - 1].transform.position.x + "," + curTrap[curTrap.Count - 1].transform.position.y + "," + curTrap[curTrap.Count - 1].transform.position.z;
                   curtrapbianma += "," + v3.x + "," + v3.y + "," + v3.z;
                   curtrapbianma += "=";
                    GameObject obj = GameObject.Instantiate(placeGameObject, new Vector3(0, 0, 0), placeGameObject.transform.rotation);
                    obj.layer = 2;
                    curTrap.Add(obj);
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x + 1, v3.y, v3.z);
                }
                if (Input.GetKeyDown(KeyCode.J))
                {
                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    if (v3.x == 1)
                        return;
                    curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x - 1, v3.y, v3.z);
                }

                if (Input.GetKeyDown(KeyCode.O))
                {
                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x, v3.y, v3.z + 1);
                }
                if (Input.GetKeyDown(KeyCode.K))
                {
                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    if (v3.z == 1)
                        return;
                    curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x, v3.y, v3.z - 1);
                }
            }
        }
        else if (curtrapbianma.Split('=').Length == 3)
        {
            RaycastHit hitt = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
            {
                float _x = Mathf.Round(hitt.point.x);
                float _y = Mathf.Round(hitt.point.y);
                float _z = Mathf.Round(hitt.point.z);
                curTrap[curTrap.Count - 1].transform.position = new Vector3(_x, _y, _z);

                if (Input.GetKeyDown(KeyCode.KeypadEnter))
                {

                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    curtrapbianma += curTrap[curTrap.Count - 1].transform.position.x + "," + curTrap[curTrap.Count - 1].transform.position.y + "," + curTrap[curTrap.Count - 1].transform.position.z;
                    curtrapbianma += "," + v3.x + "," + v3.y + "," + v3.z;
                    trapList.Add(curTrapId + "=" + curtrapbianma);
                    wordsTxt.text = "设置完成";
                    for (int i = 1; i < curTrap.Count; i++)
                    {
                        Destroy(curTrap[i]);
                    }
                    curTrap = null;
                    return;
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x + 1, v3.y, v3.z);
                }
                if (Input.GetKeyDown(KeyCode.J))
                {
                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    if (v3.x == 1)
                        return;
                    curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x - 1, v3.y, v3.z);
                }

                if (Input.GetKeyDown(KeyCode.O))
                {
                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x, v3.y, v3.z + 1);
                }
                if (Input.GetKeyDown(KeyCode.K))
                {
                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    if (v3.z == 1)
                        return;
                    curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x, v3.y, v3.z - 1);
                }
            }
        }

    }


    private void townportal()
    {
        if (curtrapbianma.Split('=').Length == 1)
        {
            RaycastHit hitt = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
            {
                float _x = Mathf.Round(hitt.point.x);
                float _y = Mathf.Round(hitt.point.y);
                float _z = Mathf.Round(hitt.point.z);
                curTrap[0].transform.position = new Vector3(_x, _y, _z);
                if (Input.GetMouseButtonDown(0))
                {
                    wordsTxt.text = "放置目标点";
                    curtrapbianma += curTrap[0].transform.position.x + "," + (curTrap[0].transform.position.y - 1) + "," + curTrap[0].transform.position.z;
                    curTrap[0].layer = 0;
                    curtrapbianma += "=";
                    trapText.enabled = true;

                    GameObject obj = GameObject.Instantiate(trapListPre[4], new Vector3(0, 0, 0), trapListPre[4].transform.rotation);
                    obj.layer = 2;
                    curTrap.Add(obj);
                }
            }
        }
        else if (curtrapbianma.Split('=').Length == 2)
        {
            RaycastHit hitt = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
            {
                float _x = Mathf.Round(hitt.point.x);
                float _y = Mathf.Round(hitt.point.y);
                float _z = Mathf.Round(hitt.point.z);
                curTrap[1].transform.position = new Vector3(_x, _y, _z);

                if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
                    curtrapbianma += curTrap[1].transform.position.x + "," + curTrap[1].transform.position.y + "," + curTrap[1].transform.position.z;
                    wordsTxt.text = "设置完毕";
                    trapList.Add(curTrapId + "=" + curtrapbianma);
                    curTrap = null;
                    curTrapId = 0;
                }
            }
        }
    }
    private void cubeTrapupdata()
    {
        if (curtrapbianma.Split('=').Length == 1)
        {
            RaycastHit hitt = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
            {
                float _x = Mathf.Round(hitt.point.x);
                float _y = Mathf.Round(hitt.point.y);
                float _z = Mathf.Round(hitt.point.z);
                curTrap[0].transform.position = new Vector3(_x, _y, _z);
                if (Input.GetMouseButtonDown(0))
                {
                    wordsTxt.text = "类型1.掉落2.buffID，延迟";
                    curtrapbianma += curTrap[0].transform.position.x + "," + (curTrap[0].transform.position.y) + "," + curTrap[0].transform.position.z + "," + curTrap[0].transform.lossyScale.x + "," + curTrap[0].transform.lossyScale.y + "," + curTrap[0].transform.lossyScale.z;
                    curTrap[0].layer = 0;
                    curtrapbianma += "=";
                    trapText.enabled = true;
                    trapText.text = ""+trapNumList[0]+ "," + trapNumList[1];
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x + 1, v3.y, v3.z);
                }
                if (Input.GetKeyDown(KeyCode.J))
                {
                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    if (v3.x == 1)
                        return;
                    curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x - 1, v3.y, v3.z);
                }

                if (Input.GetKeyDown(KeyCode.O))
                {
                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x, v3.y, v3.z + 1);
                }
                if (Input.GetKeyDown(KeyCode.K))
                {
                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    if (v3.z == 1)
                        return;
                    curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x, v3.y, v3.z - 1);
                }


            }
        }
        if (curtrapbianma.Split('=').Length == 2)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                curtrapbianma += trapText.text;
                trapList.Add(curTrapId + "=" + curtrapbianma);
                trapText.enabled = false;
                trapText.text = "";
                wordsTxt.text = "设置完毕";
                curTrap = null;
                curTrapId = 0;
            }
        }
    }
    private void shootTrapupdata()
    {
        if (curtrapbianma.Split('=').Length == 1)
        {
            RaycastHit hitt = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
            {
                float _x = Mathf.Round(hitt.point.x);
                float _y = Mathf.Round(hitt.point.y);
                float _z = Mathf.Round(hitt.point.z);
                curTrap[0].transform.position = new Vector3(_x, _y, _z);
                if (Input.GetMouseButtonDown(0))
                {
                    wordsTxt.text = "CD,速度,距离,类型,buffid,持续时间:";
                    trapText.text = trapNumList[0] + "," + trapNumList[1] + "," + trapNumList[2] + "," + trapNumList[3] + "," + trapNumList[4] + "," + trapNumList[5]; 
                    trapText.enabled = true;
                    curtrapbianma += curTrap[0].transform.position.x + "," + curTrap[0].transform.position.y + "," + curTrap[0].transform.position.z;
                    curTrap[0].layer = 0;
                    curtrapbianma += "=";
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                curTrap[0].transform.Rotate(new Vector3(0, 90, 0));
            }
        }
        else if (curtrapbianma.Split('=').Length == 2)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                for (var i = 0; i < trapText.text.Split(',').Length; i++)
                {
                    trapNumList[i] = int.Parse(trapText.text.Split(',')[i]);
                }
                curtrapbianma += trapNumList[0] + "," + trapNumList[1] + "," + trapNumList[2] + "," + trapNumList[3] + "," + trapNumList[4] + "," + trapNumList[5];

                trapList.Add(curTrapId+"="+curtrapbianma);

                trapText.enabled = false;


                wordsTxt.text = "设置完毕";
                curTrap = null;
                curTrapId = 0;
            }

            /*if (Input.GetKeyDown(KeyCode.I))
            {
                trapNumList[0] = trapNumList[0] + 1;
                wordsTxt.text = "CD:" + trapNumList[0] + "飞行速度:" + trapNumList[1] + "距离:" + trapNumList[2] + "类型:" + trapNumList[3] + "      “IJ” CD “OK” 飞行速度 “PL” 距离 “[;” 类型";
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                trapNumList[0] = trapNumList[0] - 1;
                if (trapNumList[0] < 1)
                    trapNumList[0] = 1;
                wordsTxt.text = "CD:" + trapNumList[0] + "飞行速度:" + trapNumList[1] + "距离:" + trapNumList[2] + "类型:" + trapNumList[3] + "      “IJ” CD “OK” 飞行速度 “PL” 距离 “[;” 类型";
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                trapNumList[1] = trapNumList[1] + 1;
                wordsTxt.text = "CD:" + trapNumList[0] + "飞行速度:" + trapNumList[1] + "距离:" + trapNumList[2] + "类型:" + trapNumList[3] + "      “IJ” CD “OK” 飞行速度 “PL” 距离 “[;” 类型";
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                trapNumList[1] = trapNumList[1] - 1;
                if (trapNumList[1] < 1)
                    trapNumList[1] = 1;
                wordsTxt.text = "CD:" + trapNumList[0] + "飞行速度:" + trapNumList[1] + "距离:" + trapNumList[2] + "类型:" + trapNumList[3] + "      “IJ” CD “OK” 飞行速度 “PL” 距离 “[;” 类型";
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                trapNumList[2] = trapNumList[2] + 1;
                wordsTxt.text = "CD:" + trapNumList[0] + "飞行速度:" + trapNumList[1] + "距离:" + trapNumList[2] + "类型:" + trapNumList[3] + "      “IJ” CD “OK” 飞行速度 “PL” 距离 “[;” 类型";
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                trapNumList[2] = trapNumList[2] - 1;
                if (trapNumList[2] < 1)
                    trapNumList[2] = 1;
                wordsTxt.text = "CD:" + trapNumList[0] + "飞行速度:" + trapNumList[1] + "距离:" + trapNumList[2] + "类型:" + trapNumList[3] + "      “IJ” CD “OK” 飞行速度 “PL” 距离 “[;” 类型";
            }

            if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                trapNumList[3] = 1;
                wordsTxt.text = "CD:" + trapNumList[0] + "飞行速度:" + trapNumList[1] + "距离:" + trapNumList[2] + "类型:" + trapNumList[3] + "      “IJ” CD “OK” 飞行速度 “PL” 距离 “[;” 类型";
            }
            if (Input.GetKeyDown(KeyCode.Colon))
            {
                trapNumList[3] = 2;
                if (trapNumList[1] < 1)
                    trapNumList[1] = 1;
                wordsTxt.text = "CD:" + trapNumList[0] + "飞行速度:" + trapNumList[1] + "距离:" + trapNumList[2] + "类型:" + trapNumList[3] + "      “IJ” CD “OK” 飞行速度 “PL” 距离 “[;” 类型";
            }*/
        }
    }
    private void remonsterupdata() {
        if (curtrapbianma.Split('=').Length == 1)
        {
            RaycastHit hitt = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
            {
                float _x = Mathf.Round(hitt.point.x);
                float _y = Mathf.Round(hitt.point.y);
                float _z = Mathf.Round(hitt.point.z);
                curTrap[0].transform.position = new Vector3(_x, _y, _z);
                if (Input.GetMouseButtonDown(0))
                {
                    wordsTxt.text = "最多存在:" + trapNumList[0] + "刷新CD:" + trapNumList[1] + "      “IJ” 个数 “OK” CD";
                    curtrapbianma += curTrap[0].transform.position.x + "," + curTrap[0].transform.position.y + "," + curTrap[0].transform.position.z;
                    curTrap[0].layer = 0;
                    curtrapbianma += "=";
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                curTrap[0].transform.Rotate(new Vector3(0, 90, 0));
            }
        }
        else if (curtrapbianma.Split('=').Length == 2) {

            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                curtrapbianma += trapNumList[0]+","+ trapNumList[1];
                Debug.Log(curtrapbianma);
                trapList.Add(curTrapId + "=" + curtrapbianma);
                wordsTxt.text = "设置完毕";
                curTrap = null;
                curTrapId = 0;
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                trapNumList[0] = trapNumList[0] +  1;
                wordsTxt.text = "最多存在:" + trapNumList[0] + "刷新CD" + trapNumList[1] + "/n";
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                trapNumList[0] = trapNumList[0] - 1;
                if (trapNumList[0] < 1)
                    trapNumList[0] = 1;
                wordsTxt.text = "最多存在:" + trapNumList[0] + "刷新CD" + trapNumList[1] + "/n";
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                trapNumList[1] = trapNumList[1] + 1;
                wordsTxt.text = "最多存在:" + trapNumList[0] + "刷新CD" + trapNumList[1] + "/n";
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                trapNumList[1] = trapNumList[1]- 1;
                if (trapNumList[1] < 1)
                    trapNumList[1] = 1;
                wordsTxt.text = "最多存在:" + trapNumList[0] + "刷新CD" + trapNumList[1] + "/n";
            }


        }
    }
    private void doorUpdata()
    {
        if (curtrapbianma.Split('=').Length == 1)
        {
            RaycastHit hitt = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
            {
                float _x = Mathf.Round(hitt.point.x);
                float _y = Mathf.Round(hitt.point.y);
                float _z = Mathf.Round(hitt.point.z);
                curTrap[0].transform.position = new Vector3(_x, _y, _z);
                if (Input.GetMouseButtonDown(0))
                {
                    wordsTxt.text = "放置触发范围的位置 空格新建范围 小键盘回车完成";
                    curtrapbianma += curTrap[0].transform.position.x + ","+curTrap[0].transform.position.y + "," + curTrap[0].transform.position.z + "," + curTrap[0].transform.rotation.y;
                    curtrapbianma += "=";
                    curTrap[0].layer = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                curTrap[0].transform.Rotate(new Vector3(0, 90, 0));
            }
        }
        else if (curtrapbianma.Split('=').Length == 2)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject obj = GameObject.Instantiate(placeGameObject, new Vector3(0, 0, 0), placeGameObject.transform.rotation);
                obj.layer = 2;
                curTrap.Add(obj);
            }

            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                string str = "";
                for (int i = 1; i < curTrap.Count; i++)
                {
                    if (str.Length > 1)
                        str += ";";
                    str += curTrap[i].transform.position.x + "," + curTrap[i].transform.position.y + "," + curTrap[i].transform.position.z + ","
                             + curTrap[i].transform.lossyScale.x + "," + curTrap[i].transform.lossyScale.y + "," + curTrap[i].transform.lossyScale.z;
                }
                

                curtrapbianma += str;

                wordsTxt.text = "放置清除条件 范围无怪  空格新建范围 小键盘回车完成";
                curtrapbianma += "=";
                return;
            }



            if (curTrap.Count > 1)
            {
                RaycastHit hitt = new RaycastHit();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
                {
                    wordsTxt.text = "调整当前范围的位置大小IJ横向 ok纵向 空格新建范围 小键盘回车完成";
                    float _x = Mathf.Round(hitt.point.x);
                    float _y = Mathf.Round(hitt.point.y);
                    float _z = Mathf.Round(hitt.point.z);
                    curTrap[curTrap.Count - 1].transform.position = new Vector3(_x, _y, _z);
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x + 1, v3.y, v3.z);
                }
                if (Input.GetKeyDown(KeyCode.J))
                {
                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    if (v3.x == 1)
                        return;
                    curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x - 1, v3.y, v3.z);
                }

                if (Input.GetKeyDown(KeyCode.O))
                {
                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x, v3.y, v3.z + 1);
                }
                if (Input.GetKeyDown(KeyCode.K))
                {
                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    if (v3.z == 1)
                        return;
                    curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x, v3.y, v3.z - 1);
                }
            }
        }
        else if (curtrapbianma.Split('=').Length == 3)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject obj = GameObject.Instantiate(placeGameObject, new Vector3(0, 0, 0), placeGameObject.transform.rotation);
                obj.layer = 2;
                curTrap.Add(obj);
            }

            int num = curtrapbianma.Split('=')[1].Split(';').Length;

            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                string str = "";
                for (int i = 1+ num; i < curTrap.Count; i++)
                {
                    if (str.Length > 1)
                        str += ";";
                    str += curTrap[i].transform.position.x + "," + curTrap[i].transform.position.y + "," + curTrap[i].transform.position.z + ","
                             + curTrap[i].transform.lossyScale.x + "," + curTrap[i].transform.lossyScale.y + "," + curTrap[i].transform.lossyScale.z;
                }

                curtrapbianma += curTrapId + str;

                trapList.Add(curTrapId + "=" + curtrapbianma);

                wordsTxt.text = "设置完成";
                for (int i = 1; i < curTrap.Count; i++)
                {
                    Destroy(curTrap[i]);
                }
                curTrap = null;
                return;
            }


            if (curTrap.Count > 1 + num)
            {
                RaycastHit hitt = new RaycastHit();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
                {
                    wordsTxt.text = "调整当前范围的位置大小IJ横向 ok纵向 空格新建范围 小键盘回车完成";
                    float _x = Mathf.Round(hitt.point.x);
                    float _y = Mathf.Round(hitt.point.y);
                    float _z = Mathf.Round(hitt.point.z);
                    curTrap[curTrap.Count - 1].transform.position = new Vector3(_x, _y, _z);
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x + 1, v3.y, v3.z);
                }
                if (Input.GetKeyDown(KeyCode.J))
                {
                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    if (v3.x == 1)
                        return;
                    curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x - 1, v3.y, v3.z);
                }

                if (Input.GetKeyDown(KeyCode.O))
                {
                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x, v3.y, v3.z + 1);
                }
                if (Input.GetKeyDown(KeyCode.K))
                {
                    Vector3 v3 = curTrap[curTrap.Count - 1].transform.localScale;
                    if (v3.z == 1)
                        return;
                    curTrap[curTrap.Count - 1].transform.localScale = new Vector3(v3.x, v3.y, v3.z - 1);
                }
            }
        }
    }
}
