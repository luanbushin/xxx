using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Config
{
    public class CubeTrap : TrapVo
    {
        public Vector3 v3;
        public Vector3 sv3;
        public int AtkType;
        public int buffid;
        public int delay;

        public int curcd;

        public void inittrap(string[] arr)
        {
            string[] arr2 = arr[1].Split(',');
            v3 = new Vector3(int.Parse(arr2[0]), int.Parse(arr2[1]), int.Parse(arr2[2]));
            sv3 = new Vector3(float.Parse(arr2[3])-0.1f, float.Parse(arr2[4]), float.Parse(arr2[5])-0.1f);
            arr2 = arr[2].Split(',');
            AtkType = int.Parse(arr2[0]);
            delay = int.Parse(arr2[1]);
        }
    }
}