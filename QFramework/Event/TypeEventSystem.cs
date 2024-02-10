using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public interface ITypeEventSystem //����һ���¼��ӿ�,�̳иýӿڱ���ʵ����Щ����
    {
        void Send<T>() where T : new();
        void Send<T>(T e);
        IUnRegister Register<T>(Action<T> onEvent);//��ע���ʱ�򷵻�һ��ע�����󣬱�������ע��

        void UnRegister<T>(Action<T> onEvent);
    }
    public class TypeEventSystem : ITypeEventSystem
    {
        /// <summary>
        /// �¼���Ϣ����
        /// </summary>
        interface IRegistrations { } //��װ������Ľӿڣ������滻ԭ��
        class Registrations<T> : IRegistrations //��װ��һ���ܹ�ʹ�÷����ˣ�������Ȼ����Ҫһ�η�װ IRegistrations
        {
            //OnEvent �ֶ���Ĭ�������ָ��һ����ִ���κβ����ķ���������������������ʱ�������Ϊָ���κ�����Ϊ Action<T> ��ί��
            public Action<T> OnEvent = (e) => { };
        }

        //���ֵ�������Ҫ�ܴ洢 ����ί�У�����TypeEventSystem�����з��ͣ�������Ҫ����һ�η��ͷ�װ Registrations
        private Dictionary<Type, IRegistrations> _eventRegisters = new Dictionary<Type, IRegistrations> ();
        public IUnRegister Register<T>(Action<T> onEvent)
        {
            //��ȡ�����Ͳ��� T ��ʵ������
            var type =  typeof(T);
            //���ڴ洢ί�еı���
            IRegistrations eventRegisstrations;
            //���ݸ����ļ�����ȡ��Ӧ��ֵ������ҵ������ƥ���ֵ ����true
            //������ֵ�洢�ڴ���ĵڶ��������У�����Ҳ������ƥ���ֵ���򷵻� false�����Ҵ���ĵڶ�������������Ĭ��ֵ������ null��
            if (_eventRegisters.TryGetValue(type, out eventRegisstrations))
            {

            }
            else //����ֵ���û���ҵ�
            {
                eventRegisstrations = new Registrations<T>(); //����һ����ί������
                _eventRegisters.Add(type, eventRegisstrations); //���ֵ�����Ӽ�ֵ��
            }
            (eventRegisstrations as Registrations<T>).OnEvent += onEvent; //��ί�����������ί��

            return new TypeEventSystemUnRegister<T>()
            {
                OnEvent = onEvent,
                TypeEventSystem = this //�¼�����System
            };
        }

        public void Send<T>() where T : new()
        {
            //ͨ�� T ��Ĭ�Ϲ��캯������һ���µ�ʵ����������ʵ�����ݸ���һ�����ص� Send �������з��͡�
            var e = new T();
            Send<T>(e);
        }

        public void Send<T>(T e)
        {
            var type = typeof (T);
            IRegistrations eventRegisrations; //�����洢ί�е�����
            if(_eventRegisters.TryGetValue(type,out eventRegisrations))
            {
                //������ֵ������У��ҵ��˼�����ô��ִ�� ֵ��ί���������е�����ί��,�������
                (eventRegisrations as Registrations<T>).OnEvent?.Invoke(e);
            }
        }

        public void UnRegister<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations eventRegistrations;
            //����ֵ����ҵ��� �����ͻὫ ����Ӧ��ֵ ���� eventRegistrations
            if (_eventRegisters.TryGetValue (type, out eventRegistrations))
            {
                //��ֵ��ί���������е�ί�� ɾ��
                (eventRegistrations as Registrations<T>).OnEvent -= onEvent;
            }
        }
    }
    #region �¼�������չ����
    public interface IUnRegister //�����͵Ķ��� Ϊ ע������
    {
        void UnRegister();
    }
    /// <summary>
    /// ע���ӿ������ ί�С�
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
    /// ע���Ĵ���������Ҫ������gameObject�����ϣ���GameObject���٣����Զ���ע�����Ϣ�¼�ȫ��ע��
    /// </summary>
    public class UnRegisterOnDestroyTrigger : MonoBehaviour
    {
        //����һ������ġ��������ظ�Ԫ�صļ���HasSet,�洢IUnRegister
        private HashSet<IUnRegister> _UnRegisters = new HashSet<IUnRegister>();
        public void AddUnRegister(IUnRegister unRegister)//�������������Ҫע���Ķ���
        {
            _UnRegisters.Add(unRegister);
        }
        void OnDestroy()
        {
            foreach(var unRegister in _UnRegisters) //���������Ķ���ִ��ע������
            {
                unRegister.UnRegister();
            }
            _UnRegisters.Clear(); //�������
        }
    }
    public static class UnRegisterExtension
    {
        //��չ������ΪIUnRegister�����һ���������÷�����Ҫ��Ҫ����Ҳ��������GameObejct go;
        public static void UnRegisterWhenGameObjectDestroyed(this IUnRegister unRegister,GameObject gameObject)
        {
            var trigger = gameObject.GetComponent<UnRegisterOnDestroyTrigger>(); //���gameObject�ϵĴ�����
            if (!trigger)
            {
                //���������Ϊ��,���������ע���ű�
                trigger = gameObject.AddComponent<UnRegisterOnDestroyTrigger>();
            }
            trigger.AddUnRegister(unRegister); //�򴥷������һ��ע������IUnRegister unRegister
        }
    }
    #endregion
}
