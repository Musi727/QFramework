using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    /// <summary>
    /// �̳иýӿڲ��ܷ���System
    /// </summary>
    public interface ICanGetSystem : IBelongTotArchitecture
    {
    }
    public static class CanGetSystemExtension
    {
        public static T GetSystem<T>(this ICanGetSystem self) where T : class, ISystem
        {
            return self.GetArchitecture().GetSystem<T>();
        }
    }
}
