
using System.IO;
using UnityEngine;

public class MapConfig
{
    public string[] boxNames;
    public GameObject[] mapItemList;

    public string[] flowerNames;
    public GameObject[] flowersList;

    public MapConfig() {
        getBoxNames();
    }
    private void getBoxNames()
    {
        string boxname = "";
        string flower = "";
        string fullPath = "Assets/Resources/map/Prefabs" + "/";

        if (Directory.Exists(fullPath))
        {
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);


            for (int i = 0; i < files.Length; i++)
            {
                if (!files[i].Name.EndsWith(".prefab"))
                {
                    continue;
                }
                string str = files[i].Name.Replace(".prefab", "");

                if (files[i].Name.IndexOf("Plants_Flower") > -1)
                {
                    if (flower.Length > 0)
                        flower += ";";
                    flower += str;
                }
                

                if (boxname.Length > 0)
                    boxname += ";";
                boxname += str;
            }
            
            boxNames = boxname.Split(';');
            flowerNames = flower.Split(';');
        }

        mapItemList = new GameObject[boxNames.Length];
        flowersList = new GameObject[flowerNames.Length];


        for (int i = 0; i < boxNames.Length; i++)
        {
            mapItemList[i] = (GameObject)Resources.Load("map/Prefabs/" + boxNames[i]);
        }
        for (int i = 0; i < flowerNames.Length; i++)
        {
            flowersList[i] = (GameObject)Resources.Load("map/Prefabs/" + flowerNames[i]);
        }
    }
}