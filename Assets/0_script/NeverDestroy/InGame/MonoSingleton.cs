using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Global
{
    public class GONeverDie
    {
        private static GameObject _go = null;
        public static GameObject go
        {
            get
            {
                if (_go == null)
                {
                    _go = new GameObject();
                    _go.name = "NeverDie";
                    Object.DontDestroyOnLoad(_go);
                }
                return _go;
            }
        }
    }

    public abstract class MonoSingleton<T> : MonoNotice
        where T : MonoBehaviour
    {
        private static T _instance = null;
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GONeverDie.go.AddComponent<T>();
                }
                return _instance;
            }
        }

    }
}
