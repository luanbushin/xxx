using Game.Global;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MapCreat : MonoBehaviour
{
    public GameObject mainCanvas;

    public Button saveBtn;
    public Button loadBtn;


    public Button savegroupBtn;
    public Button newgroupBtn;
    public Button grouplistBtn;
    public Button cancelBtn;
    public Button newBtn;
    public Button changeBtn;
    public Button deleteBtn;
    public InputField findTxt;

    public Camera mainCamera;
    public Camera camera1;
    public Camera camera2;



    public GameObject chooseBuild;
    public GameObject creatPanel;
    public Text widthTxt;
    public Text heightTxt;
    public Button creatBtn;

    public GameObject map;
    public GameObject muenObj;


    private GameObject[] mapItemList;
    public Dropdown boxList;

    private Dictionary<Vector3, GameObject> curGroup;
    private Dictionary<Vector3, int> curIndexGroup;
    private GameObject curNewObj;
    private GameObject curChangeObj;
    private GameObject targetChangObj;

    public GameObject surface;

    private bool isDelete = false;

    private Dictionary<Vector3, GameObject> mapObject = new Dictionary<Vector3, GameObject>();
    private Dictionary<Vector3, int> mapIndexObject = new Dictionary<Vector3, int>();

    private Dictionary<Vector3, GameObject> groupMapObject;
    private Dictionary<Vector3, int> groupMapIndexObject;

    private Dictionary<Vector3, GameObject> mapProObject = new Dictionary<Vector3, GameObject>();

    private List<List<CreatMapItem>> mapGroupList;

    public int curIndex = 0;

    private string[] boxNames;


    private string creatMode = "map";


    private string fromtype;


    // Use this for initialization
    void Start()
    {
        Debug.Log("===================");
        Debug.Log(Application.streamingAssetsPath);

        camera1.gameObject.SetActive(false);
        camera2.gameObject.SetActive(false);


        creatBtn.onClick.AddListener(creatMap);

        saveBtn.onClick.AddListener(onSaveMap);
        loadBtn.onClick.AddListener(onLoadMap);

        savegroupBtn.onClick.AddListener(onSaveGroup);
        newgroupBtn.onClick.AddListener(onNewGroup);
        grouplistBtn.onClick.AddListener(onGroupList);
        cancelBtn.onClick.AddListener(onCanelBuild);
        newBtn.onClick.AddListener(onNewBuild);
        changeBtn.onClick.AddListener(onChangeBuild);
        deleteBtn.onClick.AddListener(onDleleteBuild);
        //findTxt.transform.GetComponent<InputField>().onValueChange.AddListener(findObject);
        getBoxNames();
        loadmapGroupList();
    }

    private void onSaveGroup() {
        XMLTest test = new XMLTest();
        List<CreatMapItem> itemList = new List<CreatMapItem>();
        foreach (Vector3 list in groupMapIndexObject.Keys)
        { 
            CreatMapItem item = new CreatMapItem();
            item.v3 = new Vector3(list.x - 500, list.y+2000,list.z);
            item.typeindex = groupMapIndexObject[list];
            itemList.Add(item);
        }
        mapGroupList.Add(itemList);
        test.creatGroup(mapGroupList);
    }

    private void loadmapGroupList() {
        XMLTest xml = new XMLTest();
        //gameObject.SetActive(false);
        mapGroupList = xml.LoadGroup();
        initGroupList();
    }

    private void onNewGroup() {
        creatMode = "group";

        camera1.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        camera2.gameObject.SetActive(false);

        //mainCanvas.SetActive(false);

        Destroy(curNewObj);
        curNewObj = null;

        groupMapObject = new Dictionary<Vector3, GameObject>();
        groupMapIndexObject = new Dictionary<Vector3, int>();
    }

    private void saveGroup(){
        List<CreatMapItem> maplist = new List<CreatMapItem>();

        foreach (Vector3 list in groupMapObject.Keys) {
            CreatMapItem item = new CreatMapItem();
            item.v3 = list;
            item.typeindex = groupMapIndexObject[list];
            maplist.Add(item);
        }
        mapGroupList.Add(maplist);
    }

    private void copyGroup(int startx,int starty)
    {
        foreach (Vector3 list in groupMapObject.Keys)
        {
            GameObject obj = Instantiate(groupMapObject[list], list, Quaternion.identity);
            obj.layer = 0;
            mapObject[list] = obj;
            mapIndexObject[list] = curIndex;
            obj.transform.SetParent(map.transform);
        }
    }

    private void onGroupList()
    {
        /**camera1.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        creatMode = "group";

        groupMapObject = new Dictionary<Vector3, GameObject>();
        groupMapIndexObject = new Dictionary<Vector3, int>();
        creatPanel.SetActive(true);**/

        fromtype = creatMode;

        creatMode = "grouplist";

        camera1.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(false);
        camera2.gameObject.SetActive(true);

        //mainCanvas.SetActive(false);

        Destroy(curNewObj);
        curNewObj = null;
    }

    public void findObject(string str)
    {
        Debug.Log(str);
    }

    private void getBoxNames()
    {
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

        for (int i = 0; i < boxNames.Length; i++)
        {
            mapItemList[i] = (GameObject)Resources.Load("map/Prefabs/" + boxNames[i]);
        }

        boxList.onValueChanged.AddListener(onValueChanged);
        boxList.value = 1;
        boxList.value = 0;

        initBoxList();
    }

    private void initBoxList(){
        for (int i = 0; i < 558; i++) {
            double num =i / 20;
            GameObject obj = GameObject.Instantiate(mapItemList[i], new Vector3((i % 20)*3+5, -1000, (float)Math.Floor(num)*3), gameObject.transform.rotation);
            obj.transform.SetParent(muenObj.transform);
            mapProObject[obj.transform.position] = obj;
        }
       
    }

    private void initGroupList() {
        for (int i = 0; i < mapGroupList.Count; i++) {
            double num = i / 10;
            List<CreatMapItem> list = mapGroupList[i];
            for (int j = 0; j < list.Count; j++) {
                GameObject obj = GameObject.Instantiate(mapItemList[list[j].typeindex], new Vector3((i % 10) * 20 + list[j].v3.x, -1000, -(float)Math.Floor(num) * 20+ list[j].v3.z-40), gameObject.transform.rotation);
                obj.transform.SetParent(muenObj.transform);
                mapProObject[obj.transform.position] = obj;
            }
        }
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

    private void creatBuildList()
    {
        for (int i = 0; i < mapItemList.Length; i++)
        {
            GameObject obj = GameObject.Instantiate(mapItemList[i], Vector3.zero, gameObject.transform.rotation);
            obj.transform.position = new Vector3(i * 2, -25, 0);
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
                int pox;
                int poy;
                if (creatMode == "map")
                {
                    pox = i;
                    poy = 0;
                }
                else
                {
                    pox = i + 500;
                    poy = -2000;
                }

                GameObject obj = GameObject.Instantiate(mapItemList[curIndex], new Vector3(pox, poy, j), gameObject.transform.rotation);
                obj.transform.SetParent(map.transform);

                if (creatMode == "map")
                {
                    mapObject[obj.transform.position] = obj;
                    mapIndexObject[obj.transform.position] = 0;
                }
                else
                {
                    groupMapObject[obj.transform.position] = obj;
                    groupMapIndexObject[obj.transform.position] = 0;
                }
            }
        }

    }

    private CreatModeConf changeCreatMode() {
        CreatModeConf modeconf = new CreatModeConf();
        if (creatMode == "map")
        {
            modeconf.curCamera = mainCamera;
            modeconf.curmapObject = mapObject;
            modeconf.curmapIndexObject = mapIndexObject;
        }
        else if (creatMode == "grouplist")
        {
            modeconf.curCamera = camera2;
            modeconf.curmapObject = groupMapObject;
            modeconf.curmapIndexObject = groupMapIndexObject;
        }
        else
        {
            modeconf.curCamera = camera1;
            modeconf.curmapObject = groupMapObject;
            modeconf.curmapIndexObject = groupMapIndexObject;
        }
        return modeconf;
    }

    private void chooseGroupOrBox(CreatModeConf modeconf) {
        RaycastHit hitt = new RaycastHit();
        Ray ray = modeconf.curCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
        {
            float _x = Mathf.Round(hitt.point.x);
            float _y = Mathf.Round(hitt.point.y) - 1;
            float _z = Mathf.Round(hitt.point.z);

            if (mapProObject.ContainsKey(new Vector3(_x, _y, _z)))
            {
                targetChangObj = mapProObject[new Vector3(_x, _y, _z)];

                if (Input.GetMouseButtonDown(0) && targetChangObj)
                {
                    curNewObj = null;
                    if (_z < 40)
                    {

                    }
                    else
                    {
                        curNewObj = GameObject.Instantiate(targetChangObj, Vector3.zero, gameObject.transform.rotation);
                        curNewObj.layer = 2;
                        if (fromtype == "map")
                        {
                            camera1.gameObject.SetActive(false);
                            mainCamera.gameObject.SetActive(true);
                            camera2.gameObject.SetActive(false);

                            mainCanvas.SetActive(true);
                            creatMode = "map";
                        }
                        else
                        {
                            camera1.gameObject.SetActive(true);
                            mainCamera.gameObject.SetActive(false);
                            camera2.gameObject.SetActive(false);

                            mainCanvas.SetActive(true);
                            creatMode = "group";
                        }
                    }
                }
            }
        }
    }
    private void creatBox(Camera curCamera, Dictionary<Vector3, GameObject> curmapObject, Dictionary<Vector3, int> curmapIndexObject) {
        if (isDelete)
        {
            RaycastHit hitt = new RaycastHit();
            Ray ray = curCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
            {
                float _x = Mathf.Round(hitt.point.x);
                float _y = Mathf.Round(hitt.point.y) - 1;
                float _z = Mathf.Round(hitt.point.z);

                if (targetChangObj)
                {
                    targetChangObj.GetComponent<MeshRenderer>().enabled = true;
                }
                if (curmapObject.ContainsKey(new Vector3(_x, _y, _z)))
                {
                    targetChangObj = curmapObject[new Vector3(_x, _y, _z)];

                    if (targetChangObj)
                    {
                        targetChangObj.GetComponent<MeshRenderer>().enabled = false;
                    }
                    if (Input.GetMouseButtonDown(0) && targetChangObj)
                    {
                        curmapObject.Remove(new Vector3(_x, _y, _z));
                        curmapIndexObject.Remove(new Vector3(_x, _y, _z));
                        Destroy(targetChangObj);
                        targetChangObj = null;
                    }
                }
            }
        }
        else if (curNewObj)
        {
            RaycastHit hitt = new RaycastHit();
            Ray ray = curCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitt, 500f, 1 << 0))
            {
                Debug.DrawLine(ray.origin, hitt.point);
                float _x = Mathf.Round(hitt.point.x);
                float _y = Mathf.Round(hitt.point.y);
                // Debug.Log(_y+"--------------"+ hitt.point.y);
                float _z = Mathf.Round(hitt.point.z);
                curNewObj.transform.position = new Vector3(_x, _y, _z);

                if (Input.GetMouseButtonDown(0) && !curmapObject.ContainsKey(curNewObj.transform.position))
                {
                    GameObject obj = Instantiate(curNewObj, curNewObj.transform.position, Quaternion.identity);
                    obj.layer = 0;
                    curmapObject[curNewObj.transform.position] = obj;
                    curmapIndexObject[curNewObj.transform.position] = curIndex;
                    obj.transform.SetParent(map.transform);
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
            Ray ray = curCamera.ScreenPointToRay(Input.mousePosition);
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


                if (curmapObject.ContainsKey(curChangeObj.transform.position))
                {
                    targetChangObj = curmapObject[curChangeObj.transform.position];
                    targetChangObj.GetComponent<MeshRenderer>().enabled = false;
                    if (Input.GetMouseButtonDown(0))
                    {
                        Destroy(targetChangObj);
                        GameObject obj = Instantiate(curChangeObj, curChangeObj.transform.position, Quaternion.identity);
                        obj.layer = 0;
                        obj.transform.SetParent(map.transform);
                        curmapObject[curChangeObj.transform.position] = obj;
                        curmapIndexObject[curChangeObj.transform.position] = curIndex;
                    }
                }
            }
        }
    }


    void Update()
    {
        //Camera curCamera;
        //Dictionary<Vector3, GameObject> curmapObject;
        //Dictionary<Vector3, int> curmapIndexObject;

        CreatModeConf creatModeConf = changeCreatMode();
        if (creatMode == "grouplist")
        {
            chooseGroupOrBox(creatModeConf);
        }
        else {
            creatBox(creatModeConf.curCamera, creatModeConf.curmapObject, creatModeConf.curmapIndexObject);
        }
        
        keyContorl();
    }

    private void keyContorl() {
        if (Input.GetKeyDown(KeyCode.I))
        {
            surface.transform.Translate(new Vector3(0, 1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            surface.transform.Translate(new Vector3(0, -1, 0));
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
