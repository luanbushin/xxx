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

    public Grid mygrid;
   
    
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
            mygrid = new Grid();

            foreach (Vector3 list in mapIndexObj.Keys)
            {
                GameObject obj = GameObject.Instantiate(mapItemList[mapIndexObj[list]], list, gameObject.transform.rotation);


                if (list.y == 1&& list.x<150&& list.z < 150) {
                    mygrid.grid[(int)list.x, (int)list.z].mapindex = mapIndexObj[list];
                }
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
            return tov3;
        }
        else {

            FindingPath(fromv3, tov3);

            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.GetComponent<BoxCollider>().isTrigger = true;

            for (int i = mygrid.path.Count - 1; i >=0 ; i--) {
                obj.transform.position = mygrid.path[i]._worldPos;
                if (mygrid.path[i]._worldPos.x == Mathf.Round(fromv3.x) && mygrid.path[i]._worldPos.z == Mathf.Round(fromv3.z))
                    continue;
                
                if (isLineArrive(fromv3, mygrid.path[i]._worldPos))
                {
                    Destroy(obj);
                    return mygrid.path[i]._worldPos;
                }
            }
            Destroy(obj);

            /*return Vector3.zero;
            List<List<Vector3>> allway = new List<List<Vector3>>();
            List<Vector3> list = new List<Vector3>();
            list.Add(fromv3);
            findV3(allway,list, tov3);


            Debug.Log(allway);*/
            return Vector3.zero;
        }
    }

    void FindingPath(Vector3 StarPos, Vector3 EndPos)
    {
        Node startNode = mygrid.GetFromPos(StarPos);
        Node endNode = mygrid.GetFromPos(EndPos);
        List<Node> openSet = new List<Node>();
        HashSet<Node> closeSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closeSet.Add(currentNode);

            if (currentNode == endNode)
            {
                GeneratePath(startNode, endNode);
                return;
            }
            //判断周围最优节点
            foreach (var item in mygrid.GetNeibourhood(currentNode))
            {
                if (item.mapindex != 0 || closeSet.Contains(item))
                    continue;
                int newCost = currentNode.gCost + GetDistanceNodes(currentNode, item);
                if (newCost < item.gCost || !openSet.Contains(item))
                {
                    item.gCost = newCost;
                    item.hCost = GetDistanceNodes(item, endNode);
                    item.parent = currentNode;
                    if (!openSet.Contains(item))
                    {
                        openSet.Add(item);
                    }
                }

            }
        }
    }

    private void GeneratePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node temp = endNode;
        while (temp != startNode)
        {
            path.Add(temp);
            temp = temp.parent;
        }
        //列表反转
        path.Reverse();
        mygrid.path = path;

    }

    int GetDistanceNodes(Node a, Node b)
    {
        //估算权值,对角线算法 看在X轴还是Y轴格子数多  可计算斜移动
        int cntX = Mathf.Abs(a._gridX - b._gridX);
        int cntY = Mathf.Abs(a._gridY - b._gridY);
        if (cntX > cntY)
        {
            return 14 * cntY + 10 * (cntX - cntY);
        }
        else
        {
            return 14 * cntX + 10 * (cntY - cntX);
        }

        //曼哈顿算法
        //return Mathf.Abs(a._gridX - b._gridX) * 10 + Mathf.Abs(a._gridY - b._gridY) * 10;
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
