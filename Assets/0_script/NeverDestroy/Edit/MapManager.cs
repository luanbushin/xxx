using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public static MapManager Instance;

    private Dictionary<Vector3, GameObject> mapObj = new Dictionary<Vector3, GameObject>();

    private GameObject[] mapItemList;
    private string[] boxNames;
    public GameObject mapparent;
    // Use this for initialization

    private void Awake()
    {
        Instance = this;
    }


    void Start() {
        getBoxNames();
    }
    private void getBoxNames()
    {
        string boxname = "";
        string fullPath = "Assets/Resources/Prefabs/box" + "/";

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
            mapItemList[i] = (GameObject)Resources.Load("Prefabs/box/" + boxNames[i]);
        }
    }


    public void initmap(Dictionary<Vector3, int> mapIndexObj)
    {

        foreach (Vector3 list in mapIndexObj.Keys)
        {
            GameObject obj = GameObject.Instantiate(mapItemList[mapIndexObj[list]], list, gameObject.transform.rotation);
            obj.transform.SetParent(mapparent.transform);
            mapObj[obj.transform.position] = obj;
        }
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
