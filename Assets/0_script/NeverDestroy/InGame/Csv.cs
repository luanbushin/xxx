using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using Game.Global;
using Game.Config;

namespace Game.Config
{
    public abstract class CsvValueParser<V>
        where V : CsvValueParser<V>
    {
        public V tryParse(CsvValue v)
        {
            try
            {
                return parse(v);
            }
            catch (Exception ex)
            {
                Log.warning(v.debugInfo);
                Collector.Error.add(this, ex, v.debugInfo);
                return default(V);
            }
        }
        public abstract V parse(CsvValue v);

    }

    public class CsvValue
    {
        public static readonly CsvValue DEFAULT = new CsvValue();
        private Dictionary<string, string> _strs = new Dictionary<string, string>();
        private Dictionary<string, int> _ints = new Dictionary<string, int>();
        private Dictionary<string, float> _floats = new Dictionary<string, float>();

        private string[] _items = null;
        private string[] _keys = null;
        private string _file_name = null;

        public string debugInfo
        {
            get
            {
                return _file_name + "\n" +
                    string.Join(" | ", _keys) + "\n" +
                    string.Join(" | ", _items);
            }
        }
        public void parseLine(string fileName, string[] keys, string[] items)
        {
            _file_name = fileName;
            _keys = keys;
            _items = items;
#if UNITY_EDITOR
            if (keys.Length != items.Length)
            {
                Log.error("parseLine keys.Length != items.Length");
            }
#endif
            for(int i = 0; i < keys.Length; ++i)
            {
                string item = items[i];
                try
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        // 有些没填的数据也可能作为key
                        // 没填的都当作是""
                        _strs[keys[i]] = "";
                    }
                    else if (item.StartsWith("\""))
                    {
                        item = item.Remove(item.Length - 1, 1); // 先去尾
                        item = item.Remove(0, 1); // 后去头
                        // 如果反过来则是 item.Length - 2
                        _strs[keys[i]] = item;
                    }
                    else if (item.IndexOf('.') >= 0)
                    {
                        float v;
                        if (float.TryParse(item, out v))
                            _floats[keys[i]] = v;
                        else
                            _floats[keys[i]] = 0f;
                    }   
                    else
                    {
                        if (string.IsNullOrEmpty(item))
                        {
                            _floats[keys[i]] = 0f;
                            _ints[keys[i]] = 0;
                        }
                        else
                        {
                            float vf;
                            if (float.TryParse(item, out vf))
                                _floats[keys[i]] = vf;
                            else
                                _floats[keys[i]] = 0f;

                            int vi;
                            if (int.TryParse(item, out vi))
                                _ints[keys[i]] = vi;
                            else
                                _ints[keys[i]] = 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Collector.Error.add(this, ex, "item = " + item);
                }
            }
        }

        public void findKey(string name)
        {
            if (_ints.ContainsKey(name))
            {
                key = _ints[name];
            }
            else
            {
                Log.error("no key found in Csv!", name, getInt("ID"));
            }
        }

        public void findKey2(string name)
        {
            if (_ints.ContainsKey(name))
            {
                key2 = _ints[name];
            }
            else
            {
                Log.error("no key found in Csv!", name, getInt("ID"));
            }
        }

        public string fix(string src, char sep)
        {
            if (src == null) return null;
            if (src.EndsWith(sep.ToString()))
            {
                var ret = src.Substring(0, src.Length - 1);
                return ret;
            }
            return src;
        }

        public string[] split(string key_, char sep = ';')
        {
            var str = fix(getStr(key_), sep);
            if (string.IsNullOrEmpty(str)) return new string[0];
            return str.Split(sep);
        }
    
        public T[] splitEnum<T>(string key_, char sep = ';')
        {
            var strs = split(key_, sep);
            int len = strs.Length;
            var ret = new T[len];
            for(int i = 0; i < len; ++i)
            {
                ret[i] = str2enum<T>(strs[i]);
            }
            return ret;
        }

        public int[] splitInt(string key_, char sep = ';')
        {
            var strs = split(key_, sep);
            int len = strs.Length;
            var ret = new int[len];
            for (int i = 0; i < len; ++i)
            {
                ret[i] = int.Parse(strs[i]);
            }
            return ret;
        }

        public int[][] splitInt2(string key_, char sep1 = ';', char sep2 = ' ')
        {
            var strs1 = split(key_, sep1);
            var len1 = strs1.Length;
            var ret1 = new int[len1][];
            for(int i = 0; i < len1; ++i)
            {
                var strs2 = string.IsNullOrEmpty(strs1[i]) ?
                    new string[0] : fix(strs1[i], sep2).Split(sep2);
                var len2 = strs2.Length;
                var ret2 = new int[len2];
                for(int j = 0; j < len2; ++j)
                {
                    ret2[j] = int.Parse(strs2[j]);
                }
                ret1[i] = ret2;
            }
            return ret1;
        }

