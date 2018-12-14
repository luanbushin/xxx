using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Game.Noticfacation
{
    public abstract class NEvt
    {
        public abstract void removeTargetListeners(object tar);
    }

    public class NEvt_0 : NEvt
    {
        public delegate void OnVoid();
        private OnVoid _listeners = null;
        private Dictionary<object, OnVoid> _obj_cache = new Dictionary<object, OnVoid>();

        public void addListener(OnVoid cb, object tar = null)
        {
            
            _listeners -= cb;
            _listeners += cb;
            if (tar == null) return;
            if (_obj_cache.ContainsKey(tar))
            {
                _obj_cache[tar] -= cb;
                _obj_cache[tar] += cb;
            }
            else
            {
                _obj_cache[tar] = cb;
            }
        }

        public override void removeTargetListeners(object tar)
        {
            if (_obj_cache.ContainsKey(tar))
            {
                _listeners -= _obj_cache[tar];
                _obj_cache.Remove(tar);
            }
        }

        public void removeListener(OnVoid cb, object tar = null)
        {
            _listeners -= cb;
            if (tar == null) return;
            if (_obj_cache.ContainsKey(tar))
            {
                _obj_cache[tar] -= cb;
                if (_obj_cache[tar] == null) _obj_cache.Remove(tar);
            }
        }
        
        public void broadcast()
        {
            if (_listeners == null) return;
            _listeners.Invoke();
            
        }
    }

    public class NEvt_1<T> : NEvt
    {
        public delegate void OnArg(T v);
        private OnArg _listeners = null;
        private Dictionary<object, OnArg> _obj_cache = new Dictionary<object, OnArg>();

        public void addListener(OnArg cb, object tar = null)
        {
            _listeners -= cb;
            _listeners += cb;
            if (tar == null) return;
            if (_obj_cache.ContainsKey(tar))
            {
                _obj_cache[tar] -= cb;
                _obj_cache[tar] += cb;
            }
            else
            {
                _obj_cache[tar] = cb;
            }
        }

        public override void removeTargetListeners(object tar)
        {
            if (_obj_cache.ContainsKey(tar))
            {
                _listeners -= _obj_cache[tar];
                _obj_cache.Remove(tar);
            }
        }

        public void removeListener(OnArg cb, object tar = null)
        {
            _listeners -= cb;
            if (tar == null) return;
            if (_obj_cache.ContainsKey(tar))
            {
                _obj_cache[tar] -= cb;
                if (_obj_cache[tar] == null) _obj_cache.Remove(tar);
            }
        }

        public void broadcast(T v)
        {
            if (_listeners == null) return;
            _listeners.Invoke(v);
        }
    }

    public class NEvt_2<T1, T2> : NEvt
    {
        public delegate void OnArg(T1 v1, T2 v2);
        private OnArg _listeners = null;
        private Dictionary<object, OnArg> _obj_cache = new Dictionary<object, OnArg>();

        public void addListener(OnArg cb, object tar = null)
        {
            _listeners -= cb;
            _listeners += cb;
            if (tar == null) return;
            if (_obj_cache.ContainsKey(tar))
            {
                _obj_cache[tar] -= cb;
                _obj_cache[tar] += cb;
            }
            else
            {
                _obj_cache[tar] = cb;
            }
        }

        public override void removeTargetListeners(object tar)
        {
            if (_obj_cache.ContainsKey(tar))
            {
                _listeners -= _obj_cache[tar];
                _obj_cache.Remove(tar);
            }
        }

        public void removeListener(OnArg cb, object tar = null)
        {
            _listeners -= cb;
            if (tar == null) return;
            if (_obj_cache.ContainsKey(tar))
            {
                _obj_cache[tar] -= cb;
                if (_obj_cache[tar] == null) _obj_cache.Remove(tar);
            }
        }

        public void broadcast(T1 v1, T2 v2)
        {
            if (_listeners == null) return;
            _listeners.Invoke(v1, v2);
        }
    }

    public class NEvt_3<T1, T2, T3> : NEvt
    {
        public delegate void OnArg(T1 v1, T2 v2, T3 v3);
        private OnArg _listeners = null;
        private Dictionary<object, OnArg> _obj_cache = new Dictionary<object, OnArg>();

        public void addListener(OnArg cb, object tar = null)
        {
            _listeners -= cb;
            _listeners += cb;
            if (tar == null) return;
            if (_obj_cache.ContainsKey(tar))
            {
                _obj_cache[tar] -= cb;
                _obj_cache[tar] += cb;
            }
            else
            {
                _obj_cache[tar] = cb;
            }
        }

        public override void removeTargetListeners(object tar)
        {
            if (_obj_cache.ContainsKey(tar))
            {
                _listeners -= _obj_cache[tar];
                _obj_cache.Remove(tar);
            }
        }

        public void removeListener(OnArg cb, object tar = null)
        {
            _listeners -= cb;
            if (tar == null) return;
            if (_obj_cache.ContainsKey(tar))
            {
                _obj_cache[tar] -= cb;
                if (_obj_cache[tar] == null) _obj_cache.Remove(tar);
            }
        }

        public void broadcast(T1 v1, T2 v2, T3 v3)
        {
            if (_listeners == null) return;
            _listeners.Invoke(v1, v2, v3);
        }
    }

    public class NEvt_4<T1, T2, T3, T4> : NEvt
    {
        public delegate void OnArg(T1 v1, T2 v2, T3 v3, T4 v4);
        private OnArg _listeners = null;
        private Dictionary<object, OnArg> _obj_cache = new Dictionary<object, OnArg>();

        public void addListener(OnArg cb, object tar = null)
        {
            _listeners -= cb;
            _listeners += cb;
            if (tar == null) return;
            if (_obj_cache.ContainsKey(tar))
            {
                _obj_cache[tar] -= cb;
                _obj_cache[tar] += cb;
            }
            else
            {
                _obj_cache[tar] = cb;
            }
        }

        public override void removeTargetListeners(object tar)
        {
            if (_obj_cache.ContainsKey(tar))
            {
                _listeners -= _obj_cache[tar];
                _obj_cache.Remove(tar);
            }
        }

        public void removeListener(OnArg cb, object tar = null)
        {
            _listeners -= cb;
            if (tar == null) return;
            if (_obj_cache.ContainsKey(tar))
            {
                _obj_cache[tar] -= cb;
                if (_obj_cache[tar] == null) _obj_cache.Remove(tar);
            }
        }

        public void broadcast(T1 v1, T2 v2, T3 v3, T4 v4)
        {
            if (_listeners == null) return;
            _listeners.Invoke(v1, v2, v3, v4);
        }
    }
}