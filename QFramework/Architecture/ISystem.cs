using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public interface ISystem : IBelongTotArchitecture,ICanSetArchitecture,ICanGetUtility, ICanGetModel,ICanSendEvent,ICanRegisterEvent,ICanGetSystem
    {
        void Init();
    }
    public abstract class AbstractSystem : ISystem
    {
        private IArchitecture _architecture = null;
        IArchitecture IBelongTotArchitecture.GetArchitecture() //ͨ��ʵ�ֽӿڣ��˸�÷�������������ֱ�ӷ���
        {
            return _architecture;
        }
        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)//ͨ��ʵ�ֽӿڣ��˸�÷�������������ֱ�ӷ���
        {
            _architecture = architecture;
        }
        void ISystem.Init()
        {
            OnInit();
        }
        protected abstract void OnInit();
    }
}
