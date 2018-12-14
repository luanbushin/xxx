using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Config
{
    public class AtkOffice : TrapVo
    {
        public Vector3 v3;
        public int atkCD;
        public int speed;
        public int rang;
        public int atkType;
        public int buffid;
        public int continued;

        public int curcd;

        public void inittrap(string[] arr)
        {
            string[] arr2 = arr[1].Split(',');
            v3 = new Vector3(int.Parse(arr2[0]), int.Parse(arr2[1]), int.Parse(arr2[2]));

            arr2 = arr[2].Split(',');
            atkCD = int.Parse(arr2[0]);
            speed = int.Parse(arr2[1]);
            rang = int.Parse(arr2[2]);
            atkType = int.Parse(arr2[3]);
            buffid = int.Parse(arr2[4]);
            continued = int.Parse(arr2[5]);
        }
    }
}