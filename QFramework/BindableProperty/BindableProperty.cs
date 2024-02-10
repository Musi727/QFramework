using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    /// <summary>
    /// 可绑定属性
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
                if (value != null && value.Equals(value)) return; //当value为空时是没有Equals方法的
                _value = value;
                OnValueChanged?.Invoke(value);
            }
        }
        public Action<T> OnValueChanged = (e) => { }; //默认委托为空
        public IUnRegister Register(Action<T> onValueChanged) // 通过该方法为BindableProperty注册事件，并且返回该该事件（用于注销）
        {
            OnValueChanged += onValueChanged;
            return new BindablePropertyUnRegister<T>()
            {
                BindableProperty = this,
                OnValueChanged = onValueChanged
            };
        }
        /// <summary>
        /// 当注册的时候直接使用该初始值调用一次OnValueChanged，然后执行Register
        /// </summary>
        public IUnRegister RegisterWithInValue(Action<T> onValueChanged)
        {
            onValueChanged(_value);
            return Register(onValueChanged);
        }

        #region 优化BindableProperty的使用
        /// <summary>
        /// implicit 关键字表示这个转换应该是隐式的，意味着编译器会在必要时自动执行此转换，而不需要显式的强制类型转换。
        /// 它简单地返回 BindableProperty<T> 实例的 Value 属性，这个属性预计保存了类型为 T 的值。
        /// 直接使用 a == b，而不用 a.Value == b.Value
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

        public void UnRegister(Action<T> onValueChanged) // 注销BindableProperty上的事件
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
