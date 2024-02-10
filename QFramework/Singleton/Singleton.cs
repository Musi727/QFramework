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
                    // ͨ�������ȡ���죬 BindingFlags.Instance ��ʾʵ�����캯����BindingFlags.NonPublic ��ʾ�ǹ������캯����
                    var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                    // ��ȡ�޲η� public �Ĺ���,���ڵ����İ�ȫУ��
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

