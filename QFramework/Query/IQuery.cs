using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public interface IQuery<T> :IBelongTotArchitecture,ICanSetArchitecture,ICanGetModel,ICanGetSystem,ICanGetUtility
    {
        T Do();
    }
    public abstract class AbstractQuery<T> : IQuery<T>
    {
        private IArchitecture _architecture;
        public T Do()
        {
            return OnDo();
        }
        protected abstract T OnDo();

        IArchitecture IBelongTotArchitecture.GetArchitecture()
        {
            return _architecture;
        }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            _architecture = architecture;
        }
    }
}