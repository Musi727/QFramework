using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public interface ITypeEventSystem //声明一个事件接口,继承该接口必须实现这些方法
    {
        void Send<T>() where T : new();
        void Send<T>(T e);
        IUnRegister Register<T>(Action<T> onEvent);//当注册的时候返回一个注销对象，避免忘记注销

        void UnRegister<T>(Action<T> onEvent);
    }
    public class TypeEventSystem : ITypeEventSystem
    {
        /// <summary>
        /// 事件消息容器
        /// </summary>
        interface IRegistrations { } //封装泛型类的接口：里氏替换原则
        class Registrations<T> : IRegistrations //封装了一层能够使用泛型了，但是仍然在需要一次封装 IRegistrations
        {
            //OnEvent 字段在默认情况下指向一个不执行任何操作的方法，但是您可以在运行时将其更改为指向任何类型为 Action<T> 的委托
            public Action<T> OnEvent = (e) => { };
        }

        //该字典容器需要能存储 泛型委托，但是TypeEventSystem不能有泛型，所以需要进行一次泛型封装 Registrations
        private Dictionary<Type, IRegistrations> _eventRegisters = new Dictionary<Type, IRegistrations> ();
        public IUnRegister Register<T>(Action<T> onEvent)
        {
            //获取了类型参数 T 的实际类型
            var type =  typeof(T);
            //用于存储委托的变量
            IRegistrations eventRegisstrations;
            //根据给定的键来获取相应的值。如果找到了与键匹配的值 返回true
            //并将该值存储在传入的第二个参数中；如果找不到与键匹配的值，则返回 false，并且传入的第二个参数将会是默认值（比如 null）
            if (_eventRegisters.TryGetValue(type, out eventRegisstrations))
            {

            }
            else //如果字典中没有找到
            {
                eventRegisstrations = new Registrations<T>(); //声明一个空委托容器
                _eventRegisters.Add(type, eventRegisstrations); //在字典中添加键值对
            }
            (eventRegisstrations as Registrations<T>).OnEvent += onEvent; //在委托容器中添加委托

            return new TypeEventSystemUnRegister<T>()
            {
                OnEvent = onEvent,
                TypeEventSystem = this //事件基类System
            };
        }

        public void Send<T>() where T : new()
        {
            //通过 T 的默认构造函数创建一个新的实例，并将该实例传递给另一个重载的 Send 方法进行发送。
            var e = new T();
            Send<T>(e);
        }

        public void Send<T>(T e)
        {
            var type = typeof (T);
            IRegistrations eventRegisrations; //声明存储委托的容器
            if(_eventRegisters.TryGetValue(type,out eventRegisrations))
            {
                //如果在字典容器中，找到了键，那么就执行 值（委托容器）中的所有委托,传入参数
                (eventRegisrations as Registrations<T>).OnEvent?.Invoke(e);
            }
        }

        public void UnRegister<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations eventRegistrations;
            //如果字典中找到了 键，就会将 键对应的值 存入 eventRegistrations
            if (_eventRegisters.TryGetValue (type, out eventRegistrations))
            {
                //将值（委托容器）中的委托 删除
                (eventRegistrations as Registrations<T>).OnEvent -= onEvent;
            }
        }
    }
    #region 事件基类扩展内容
    public interface IUnRegister //该类型的对象 为 注销对象
    {
        void UnRegister();
    }
    /// <summary>
    /// 注销接口里包含 委托、
    /// </summary>
    public class TypeEventSystemUnRegister<T> : IUnRegister
    {
        public ITypeEventSystem TypeEventSystem { get; set; } 
        public Action<T> OnEvent { get; set; }

        public void UnRegister()
        {
            TypeEventSystem.UnRegister(OnEvent);

            TypeEventSystem = null;
            OnEvent = null;
        }
    }
    /// <summary>
    /// 注销的触发器，需要挂载在gameObject对象上，当GameObject销毁，会自动将注册的消息事件全部注销
    /// </summary>
    public class UnRegisterOnDestroyTrigger : MonoBehaviour
    {
        //声明一个无序的、不包含重复元素的集合HasSet,存储IUnRegister
        private HashSet<IUnRegister> _UnRegisters = new HashSet<IUnRegister>();
        public void AddUnRegister(IUnRegister unRegister)//向容器中添加需要注销的对象
        {
            _UnRegisters.Add(unRegister);
        }
        void OnDestroy()
        {
            foreach(var unRegister in _UnRegisters) //遍历容器的对象，执行注销操作
            {
                unRegister.UnRegister();
            }
            _UnRegisters.Clear(); //清空容器
        }
    }
    public static class UnRegisterExtension
    {
        //拓展函数：为IUnRegister类添加一个方法：该方法需要需要传入也给参数：GameObejct go;
        public static void UnRegisterWhenGameObjectDestroyed(this IUnRegister unRegister,GameObject gameObject)
        {
            var trigger = gameObject.GetComponent<UnRegisterOnDestroyTrigger>(); //获得gameObject上的触发器
            if (!trigger)
            {
                //如果触发器为空,则主动添加注销脚本
                trigger = gameObject.AddComponent<UnRegisterOnDestroyTrigger>();
            }
            trigger.AddUnRegister(unRegister); //向触发器添加一个注销对象IUnRegister unRegister
        }
    }
    #endregion
}
