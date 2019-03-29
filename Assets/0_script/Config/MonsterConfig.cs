using UnityEngine;
using UnityEditor;
using System.IO;

public class MonsterConfig
{
    public string[] enemyNames;
    public GameObject[] enemyItemList;

    public MonsterConfig()
    {
        getMonsterNames();
    }
    private void getMonsterNames()
    {
        string enemyname = "";

        string fullPath = Application.dataPath + "/Resources/enemy/" + "";
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


                if (enemyname.Length > 0)
                    enemyname += ";";
                enemyname += str;
            }

            enemyNames = enemyname.Split(';');
        }

        enemyItemList = new GameObject[enemyNames.Length];


        for (int i = 0; i < enemyNames.Length; i++)
        {
            enemyItemList[i] = (GameObject)Resources.Load("enemy/" + enemyNames[i]);
        }

    }

}