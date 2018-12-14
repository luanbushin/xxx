using UnityEngine;

namespace Game
{
    public class Log
    {
        public static void bytes2sbytes(byte[] bytes, string tag = "[sbytes]")
        {
            if (bytes == null) return;
            if (bytes.Length == 0) return;
            var str = "";
            for (int i = 0; i < bytes.Length; ++i)
            {
                sbyte b = (sbyte)bytes[i];
                if (i == 0)
                {
                    str += b.ToString();
                }
                else
                {
                    str += " " + b.ToString();
                }
                
            }
            Debug.LogFormat("{0} -> {1}", tag, str);
        }

        public static void vec3(Vector3 v, string tag = "[vec3]")
        {
            Debug.LogFormat("{0} -> ({1}, {2}, {3})", tag, v.x, v.y, v.z);
        }

        public static void vec2(Vector2 v, string tag = "[vec2]")
        {
            Debug.LogFormat("{0} -> ({1}, {2})", tag, v.x, v.y);
        }

        public static void join(string separator, params object[] args)
        {
            LogArgs.join(separator, args);
        }

        public static void with(object target, params object[] args)
        {
            LogArgs.with(target, args);
        }

        public static void info(params object[] args)
        {
            LogArgs.info(args);
        }

        public static void error(params object[] args)
        {
            LogArgs.error(args);
        }

        public static void warning(params object[] args)
        {
            LogArgs.warning(args);
        }
    }

    public class LogArgs
    {
        public static void join<T>(string separator, T[] args)
        {
            Debug.Log(string.Join(separator, ArrayTransfer.arr2strArr(args)));
        }

        public static void with<T>(object target, T[] args)
        {
            Debug.LogFormat("[{0}] -> {1}", target.GetType().Name, ArrayTransfer.arr2str(args));
        }

        public static void info<T>(T[] args)
        {
            Debug.Log(ArrayTransfer.arr2str(args));
        }

        public static void error<T>(T[] args)
        {
            Debug.LogError(ArrayTransfer.arr2str(args));
        }

        public static void warning<T>(T[] args)
        {
            Debug.LogWarning(ArrayTransfer.arr2str(args));
        }
    }

    public class LogFields
    {
        private static string filedsStr<T>(T tar)
        {
            string ret = "";
            var t = typeof(T);
            foreach(var info in t.GetFields())
            {
                ret += info.FieldType.Name + " " + info.Name + " = " + info.GetValue(tar) + "\n";
            }
            return ret;
        }

        public static void info<T>(T tar)
        {
            Debug.Log(filedsStr(tar));
        }

        public static void error<T>(T tar)
        {
            Debug.LogError(filedsStr(tar));
        }

        public static void warning<T>(T tar)
        {
            Debug.LogWarning(filedsStr(tar));
        }
    }
}
