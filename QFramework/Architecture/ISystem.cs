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
        IArchitecture IBelongTotArchitecture.GetArchitecture() //通过实现接口，阉割该方法，限制子类直接访问
        {
            return _architecture;
        }
        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)//通过实现接口，阉割该方法，限制子类直接访问
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
