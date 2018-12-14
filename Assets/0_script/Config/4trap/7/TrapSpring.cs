using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Config
{
    public class TrapSpring : TrapVo
    {
        public Vector3 v3;
        public float rotiony;
        public float range;

        public float curRange;
        public int curLiveEnemy;

        public Boolean isshrink = false;

        public void inittrap(string[] arr) {
            curRange = -1;
            string[] arr2 = arr[1].Split(',');
            v3 = new Vector3(int.Parse(arr2[0]), int.Parse(arr2[1]), int.Parse(arr2[2]));
            rotiony = float.Parse(arr2[3]);

            string[] arr1 = arr[2].Split(',');

            range = float.Parse(arr1[0]);

        }

    }
}