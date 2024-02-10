using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QFramework
{
    /// <summary>
    /// 继承该接口才能访问Utility
    /// </summary>
    public interface ICanGetUtility : IBelongTotArchitecture
    {
    }
    public static class CanGetUtilityExtension
    {
        //拓展函数 第一个参数类型即拓展的类型，必须用this关键词修饰
        //为所有继承了ICanGetUtility的接口拓展一个方法：GetUtility
        public static T GetUtility<T>(this ICanGetUtility self) where T : class, IUtility
        {
            return self.GetArchitecture().GetUtility<T>();
        }
    }
}