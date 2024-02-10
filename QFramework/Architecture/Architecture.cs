using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace QFramework
{
    //����һ���ܹ���˵����Ҫע����кܶ࣬�����ܽ�����з��࣬Utility�����ࣨ�����߼�����Model�����ࣨ���ݲ㣩
    public interface IArchitecture
    {
        // �ṩһ����ȡ Utility �� API
        TUtility GetUtility<TUtility>() where TUtility : class; //���Utility
        TModel GetModel<TModel>() where TModel : class, IModel; //���Model
        TSystem GetSystem<TSystem>() where TSystem : class, ISystem;//���System
        void RegisterUtility<TUtility>(TUtility UtilityModule);//ע��Utility,����IStoragy
        void RegisterModel<TModel>(TModel ModelModule) where TModel : IModel;//ע��Model,����counterApp
        void RegisterSystem<TSystem>(TSystem SystemmMdule) where TSystem : ISystem;
        void SendCommand<TCommand>() where TCommand : ICommand,new (); //��������
        void SendCommand<TCommand>(TCommand Command) where TCommand : ICommand; //��������
        void SendEvent<TEvent>() where TEvent : new(); //�����¼�
        void SendEvent<TEvent>(TEvent e); //�����¼�
        IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent); //ע���¼�
        void UnRegisterEvent<TEvent>(Action<TEvent> onEvent); //ע���¼�
        TResult SendQuery<TResult>(IQuery<TResult> query); //��ѯ����
    }
    public abstract class Architecture<T>: IArchitecture where T : Architecture<T>, new()
    { 
        private static T _architecture = null; //�洢������Ϊ T �� Architecture<T> ���ʵ��
        /// <summary>
        /// �ṩһ�������ӿ�,��Controller���ܹ�����Architecture�ڲ���Model,Utility,System��
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
        private bool _inited = false; //�Ƿ��Ѿ���ʼ�����
        private List<IModel> _Models = new List<IModel>(); //���ڴ��Models����
        public static Action<T> OnRegisterPatch = architecture => { };
        private List<ISystem> _Systems = new List<ISystem>();
        private static void MakeSureArchitecture() //��̬����
        {
            if (_architecture == null)
            {
                _architecture = new T();
                _architecture.Init();

                OnRegisterPatch?.Invoke(_architecture);
                //��ʼ��Model
                foreach(var architectureModel in _architecture._Models)
                {
                    architectureModel.Init();
                }
                //���Model
                _architecture._Models.Clear();
                //��ʼ��System
                foreach(var architectureSystem in _architecture._Systems)
                {
                    architectureSystem.Init();
                }
                //���System
                _architecture._Systems.Clear();
                _architecture._inited = true;
            }
        }
        private IOCContainer mContainer = new IOCContainer();
        //�ṩһ������ ע��ģ��
        protected abstract  void Init();

        //ע��ģ���API
        //public static void Register<T>(T instance)
        //{
        //    MakeSureArchitecture();
        //    _architecture.mContainer.Register(instance);
        //}
        //��ȡģ��API
        //public static T Get<T>() where T : class
        //{
        //    MakeSureArchitecture();
        //    return _architecture.mContainer.Get<T>(); //��Ϊ�Ǿ�̬������������Ҫ_architecture.
        //}

        #region IArchitectureʵ��
        public  TUtility GetUtility<TUtility>() where TUtility : class
        {
            return _architecture.mContainer.Get<TUtility>();
        }
        public  void RegisterUtility<TUtility>(TUtility UtilityMoudle)
        {
            _architecture.mContainer.Register<TUtility>(UtilityMoudle);
        }
        //���������ˣ�һ�� Model ��ʼ���Ļ��ƣ��ֱ����ж��Ƿ��Ѿ���ʼ���� mInited ����
        //���������ڼ�¼��Ҫ��ʼ���� Model �� mModels List���������˳�ʼ���߼��Լ���Ӧ���жϵȡ�
        public void RegisterModel<TModel>(TModel Modelmodule) where TModel : IModel
        {
            //��Model��ֵ
            Modelmodule.SetArchitecture(this);
            mContainer.Register<TModel>(Modelmodule);
            if (_inited) //����Ѿ���ʼ�����
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
        public void SendCommand<TCommand>() where TCommand : ICommand, new() //ִ������
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
