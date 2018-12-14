using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Config
{
    public class MonsterRefreshVo : TrapVo
    {
        public Vector3 v3;
        public int maxLiveEnemy;
        public int refreshCD;

        public int curLiveEnemy;
        public int currefreshindex;

        public void inittrap(string[] arr)
        {
            string[] arr2 = arr[1].Split(',');
            v3 = new Vector3(int.Parse(arr2[0]), int.Parse(arr2[1]), int.Parse(arr2[2]));

            arr2 = arr[2].Split(',');

            maxLiveEnemy = int.Parse(arr2[0]);
            refreshCD = int.Parse(arr2[1]);
        }
    }
}