using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Noticfacation;

namespace Game
{
    public class MonoNotice : MonoBehaviour
    {
        private List<NEvt> _evts = new List<NEvt>();
        public void addListener(NEvt_0 evt, NEvt_0.OnVoid cb)
        {
            if (!_evts.Contains(evt)) _evts.Add(evt);
            evt.addListener(cb, this);
        }

        public void addListener<T>(NEvt_1<T> evt, NEvt_1<T>.OnArg cb)
        {
            if (!_evts.Contains(evt)) _evts.Add(evt);
            evt.addListener(cb, this);
        }

        public void addListener<T1, T2>(NEvt_2<T1, T2> evt, NEvt_2<T1, T2>.OnArg cb)
        {
            if (!_evts.Contains(evt)) _evts.Add(evt);
            evt.addListener(cb, this);
        }

        public void addListener<T1, T2, T3>(NEvt_3<T1, T2, T3> evt, NEvt_3<T1, T2, T3>.OnArg cb)
        {
            if (!_evts.Contains(evt)) _evts.Add(evt);
            evt.addListener(cb, this);
        }

        public void addListener<T1, T2, T3, T4>(NEvt_4<T1, T2, T3, T4> evt, NEvt_4<T1, T2, T3, T4>.OnArg cb)
        {
            if (!_evts.Contains(evt)) _evts.Add(evt);
            evt.addListener(cb, this);
        }

        public void removeListener(NEvt_0 evt, NEvt_0.OnVoid cb)
        {
            evt.removeListener(cb, this);
        }

        public void removeListener<T>(NEvt_1<T> evt, NEvt_1<T>.OnArg cb)
        {
            evt.removeListener(cb, this);
        }

        public void removeListener<T1, T2>(NEvt_2<T1, T2> evt, NEvt_2<T1, T2>.OnArg cb)
        {
            evt.removeListener(cb, this);
        }

        public void removeListener<T1, T2, T3>(NEvt_3<T1, T2, T3> evt, NEvt_3<T1, T2, T3>.OnArg cb)
        {
            evt.removeListener(cb, this);
        }

        public void removeListener<T1, T2, T3, T4>(NEvt_4<T1, T2, T3, T4> evt, NEvt_4<T1, T2, T3, T4>.OnArg cb)
        {
            evt.removeListener(cb, this);
        }
        
        // 避免在子类中覆盖这个函数
        private void OnDestroy()
        {
            foreach(var evt in _evts)
            {
                evt.removeTargetListeners(this);
            }
            OnDispose();
        }

        virtual protected void OnDispose()
        {
            // override this
        }
    }
}
