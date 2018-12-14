using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Config
{
    public class Transfer : TrapVo
    {
        public Vector3 v3;
        public Vector3 tv3;

        public int curcd;

        public void inittrap(string[] arr)
        {
            string[] arr2 = arr[1].Split(',');
            v3 = new Vector3(int.Parse(arr2[0]), int.Parse(arr2[1])+1, int.Parse(arr2[2]));

            arr2 = arr[2].Split(',');
            tv3 = new Vector3(int.Parse(arr2[0]), int.Parse(arr2[1]), int.Parse(arr2[2]));
        }
    }
}