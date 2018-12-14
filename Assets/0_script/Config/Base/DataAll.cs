using UnityEngine;
using System.Collections;

namespace Game.Config
{
    public class DataAll
    {
        public delegate void OnVoid();
        public delegate void OnStart(OnVoid cb);
        private static int _count = 0;
        private static OnStart _starts = null;
        public static int addStart(OnStart start)
        {
            _starts += start;
            return ++_count;
        }

        public static void start(OnVoid cb)
        {
            _starts.Invoke(() =>
            {
                if (--_count > 0) return;
                cb();
            });
        }
    }
}
