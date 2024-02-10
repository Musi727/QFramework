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
        IArchitecture IBelongTotArchitecture.GetArchitecture()//��ʾʵ�ֽӿڣ����ƣ����ǽ�ֹ���������
        {
            return _architecture;
        }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture) //��ʾʵ�ֽӿڣ����ƣ����ǽ�ֹ���������
        {
            _architecture = architecture;
        }
        void ICommand.Excute()//��ʾʵ�ֽӿڣ����ƣ����ǽ�ֹ���������
        {
            OnExecute();
        }
        protected abstract void OnExecute();
    }
}
