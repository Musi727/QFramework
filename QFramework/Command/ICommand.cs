using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public interface ICommand : IBelongTotArchitecture, ICanSetArchitecture, ICanGetSystem, ICanGetModel, ICanGetUtility,ICanSendEvent,ICanSendCommand,ICanSendQuery
    {
        void Excute();
    }
    public abstract class AbstractCommand : ICommand
    {
        private IArchitecture _architecture = null;
        IArchitecture IBelongTotArchitecture.GetArchitecture()//显示实现接口，限制（不是禁止）子类访问
        {
            return _architecture;
        }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture) //显示实现接口，限制（不是禁止）子类访问
        {
            _architecture = architecture;
        }
        void ICommand.Excute()//显示实现接口，限制（不是禁止）子类访问
        {
            OnExecute();
        }
        protected abstract void OnExecute();
    }
}
