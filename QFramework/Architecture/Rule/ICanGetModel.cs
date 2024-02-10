using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    /// <summary>
    /// �̳иýӿڲ��ܷ���Model
    /// </summary>
    public interface ICanGetModel : IBelongTotArchitecture
    {
    }
    public static class CanGetModelExtension
    {
        public static T GetModel<T>(this ICanGetModel self) where T : class, IModel
        {
            return self.GetArchitecture().GetModel<T>();
        }
    }
}