using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    /// <summary>
    /// �ɰ�����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BindableProperty <T> 
    {
        public BindableProperty(T defaultValue = default(T))
        {
            _value = defaultValue;
        }
        private T _value = default(T);
        public T Value
        {
            get => _value;
            set
            {
                if (value == null && _value == null) return;
                if (value != null && value.Equals(value)) return; //��valueΪ��ʱ��û��Equals������
                _value = value;
                OnValueChanged?.Invoke(value);
            }
        }
        public Action<T> OnValueChanged = (e) => { }; //Ĭ��ί��Ϊ��
        public IUnRegister Register(Action<T> onValueChanged) // ͨ���÷���ΪBindablePropertyע���¼������ҷ��ظø��¼�������ע����
        {
            OnValueChanged += onValueChanged;
            return new BindablePropertyUnRegister<T>()
            {
                BindableProperty = this,
                OnValueChanged = onValueChanged
            };
        }
        /// <summary>
        /// ��ע���ʱ��ֱ��ʹ�øó�ʼֵ����һ��OnValueChanged��Ȼ��ִ��Register
        /// </summary>
        public IUnRegister RegisterWithInValue(Action<T> onValueChanged)
        {
            onValueChanged(_value);
            return Register(onValueChanged);
        }

        #region �Ż�BindableProperty��ʹ��
        /// <summary>
        /// implicit �ؼ��ֱ�ʾ���ת��Ӧ������ʽ�ģ���ζ�ű��������ڱ�Ҫʱ�Զ�ִ�д�ת����������Ҫ��ʽ��ǿ������ת����
        /// ���򵥵ط��� BindableProperty<T> ʵ���� Value ���ԣ��������Ԥ�Ʊ���������Ϊ T ��ֵ��
        /// ֱ��ʹ�� a == b�������� a.Value == b.Value
        /// </summary>
        /// <param name="property"></param>
        public static implicit operator T(BindableProperty<T> property)
        {
            return property.Value;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
        #endregion

        public void UnRegister(Action<T> onValueChanged) // ע��BindableProperty�ϵ��¼�
        {
            OnValueChanged -= onValueChanged;
        }
    }
    public class BindablePropertyUnRegister<T> : IUnRegister
    {
        public BindableProperty<T> BindableProperty { get; set; }

        public Action<T> OnValueChanged { get; set; }

        public void UnRegister()
        {
            BindableProperty.UnRegister(OnValueChanged);
        }
    }
}
