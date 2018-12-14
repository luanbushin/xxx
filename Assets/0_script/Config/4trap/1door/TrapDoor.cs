using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Config
{
    public class TrapDoor : TrapVo
    {
        public Vector3 v3;
        public float rotiony;
        public List<Vector3> triggerPList;
        public List<Vector3> triggerRList;

        public List<Vector3> clearPList;
        public List<Vector3> clearRList;

        public int curLiveEnemy;

        public void inittrap(string[] arr) {
            string[] arr2 = arr[1].Split(',');
            v3 = new Vector3(int.Parse(arr2[0]), int.Parse(arr2[1]), int.Parse(arr2[2]));
            rotiony = float.Parse(arr2[3]);

            triggerPList = new List<Vector3>();
            triggerRList = new List<Vector3>();
            string[] triggerStrarr = arr[2].Split(';');

            for (int i = 0; i < triggerStrarr.Length; i++) {
                string[] arr1 = triggerStrarr[i].Split(',');

                Vector3 tranv3 = new Vector3(float.Parse(arr1[0]), float.Parse(arr1[1]), float.Parse(arr1[2]));
                Vector3 seletv3 = new Vector3(float.Parse(arr1[3]), float.Parse(arr1[4]), float.Parse(arr1[5]));
                triggerPList.Add(tranv3);
                triggerRList.Add(seletv3);
            }

            clearPList = new List<Vector3>();
            clearRList = new List<Vector3>();
            string[] clearStrarr = arr[3].Split(';');

            for (int i = 0; i < clearStrarr.Length; i++)
            {
                string[] arr1 = clearStrarr[i].Split(',');

                Vector3 tranv3 = new Vector3(float.Parse(arr1[0]), float.Parse(arr1[1]), float.Parse(arr1[2]));
                Vector3 seletv3 = new Vector3(float.Parse(arr1[3]), float.Parse(arr1[4]), float.Parse(arr1[5]));
                clearPList.Add(tranv3);
                clearRList.Add(seletv3);
            }
        }

    }
}