using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public interface ICanSendEvent : IBelongTotArchitecture
    {
    }
    public static class CanSendEventExtension
    {
        public static void SendEvent<T>(this ICanSendEvent self) where T : new()
        {
            self.GetArchitecture().SendEvent<T>();
        }
        public static void SendEvent<T>(this ICanSendEvent self,T e) where T : new()
        {
            self.GetArchitecture().SendEvent<T>(e);
        }
    }
}
