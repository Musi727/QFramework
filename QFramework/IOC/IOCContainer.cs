using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public  class IOCContainer 
    {
        public Dictionary<Type,object> instances = new Dictionary<Type, object> ();
        public void Register<T> (T instance)
        {
            var key = typeof(T);
            if (instances.ContainsKey(key))
            {
                instances[key] = instance;
            }
            else
            {
                instances.Add(key, instance);
            }
        }
        public T Get<T>() where T : class
        {
            var key = typeof(T);
            if (instances.ContainsKey(key))
            {
                return instances[key] as T;
            }
            return null;
        }
    }
}