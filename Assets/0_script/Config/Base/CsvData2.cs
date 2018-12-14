using System.Collections.Generic;
using OnVoid = Game.Config.DataAll.OnVoid;

namespace Game.Config
{
    public abstract class CsvData2<T, V>
        where T : CsvData2<T, V>, new()
        where V : CsvValueParser<V>, new()
    {
        private static T _instance = default(T);
        private static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
        protected abstract string fileName { get; }
        protected string key1 { get { return "ID"; } }
        protected abstract string key2 { get; }

        private Dictionary<int, Dictionary<int, V>> _data;
        public void _start(OnVoid cb)
        {
            Csv.instance.parse2(
            PathInfo.STREAM_URL + "Configs/Csv/" + fileName,
            key1, key2,
            (Dictionary<int, Dictionary<int, CsvValue>> dt) =>
            {
                _data = new Dictionary<int, Dictionary<int, V>>();
                foreach (var p in dt)
                {
                    _data[p.Key] = new Dictionary<int, V>();
                    foreach (var pair in p.Value)
                    {
                        _data[p.Key][pair.Key] = new V().tryParse(pair.Value);
                    }
                }
                if (cb != null) cb();
            });
        }

        public static void start(OnVoid cb)
        {
            instance._start(cb);
        }

        public static V get(int id, int level)
        {
            var _data = instance._data;
            if (_data == null) return null;
            if (_data.ContainsKey(id))
            {
                if (_data[id].ContainsKey(level))
                {
                    return _data[id][level];
                }
            }
            return null;
        }
    }
}
