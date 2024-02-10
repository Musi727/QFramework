using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace QFramework
{
    public class Singleton<T> where T : class
    {
        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    // 通过反射获取构造， BindingFlags.Instance 表示实例构造函数，BindingFlags.NonPublic 表示非公共构造函数。
                    var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                    // 获取无参非 public 的构造,用于单例的安全校验
                    var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);

                    if (ctor == null)
                    {
                        throw new Exception("Non-Public Constructor() not found in " + typeof(T));
                    }

                    mInstance = ctor.Invoke(null) as T;
                }
                return mInstance;
            }
        }

        private static T mInstance;
    }

}

