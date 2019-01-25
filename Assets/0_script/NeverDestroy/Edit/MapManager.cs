using Game;
using Game.Noticfacation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapManager : MonoNotice
{
    public static MapManager Instance;

    private Dictionary<Vector3, GameObject> mapObj = new Dictionary<Vector3, GameObject>();

    private GameObject[] mapItemList;
    private string[] boxNames;
    public GameObject mapparent;

    public bool mazeMode = false;
    // Use this for initialization


    /*===============================================maze========================================*/
    public int[,] maplist;
    public int[,] disposeList;
    public List<List<int>> weekList;



    private void Awake()
    {
        Instance = this;
    }


    void Start() {
        getBoxNames();

        addListener(Notice.MAZE_CREAT_COMPLETE, () =>
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
            }
            boxNames = boxname.Split(';');
        }

        mapItemList = new GameObject[boxNames.Length];

        for (int i = 0; i < boxNames.Length; i++)
        {
            mapItemList[i] = (GameObject)Resources.Load("map/Prefabs/" + boxNames[i]);
        }
    }


    public void initmap(Dictionary<Vector3, int> mapIndexObj)
    {
        if (mazeMode) {
            for (int i = 0; i < 200; i++)
            {
                for (int j = 0; j < 200; j++)
                {
                    GameObject obj = GameObject.Instantiate(mapItemList[UnityEngine.Random.Range(0, 20)], new Vector3(i, 0, j), gameObject.transform.rotation);
                    obj.transform.SetParent(mapparent.transform);
                    mapObj[obj.transform.position] = obj;
                }
            }

            this.creatBaseMap(200, 200);




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

    private void creatBaseMap(int w, int h){
        maplist = new int[200, 200];
        disposeList = new int[200,200];
       weekList = new List<List<int>>();

        for (int i = 0; i < 200; i++){

            for (int j = 0; j < 200; j++){
                if (i % 2 == 1 && j % 2 == 1)
                {
                    maplist[i,j] = 1;
                }
                else
                {
                    maplist[i,j] = 0;
                }
            }
        }

        this.randomDirection(1, 1);


        //this.gettargetpoint();
    }
    private List<int> curtoarr;
    private void randomDirection(int x, int y) {
        List<List<int>> maybeList = this.maybeGoList(x, y);
		if(maybeList.Count > 0){
            int random = UnityEngine.Random.Range(0, maybeList.Count);
            List<int> toarr = maybeList[random];
            int tox = (toarr[0] - x) / 2 + x;
            int toy = (toarr[1] - y) / 2 + y;

            maplist[tox,toy] = 1;

			disposeList[toarr[0],toarr[1]] = 1;
			this.weekList.Add(toarr);
            //Debug.Log("走过"+ this.weekList.Count);

            mazeindex++;
            if (mazeindex < 100)
                this.randomDirection(toarr[0], toarr[1]);
            else {
                curtoarr = toarr;
                Invoke("detonateBoom",.2f);
            }
            return;
        }
        else{
			//console.log("一条路走到头 ，查看之前的岔道"+ this.weekList.length);
			for(var i = this.weekList.Count-1;i>=0;i--){
                if (this.maybeGoList(this.weekList[i][0], this.weekList[i][1]).Count > 0)
                {
                    this.randomDirection(this.weekList[i][0], this.weekList[i][1]);
                    return;
                }
            }
        }

        Notice.MAZE_CREAT_COMPLETE.broadcast();
        Debug.Log("==============================================================完成");
	}
    public void detonateBoom()
    {
        mazeindex = 0;
        this.randomDirection(curtoarr[0], curtoarr[1]);
    }
    private int mazeindex;
	private List<List<int>> maybeGoList(int x, int y){
        List<List<int>> maybeList = new List<List<int>>();
        if (x - 2 > 0)
        {
            if (disposeList[x - 2,y] == 0) {
                List<int> arr = new List<int>();
                arr.Add(x - 2);
                arr.Add(y);
                maybeList.Add(arr);
            }
          
        }
        if (x + 2 < 200)
        {
            //Debug.Log(disposeList[x + 2, y]);
            if (disposeList[x + 2,y] == 0)
            {
                List<int> arr = new List<int>();
                arr.Add(x + 2);
                arr.Add(y);
                maybeList.Add(arr);
            }
        }
        if (y - 2 > 0)
        {
            if (disposeList[x,y-2] == 0)
            {
                List<int> arr = new List<int>();
                arr.Add(x );
                arr.Add(y-2);
                maybeList.Add(arr);
            }
        }
        if (y + 2 < 200)
        {
            if (disposeList[x,y + 2] == 0)
            {
                List<int> arr = new List<int>();
                arr.Add(x);
                arr.Add(y + 2);
                maybeList.Add(arr);
            }
        }
        ///Debug.Log(maybeList.Count);
        return maybeList;
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








    // Update is called once per frame
    void Update () {
		
	}
}
