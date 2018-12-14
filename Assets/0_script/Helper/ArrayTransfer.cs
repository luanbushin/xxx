using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ArrayTransfer
    {
        public static string[] arr2strArr<T>(T[] arr)
        {
            if (arr == null) return new string[0];
            var len = arr.Length;
            var str = new string[len];
            for (int i = 0; i < len; ++i)
            {
                str[i] = (arr[i] == null ? "null" : arr[i].ToString());
            }
            return str;
        }

        public static string arr2str<T>(T[] args)
        {
            return string.Join(" ", arr2strArr(args));
        }

        public static sbyte[] int2sbyte(int[] src)
        {
            var ret = new sbyte[src.Length];
            for (int i = 0; i < src.Length; ++i)
            {
                ret[i] = (sbyte)src[i];
            }
            return ret;
        }

        public static short[] int2short(int[] src)
        {
            var ret = new short[src.Length];
            for(int i = 0; i < src.Length; ++i)
            {
                ret[i] = (short)src[i];
            }
            return ret;
        }
    }
}