        public string getStr(string key_)
        {
            if (_strs.ContainsKey(key_))
            {
                return _strs[key_];
            }
            return null;
        }

        public float getFloat(string key_)
        {
            if (_floats.ContainsKey(key_))
            {
                return _floats[key_];
            }
            return 0f;
        }

        public int getInt(string key_)
        {
            if (_ints.ContainsKey(key_))
            {
                return _ints[key_];
            }
            return 0;
        }

        public T getEnum<T>(string key_)
        {
            return str2enum<T>(getStr(key_));
        }

        public T str2enum<T>(string str)
        {
            if (string.IsNullOrEmpty(str))
                return default(T);
            return (T)System.Enum.Parse(typeof(T), str);
        }

        public int key { get; private set; }
        public int key2 { get; private set; }
    }
    
    public class Csv : MonoSingleton<Csv>
    {
        private static string[] line2items(string line)
        {
            var buffer = new List<string>();
            string curr = "";
            bool inStr = false;
            for (int i = 0; i < line.Length; ++i)
            {
                char c = line[i];
                if (c == '\"')
                {
                    inStr = !inStr; // 是否在字符串内
                    curr += c;
                }
                else if (c == ',') // 只有在字符串外的','才作为分隔符
                {
                    if (inStr) // 字符串内 不分割
                    {
                        curr += c;
                    }
                    else // 分割
                    {
                        buffer.Add(curr);
                        curr = "";
                    }
                }
                else if (c == ' ' && !inStr) // 字符串外的空格都去掉
                {
                    continue; // skip
                }
                else // 其他情况添加这个字符
                {
                    curr += c;
                }
            }
            buffer.Add(curr); // 最后还需要再加一次
            return buffer.ToArray();
        }

        public delegate void OnValues1(Dictionary<int, CsvValue> dic);
        public delegate void OnValues2(Dictionary<int, Dictionary<int, CsvValue>> dic);

        private delegate void OnString(string str);
        private IEnumerator load(string url, OnString cb)
        {
            var www = new WWW(url);
            yield return www;
            if (!www.isDone)
            {
                Collector.Error.add(this, "load1 www not done!");
                yield break;
            }
            if (!string.IsNullOrEmpty(www.error))
            {
                Collector.Error.add(this, url + " : " + www.error);
                yield break;
            }
            cb(www.text);
        }
        
        public void  parse1(string url, string key, OnValues1 cb)
        {
            StartCoroutine(load1(url, key, cb));
        }

        private IEnumerator load1(string url, string key, OnValues1 cb)
        {
            yield return load(url, (string text)=>
            {
                var dic = new Dictionary<int, CsvValue>();
                string[] keys = null;
                using (StringReader sr = new StringReader(text))
                {
                    string line;
                    bool is_head_line = true;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (is_head_line)
                        {
                            is_head_line = false;
                            keys = line2items(line);
                        }
                        else
                        {
                            var csv_value = new CsvValue();
                            csv_value.parseLine(
                                Path.GetFileName(url), keys, line2items(line));
                            csv_value.findKey(key);
                            dic[csv_value.key] = csv_value;
                        }
                    }
                }

                cb(dic);
            });
        }

        public void parse2(string url, string key1, string key2, OnValues2 cb)
        {
            StartCoroutine(load2(url, key1, key2, cb));
        }

        private IEnumerator load2(string url, string key1, string key2, OnValues2 cb)
        {
            yield return load(url, (string text) =>
            {
                var dic = new Dictionary<int, Dictionary<int, CsvValue>>();
                string[] keys = null;
                using (StringReader sr = new StringReader(text))
                {
                    string line;
                    bool is_head_line = true;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (is_head_line)
                        {
                            is_head_line = false;
                            keys = line2items(line);
                        }
                        else
                        {
                            var csv_value = new CsvValue();
                            csv_value.parseLine(
                                Path.GetFileName(url), keys, line2items(line));
                            csv_value.findKey(key1);
                            csv_value.findKey2(key2);
                            if (!dic.ContainsKey(csv_value.key))
                            {
                                dic[csv_value.key] = new Dictionary<int, CsvValue>();
                            }
                            dic[csv_value.key][csv_value.key2] = csv_value;

                        }
                    }
                }

                cb(dic);
            });
        }

    }
}