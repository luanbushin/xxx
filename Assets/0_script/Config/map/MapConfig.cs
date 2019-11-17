
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MapConfig
{
    public string[] boxNames;
    public GameObject[] mapItemList;

    public string[] flowerNames;
    public GameObject[] flowersList;

    public Text debugtxt;

    public MapConfig(Text txt) {
        debugtxt = txt;
        getBoxNames();
    }
    private void getBoxNames()
    {
        string boxname = "";
        string flower = "";
        string fullPath = Application.dataPath+"/Resources/map/Prefabs" + "/";
        debugtxt.text = Directory.Exists(fullPath)+"";
        int i;
        if (Directory.Exists(fullPath))
        {
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);


            for (i = 0; i < files.Length; i++)
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


        for ( i = 0; i < boxNames.Length; i++)
        {
            debugtxt.text = boxNames[i];
            mapItemList[i] = (GameObject)Resources.Load("map/Prefabs/" + boxNames[i]);
        }
        for ( i = 0; i < flowerNames.Length; i++)
        {
            flowersList[i] = (GameObject)Resources.Load("map/Prefabs/" + flowerNames[i]);
        }
    }
}