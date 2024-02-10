using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public interface IModel :IBelongTotArchitecture,ICanSetArchitecture,ICanGetUtility,ICanSendEvent
    {
        void Init();
    }
    public abstract class AbstractModel : IModel
    {
        private IArchitecture _architecture = null;
        IArchitecture IBelongTotArchitecture.GetArchitecture() //使用显示实现接口，将其阉割，不允许子类能直接调用该方法
        {
            return _architecture;
        }
        public void SetArchitecture(IArchitecture architecture)
        {
            _architecture = architecture;
        }
        void IModel.Init()
        {
            OnInit();
        }
        protected abstract void OnInit();
    }
}

