using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace QFramework
{
    //对于一个架构来说，需要注册的有很多，我们能将其进行分类，Utility工具类（交互逻辑），Model数据类（数据层）
    public interface IArchitecture
    {
        // 提供一个获取 Utility 的 API
        TUtility GetUtility<TUtility>() where TUtility : class; //获得Utility
        TModel GetModel<TModel>() where TModel : class, IModel; //获得Model
        TSystem GetSystem<TSystem>() where TSystem : class, ISystem;//获得System
        void RegisterUtility<TUtility>(TUtility UtilityModule);//注册Utility,例如IStoragy
        void RegisterModel<TModel>(TModel ModelModule) where TModel : IModel;//注册Model,例如counterApp
        void RegisterSystem<TSystem>(TSystem SystemmMdule) where TSystem : ISystem;
        void SendCommand<TCommand>() where TCommand : ICommand,new (); //发送命令
        void SendCommand<TCommand>(TCommand Command) where TCommand : ICommand; //发送命令
        void SendEvent<TEvent>() where TEvent : new(); //发送事件
        void SendEvent<TEvent>(TEvent e); //发送事件
        IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent); //注册事件
        void UnRegisterEvent<TEvent>(Action<TEvent> onEvent); //注销事件
        TResult SendQuery<TResult>(IQuery<TResult> query); //查询数据
    }
    public abstract class Architecture<T>: IArchitecture where T : Architecture<T>, new()
    { 
        private static T _architecture = null; //存储了类型为 T 的 Architecture<T> 类的实例
        /// <summary>
        /// 提供一个单例接口,让Controller层能够访问Architecture内部（Model,Utility,System）
        /// </summary>
        public static IArchitecture Interface
        {
            get
            {
                if(_architecture == null )
                {
                    MakeSureArchitecture();
                }
                return _architecture;
            }
        }
        private bool _inited = false; //是否已经初始化完成
        private List<IModel> _Models = new List<IModel>(); //用于存放Models缓存
        public static Action<T> OnRegisterPatch = architecture => { };
        private List<ISystem> _Systems = new List<ISystem>();
        private static void MakeSureArchitecture() //静态方法
        {
            if (_architecture == null)
            {
                _architecture = new T();
                _architecture.Init();

                OnRegisterPatch?.Invoke(_architecture);
                //初始化Model
                foreach(var architectureModel in _architecture._Models)
                {
                    architectureModel.Init();
                }
                //清空Model
                _architecture._Models.Clear();
                //初始化System
                foreach(var architectureSystem in _architecture._Systems)
                {
                    architectureSystem.Init();
                }
                //清空System
                _architecture._Systems.Clear();
                _architecture._inited = true;
            }
        }
        private IOCContainer mContainer = new IOCContainer();
        //提供一个子类 注册模块
        protected abstract  void Init();

        //注册模块的API
        //public static void Register<T>(T instance)
        //{
        //    MakeSureArchitecture();
        //    _architecture.mContainer.Register(instance);
        //}
        //获取模块API
        //public static T Get<T>() where T : class
        //{
        //    MakeSureArchitecture();
        //    return _architecture.mContainer.Get<T>(); //因为是静态方法，所以需要_architecture.
        //}

        #region IArchitecture实现
        public  TUtility GetUtility<TUtility>() where TUtility : class
        {
            return _architecture.mContainer.Get<TUtility>();
        }
        public  void RegisterUtility<TUtility>(TUtility UtilityMoudle)
        {
            _architecture.mContainer.Register<TUtility>(UtilityMoudle);
        }
        //这里引入了，一个 Model 初始化的机制，分别有判断是否已经初始化的 mInited 变量
        //，还有用于记录将要初始化的 Model 的 mModels List，还增加了初始化逻辑以及相应的判断等。
        public void RegisterModel<TModel>(TModel Modelmodule) where TModel : IModel
        {
            //给Model赋值
            Modelmodule.SetArchitecture(this);
            mContainer.Register<TModel>(Modelmodule);
            if (_inited) //如果已经初始化完成
            {
                Modelmodule.Init();
            }
            else
            {
                _Models.Add(Modelmodule);
            }
        }
        public TModel GetModel<TModel>() where TModel : class, IModel
        {
            return mContainer.Get<TModel>();
        }

        public void RegisterSystem<TSystem>(TSystem SystemmMdule) where TSystem : ISystem
        {
            SystemmMdule.SetArchitecture(this);
            mContainer.Register<TSystem>(SystemmMdule);
            if (_inited)
            {
                SystemmMdule.Init();
            }
            else
            {
                _Systems.Add(SystemmMdule);
            }
        }
        TSystem IArchitecture.GetSystem<TSystem>()
        {
            return mContainer.Get<TSystem>();
        }
        public void SendCommand<TCommand>() where TCommand : ICommand, new() //执行命令
        {
            var command = new TCommand();
            command.SetArchitecture(this);
            command.Excute();
        }

        public void SendCommand<TCommand>(TCommand Command) where TCommand : ICommand
        {
            Command.SetArchitecture(this);
            Command.Excute();
        }
        #endregion
        #region TypeEventSystem Module
        private ITypeEventSystem _typeEventSystem = new TypeEventSystem();
        public void SendEvent<TEvent>() where TEvent : new()
        {
            _typeEventSystem.Send(new TEvent());
        }

        public void SendEvent<TEvent>(TEvent e)
        {
            _typeEventSystem.Send<TEvent>(e);
        }

        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            return _typeEventSystem.Register<TEvent>(onEvent);
        }

        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            _typeEventSystem.UnRegister<TEvent>(onEvent);
        }
        #endregion

        #region Query Module
        public TResult SendQuery<TResult>(IQuery<TResult> query)
        {
            query.SetArchitecture(this);
            return query.Do();
        }
        #endregion
    }
}
