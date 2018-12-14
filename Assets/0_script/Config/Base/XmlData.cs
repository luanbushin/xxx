using UnityEngine;
using System.Collections;
using System.Xml;
using OnVoid = Game.Config.DataAll.OnVoid;

namespace Game.Config
{
    public abstract class XmlData<T> : Global.MonoSingleton<T>
        where T : XmlData<T>
    {
        public int id { get; protected set; }
        
        public static void start(OnVoid cb)
        {
            instance.load(cb);
        }

        protected void load(OnVoid cb)
        {
            StartCoroutine(_load(cb));
        }

        private IEnumerator _load(OnVoid cb)
        {
            var url = PathInfo.STREAM_URL + "Configs/Xml/" + fileName;
            WWW www = new WWW(url);
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                var xml = new XmlDocument();
                var set = new XmlReaderSettings();
                set.IgnoreComments = true;
                xml.LoadXml(www.text);
                onParse(xml);
                if (cb != null) cb();
            }
            else
            {
                Collector.Error.add(this, url + " : " + www.error);
            }
        }

        protected abstract string fileName { get; }
        protected abstract void onParse(XmlDocument xml);
    }
}