using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    /// <summary>
    /// 继承该接口才能 给Architecture赋值
    /// </summary>
    public interface ICanSetArchitecture 
    {
        void SetArchitecture (IArchitecture architecture);
    }
}
