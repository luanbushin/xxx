using UnityEngine;
using System.Collections;
using System;

namespace Game.Collector
{
    public class Error
    {
        public static void add(object target, Exception e, string info)
        {
            addWithType(target.GetType(), e, info);
        }

        public static void add(object target, string error)
        {
            addWithType(target.GetType(), error);
        }

        public static void addWithType(Type target, Exception e, string info)
        {
            var str = string.Format("[Error : {0}] - {1}\n- {2}", target.Name, e.ToString(), info);
            handleError(str);
        }

        public static void addWithType(Type target, string error)
        {
            var str = string.Format("[Error : {0}] - {1}", target.Name, error);
            handleError(str);
        }

        private static void handleError(string str)
        {
            Debug.LogWarning(str);

        }
    }
}
