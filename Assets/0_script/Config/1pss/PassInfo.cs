using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Config;

public class PassInfo  {
    public string passtye;
    public List<Vector3> tagretV3List;
    public string targetStr;
    public List<Vector3> bornList;

    public List<TrapVo> trapList;

    public void setTraget(string str) {
        targetStr = str;
        string[] arr = str.Split('=');
        tagretV3List = new List<Vector3>(arr.Length);
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i].Split(',').Length > 1)
            {
                Vector3 v = new Vector3(int.Parse(arr[i].Split(',')[0]), int.Parse(arr[i].Split(',')[1]), int.Parse(arr[i].Split(',')[2]));
                tagretV3List.Add(v);
            }
        }
    }

    public void settrap(string str) {
        string[] arr1 = str.Split('t');
        trapList = new List<TrapVo>(arr1.Length);
        for (int i = 0; i < arr1.Length; i++) {
            string[] arr = arr1[i].Split('=');
            if (arr[0] == "1")
            {
                TrapDoor trap = new TrapDoor();
                trap.type = 1;
                trap.inittrap(arr);
                trapList.Add(trap);
            }
            else if (arr[0] == "2")
            {
                MonsterRefreshVo trap = new MonsterRefreshVo();
                trap.type = 2;
                trap.inittrap(arr);
                trapList.Add(trap);
            }
            else if (arr[0] == "3")
            {
                AtkOffice trap = new AtkOffice();
                trap.type = 3;
                trap.inittrap(arr);
                trapList.Add(trap);
            }
            else if (arr[0] == "4")
            {
                CubeTrap trap = new CubeTrap();
                trap.type = 4;
                trap.inittrap(arr);
                trapList.Add(trap);
            }
            else if (arr[0] == "5") {
                Transfer trap =  new Transfer();
                trap.type = 5;
                trap.inittrap(arr);
                trapList.Add(trap);
            }
            else if (arr[0] == "6")
            {
                TrapLandmine trap = new TrapLandmine();
                trap.type = 6;
                trap.inittrap(arr);
                trapList.Add(trap);
            }
            else if (arr[0] == "7")
            {
                TrapSpring trap = new TrapSpring();
                trap.type = 7;
                trap.inittrap(arr);
                trapList.Add(trap);
            }


        }
    }

    public void setborn(string str)
    {
        string[] arr = str.Split('=');
        bornList = new List<Vector3>(arr.Length);
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i].Split(',').Length >1) { 
                Vector3 v = new Vector3(int.Parse(arr[i].Split(',')[0]), int.Parse(arr[i].Split(',')[1]), int.Parse(arr[i].Split(',')[2]));
                bornList.Add(v);
            }
        }
    }
}
