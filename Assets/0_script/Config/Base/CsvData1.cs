using System.Collections.Generic;
using OnVoid = Game.Config.DataAll.OnVoid;

namespace Game.Config
{
    public abstract class CsvData1<T, V>
        where T : CsvData1<T, V>, new()
        where V : CsvValueParser<V>, new()
    {
        private static T _instance = default(T);
        public static T instance
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

        public Dictionary<int, V> data { get; private set; }
        protected void _start(OnVoid cb)
        {
            Csv.instance.parse1(
            PathInfo.STREAM_URL + "Configs/Csv/" + fileName,
            key1,
            (Dictionary<int, CsvValue> dt) =>
            {
                data = new Dictionary<int, V>();
                foreach (var pair in dt)
                {
                    data[pair.Key] = new V().tryParse(pair.Value);
                }
                
                if (cb != null) cb();
            });
        }
        
        public static void start(OnVoid cb)
        {
            instance._start(cb);
        }

        public static V get(int id)
        {
            var _data = instance.data;
            if (_data == null) return null;
            if (_data.ContainsKey(id))
            {
                return _data[id];
            }
            return null;
        }
        
    }
}
