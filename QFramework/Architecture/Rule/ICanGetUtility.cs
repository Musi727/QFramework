using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QFramework
{
    /// <summary>
    /// �̳иýӿڲ��ܷ���Utility
    /// </summary>
    public interface ICanGetUtility : IBelongTotArchitecture
    {
    }
    public static class CanGetUtilityExtension
    {
        //��չ���� ��һ���������ͼ���չ�����ͣ�������this�ؼ�������
        //Ϊ���м̳���ICanGetUtility�Ľӿ���չһ��������GetUtility
        public static T GetUtility<T>(this ICanGetUtility self) where T : class, IUtility
        {
            return self.GetArchitecture().GetUtility<T>();
        }
    }
}