using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public interface ICanSendQuery : IBelongTotArchitecture
    {
    }
    public static class CanSendQueryExtension
    {
        public static T SendQuery<T>(this ICanSendQuery self,IQuery<T> query)
        {
            return self.GetArchitecture().SendQuery<T>(query);
        }
    }
}