using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace QFramework
{
    /// <summary>
    /// 继承该接口才能获得Architecture
    /// </summary>
    public interface IBelongTotArchitecture
    {
        IArchitecture GetArchitecture();
    }
}

