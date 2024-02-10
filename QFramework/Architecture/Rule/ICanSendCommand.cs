using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace QFramework
{
    public interface ICanSendCommand : IBelongTotArchitecture
    {
    }
    public static class CanSendCommandExtension
    {
        public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand,new()
        {
            self.GetArchitecture().SendCommand<T>();
        }
        public static void SendCommand<T>(this ICanSendCommand self,T Command) where T : ICommand
        {
            self.GetArchitecture().SendCommand<T>(Command);
        }
    }
}
