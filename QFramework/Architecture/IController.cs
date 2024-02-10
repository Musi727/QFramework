using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public interface IController : IBelongTotArchitecture, ICanGetSystem,ICanGetModel,ICanSendCommand,ICanRegisterEvent,ICanSendQuery
    {
        
    }
}
